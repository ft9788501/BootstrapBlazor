// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Text;

namespace BootstrapBlazor.Components;

/// <summary>
/// 百度语音识别提供类
/// </summary>
public class BaiduRecognizerProvider : IRecognizerProvider, IAsyncDisposable
{
    private DotNetObjectReference<BaiduRecognizerProvider>? Interop { get; set; }

    private IJSObjectReference? Module { get; set; }

    [NotNull]
    private RecognizerOption? Option { get; set; }

    private BaiduSpeechOption SpeechOption { get; }

    private IJSRuntime JSRuntime { get; }

    private Baidu.Aip.Speech.Asr Client { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options"></param>
    /// <param name="runtime"></param>
    public BaiduRecognizerProvider(IOptions<BaiduSpeechOption> options, IJSRuntime runtime)
    {
        JSRuntime = runtime;
        SpeechOption = options.Value;
        Client = new Baidu.Aip.Speech.Asr(SpeechOption.AppId, SpeechOption.ApiKey, SpeechOption.Secret);
    }

    /// <summary>
    /// 回调方法
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task InvokeAsync(RecognizerOption option)
    {
        if (string.IsNullOrEmpty(option.MethodName))
        {
            throw new InvalidOperationException();
        }

        Option = option;
        if (Module == null)
        {
            Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/_content/BootstrapBlazor.BaiduSpeech/js/recognizer.js");
        }
        Interop ??= DotNetObjectReference.Create(this);
        await Module.InvokeVoidAsync(Option.MethodName, Interop, nameof(Callback), nameof(ReciveBuffers), nameof(TranslationOnce));
    }

    /// <summary>
    /// Callback 回调方法
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task Callback(string result)
    {
        if (Option.Callback != null)
        {
            await Option.Callback(result);
        }
    }

    /// <summary>
    /// ReciveBuffers 回调方法
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    [JSInvokable]
    public Task ReciveBuffers(object bytes)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// TranslationOnce 回调方法
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public async Task TranslationOnce(byte[] bytes)
    {
        var result = Client.Recognize(bytes, "wav", 16000);
        var sb = new StringBuilder();
        var text = result["result"].ToArray();
        foreach (var item in text)
        {
            sb.Append(item.ToString());
        }

        if (Option.Callback != null)
        {
            await Option.Callback(sb.ToString());
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
    /// DisposeAsync 方法
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }
}
