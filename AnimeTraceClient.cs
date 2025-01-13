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
        private string _model = null;
        private HttpClient _customeClient = null;

        public AnimeTraceClient(AnimeTraceModels model, HttpClient customeClient = null)
        {
            _model = model.ToString();
            _customeClient = customeClient;
        }

        public AnimeTraceClient(string model, HttpClient customeClient = null)
        {
            _model = model;
            _customeClient = customeClient;
        }

        public async Task<AnimeTraceResult> RecognizeAsync(string imageFileName, bool aiDetect = false, bool isMulti = false)
        {
            using FileStream fs = new FileStream(imageFileName, FileMode.Open, FileAccess.Read);
            return await RecognizeAsync(fs, aiDetect, isMulti);
        }

        public async Task<AnimeTraceResult> RecognizeAsync(byte[] image, bool aiDetect = false, bool isMulti = false)
        {
            using MemoryStream ms = new MemoryStream(image);
            return await RecognizeAsync(ms, aiDetect, isMulti);
        }

        public async Task<AnimeTraceResult> RecognizeAsync(Stream image, bool aiDetect = false, bool isMulti = false)
        {
            if (string.IsNullOrWhiteSpace(_model))
                throw new NotImplementedException("模型不能为空");

            using HttpClient client = _customeClient ?? new HttpClient();
            using StreamContent streamContent = new(image);
            streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"image\"; filename=\"img.png\"");
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"image/png");

            using MultipartFormDataContent content = new($"--------------------------{DateTime.Now.Ticks}")
            {
                { new StringContent(_model), "model" },
                { new StringContent(aiDetect ? "1":"0"), "ai_detect" },
                { streamContent },
                { new StringContent(isMulti? "1":"0"), "is_multi" },
            };

            var response = await client.PostAsync("https://aiapiv2.animedb.cn/ai/api/detect", content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"{(int)response.StatusCode} {response.StatusCode}");
            }
            string json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
                throw new Exception("接口返回内容为空，请联系作者：https://ai.animedb.cn/");

            AnimeTraceResult result = JsonConvert.DeserializeObject<AnimeTraceResult>(json);

            if (result is null)
                throw new NotImplementedException($"接口响应解析失败，请更新SDK版本。\r\n{json}");

            int code = result.Code == 0 ? result.NewCode : result.Code;

            result.Success = code is 17720 or 200;
            result.Message = code switch
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
                17720 or 200 => "识别成功",
                _ => $"未知的错误状态码 {result.NewCode}",
            };

            return result;
        }
    }
}
