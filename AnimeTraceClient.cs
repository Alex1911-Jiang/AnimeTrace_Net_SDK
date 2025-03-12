using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnimeTrace_Net_SDK
{
    public class AnimeTraceClient
    {
        private readonly string _model;
        private readonly HttpClient _customeClient = null;

        public AnimeTraceClient(AnimeTraceModels model, HttpClient customeClient = null) : this(model.ToString(), customeClient)
        { 
        }

        public AnimeTraceClient(string model, HttpClient customeClient = null)
        {
            _model = model;
            _customeClient = customeClient ?? new HttpClient();
            if (string.IsNullOrWhiteSpace(_model))
                throw new NotImplementedException("模型不能为空");
        }

        public async Task<AnimeTraceResult> SearchByBase64Async(string base64, bool aiDetect = false, bool isMulti = false)
        {
            using MultipartFormDataContent content = new($"--------------------------{DateTime.Now.Ticks}") { { new StringContent(base64), "base64" } };
            return await SearchAsync(content, aiDetect, isMulti);
        }

        public async Task<AnimeTraceResult> SearchByUrlAsync(string url, bool aiDetect = false, bool isMulti = false)
        {
            using MultipartFormDataContent content = new($"--------------------------{DateTime.Now.Ticks}") { { new StringContent(url), "url" } };
            return await SearchAsync(content, aiDetect, isMulti);
        }

        public async Task<AnimeTraceResult> SearchAsync(string imageFileName, bool aiDetect = false, bool isMulti = false)
        {
            using FileStream fs = new(imageFileName, FileMode.Open, FileAccess.Read);
            return await SearchAsync(fs, aiDetect, isMulti);
        }

        public async Task<AnimeTraceResult> SearchAsync(byte[] image, bool aiDetect = false, bool isMulti = false)
        {
            using MemoryStream ms = new(image);
            return await SearchAsync(ms, aiDetect, isMulti);
        }

        public async Task<AnimeTraceResult> SearchAsync(Stream image, bool aiDetect = false, bool isMulti = false)
        {
            using StreamContent streamContent = new(image);
            streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"file\"; filename=\"img.png\"");
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"image/png");
            using MultipartFormDataContent content = new($"--------------------------{DateTime.Now.Ticks}") { { streamContent } };
            return await SearchAsync(content, aiDetect, isMulti);
        }

        private async Task<AnimeTraceResult> SearchAsync(MultipartFormDataContent content, bool aiDetect, bool isMulti)
        {
            content.Add(new StringContent(_model), "model");
            content.Add(new StringContent(aiDetect ? "1" : "2"), "ai_detect");
            content.Add(new StringContent(isMulti ? "1" : "0"), "is_multi");

            var response = await _customeClient.PostAsync("https://api.animetrace.com/v1/search", content);
            string json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
                throw new Exception("接口返回内容为空，请联系作者：https://www.animetrace.com/");

            AnimeTraceResult result;
            try
            {
                result = JsonConvert.DeserializeObject<AnimeTraceResult>(json);
                if (result is null)
                    throw new NullReferenceException();
            }
            catch (Exception)
            {
                throw new NotImplementedException($"接口响应解析失败，请更新SDK版本。\r\n{json}");
            }

            if (string.IsNullOrWhiteSpace(result.Detail))
            {
                result.Success = result.Code is 0 or 17720 or 17721 or 200;
                result.Message = result.Code switch
                {
                    17701 => "图片大小过大",
                    17702 => "服务器繁忙，请重试",
                    17799 => "不明错误发生",
                    17703 => "请求参数不正确",
                    17704 => "API维护中",
                    17705 => "图片格式不支持",
                    17706 => "识别无法完成（内部错误，请重试）",
                    17707 => "内部错误",
                    17708 => "图片中的人物数量超过限制",
                    17709 => "无法加载统计数量",
                    17710 => "图片验证码错误",
                    17711 => "无法完成识别前准备工作（请重试）",
                    17712 => "需要图片名称",
                    0 or 200 or 17720 or 17721 => "识别成功",
                    17722 => "图片下载失败",
                    17723 => "未指定 Content-Length",
                    17724 => "不是图片文件或未指定",
                    17725 => "未指定图片",
                    17726 => "JSON 不接受包含文件",
                    17727 => "Base64 格式错误",
                    17728 => "已达到本次使用上限",
                    17729 => "未找到选择的模型",
                    17730 => "检测 AI 图片失败",
                    17731 => "服务利用人数过多，请重新尝试",
                    404 => "页面不存在",
                    17732 => "已过期",
                    17733 => "反馈成功",
                    17734 => "反馈失败",
                    17735 => "反馈识别效果成功",
                    17736 => "验证码错误",
                    _ => $"未知的错误状态码 {result.Code}",
                };
            }
            else
            {
                result.Success = false;
                result.Message = result.Detail;
            }
            return result;
        }
    }
}
