﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// RecognizerService 扩展方法
/// </summary>
public static class RecognizerServiceExtensions
{
    /// <summary>
    /// 语音识别方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static async Task RecognizeInit(this RecognizerService service, Func<string, Task> callback)
    {
        var option = new RecognizerOption()
        {
            MethodName = "bb_speech_init",
            Callback = callback
        };
        await service.InvokeAsync("Baidu", option);
    }

    /// <summary>
    /// 语音识别方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static async Task RecognizeStart(this RecognizerService service, Func<string, Task> callback)
    {
        var option = new RecognizerOption()
        {
            MethodName = "bb_speech_start",
            Callback = callback
        };
        await service.InvokeAsync("Baidu", option);
    }

    /// <summary>
    /// 关闭语音识别方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static async Task BaiduCloseAsync(this RecognizerService service, Func<string, Task> callback)
    {
        var option = new RecognizerOption()
        {
            MethodName = "bb_speech_close",
            Callback = callback
        };
        await service.InvokeAsync("Baidu", option);
    }
}
