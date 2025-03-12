# AnimeTrace_Net_SDK
DotNet SDK FOR AnimeTrace.  AnimeTrace的.Net SDK。

本项目是 [AnimeTrace](https://www.animetrace.com/) 的第三方SDK

本项目基于 .Net Standard 2.0 编写，您至少要使用 .Net Framework 4.6.1 或 .Net Core 2.0 或更高版本的框架才可以引用

## 如何安装

1. 如果你使用Visual Studio，请在nuget中搜索AnimeTrace_Net_SDK并安装

2. DotNet CLI
```shell
dotnet add package AnimeTrace_Net_SDK
```
3. PowerShell
```shell
Install-Package AnimeTrace_Net_SDK
```

## 如何使用

```CSharp
    AnimeTraceClient client = new AnimeTraceClient(AnimeTraceModels.Anime);  //实例化客户端并选择模型
    AnimeTraceResult result = await client.SearchAsync(img);  //传递图片(流/字节/路径)进行搜索
    //AnimeTraceResult result = await client.SearchByUrlAsync(string);  //传递图片链接进行搜索
    //AnimeTraceResult result = await client.SearchByBase64Async(string);  //传递图片Base64进行搜索
    foreach (var item in result.Data)
    {
        var character = item.Character.First();
        Console.WriteLine($"动画名称：{character.Work}");
        Console.WriteLine($"角色名称：{character.Character}");
        Console.WriteLine();
    }
```