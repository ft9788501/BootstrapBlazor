// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// 语音合成服务
/// </summary>
public class SynthesizerService
{
    private IEnumerable<ISynthesizerProvider> Providers { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="providers"></param>
    public SynthesizerService(IEnumerable<ISynthesizerProvider> providers)
    {
        Providers = providers;
    }

    /// <summary>
    /// 语音合成回调方法
    /// </summary>
    /// <param name="option"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task InvokeAsync(string name, SynthesizerOption option) => Providers.FirstOrDefault(s => s.Name == name)!.InvokeAsync(option);
}
