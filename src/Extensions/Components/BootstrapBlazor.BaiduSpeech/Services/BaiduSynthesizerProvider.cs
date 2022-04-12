// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace BootstrapBlazor.Components;

/// <summary>
/// 
/// </summary>
internal class BaiduSynthesizerProvider : ISynthesizerProvider, IAsyncDisposable
{
    [NotNull]
    public string? Name { get; set; } = "Baidu";

    private DotNetObjectReference<BaiduSynthesizerProvider>? Interop { get; set; }

    private IJSObjectReference? Module { get; set; }

    [NotNull]
    private SynthesizerOption? Option { get; set; }

    private BaiduSpeechOption SpeechOption { get; }

    private IJSRuntime JSRuntime { get; }

    private Baidu.Aip.Speech.Tts Client { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options"></param>
    /// <param name="runtime"></param>
    public BaiduSynthesizerProvider(IOptions<BaiduSpeechOption> options, IJSRuntime runtime)
    {
        JSRuntime = runtime;
        SpeechOption = options.Value;
        Client = new Baidu.Aip.Speech.Tts(SpeechOption.ApiKey, SpeechOption.Secret);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public async Task InvokeAsync(SynthesizerOption option)
    {
        if (!string.IsNullOrEmpty(option.Text))
        {
            Option = option;
            if (!string.IsNullOrEmpty(option.MethodName))
            {
                if (Module == null)
                {
                    Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BootstrapBlazor.AzureSpeech/js/synthesizer.js");
                }
                Interop ??= DotNetObjectReference.Create(this);
                await Module.InvokeVoidAsync(Option.MethodName!, Interop, nameof(Callback), Option.Text);
            }
            else
            {

                var result = Client.Synthesis(option.Text, new Dictionary<string, object>
                {
                    {"spd", 5}, // 语速
                    {"vol", 7}, // 音量
                    {"per", 4}  // 发音人，4：情感度丫丫童声
                });
            }

        }
        else
        {
            if (option.Callback != null)
            {
                await option.Callback(SynthesizerStatus.Close);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task Callback(SynthesizerStatus status)
    {
        if (Option.Callback != null)
        {
            await Option.Callback(status);
        }
    }

    /// <summary>
    /// DisposeAsync 方法
    /// </summary>
    /// <param name="disposing"></param>
    private async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing)
        {
            if (Interop != null)
            {
                Interop.Dispose();
            }
            if (Module is not null)
            {
                await Module.DisposeAsync();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }
}
