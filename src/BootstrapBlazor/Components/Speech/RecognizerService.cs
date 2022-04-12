// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// 语音识别服务
/// </summary>
public class RecognizerService
{
    private IEnumerable<IRecognizerProvider> Providers { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="providers"></param>
    public RecognizerService(IEnumerable<IRecognizerProvider> providers)
    {
        Providers = providers;
    }

    /// <summary>
    /// 语音识别回调方法
    /// </summary>
    /// <param name="option"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task InvokeAsync(string name, RecognizerOption option) => Providers.FirstOrDefault(p => p.Name
     == name)!.InvokeAsync(option);
}
