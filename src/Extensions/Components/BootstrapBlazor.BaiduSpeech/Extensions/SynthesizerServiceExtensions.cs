﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// SynthesizerService 扩展操作类
/// </summary>
public static class SynthesizerServiceExtensions
{
    /// <summary>
    /// 语音合成方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="text"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static async Task BaiduSynthesizerOnceAsync(this SynthesizerService service, string? text, Func<SynthesizerStatus, Task> callback)
    {
        var option = new SynthesizerOption()
        {
            Text = text,
            Callback = callback
        };
        await service.InvokeAsync(option);
    }

    /// <summary>
    /// 语音合成方法
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="text"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static async Task BaiduSynthesizerOnceAsync(this ISynthesizerProvider provider, string? text, Func<SynthesizerStatus, Task> callback)
    {
        var option = new SynthesizerOption()
        {
            Text = text,
            Callback = callback
        };
        await provider.InvokeAsync(option);
    }
}
