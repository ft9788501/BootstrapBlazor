// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Shared.Samples;

/// <summary>
/// 
/// </summary>
public partial class Recognizers
{
    [Inject]
    [NotNull]
    private IEnumerable<IRecognizerProvider>? RecognizerProviders { get; set; }

    [NotNull]
    private IRecognizerProvider? RecognizerProvider { get; set; }

    private IRecognizerProvider? AzureProvider { get; set; }

    private bool Start { get; set; }

    private string? Result { get; set; }

    private string ButtonText { get; set; } = "开始识别";

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        RecognizerProvider = RecognizerProviders.OfType<AzureRecognizerProvider>().FirstOrDefault();
    }

    private async Task OnStart()
    {
        if (ButtonText == "开始识别")
        {
            Start = true;
            ButtonText = "结束识别";
            await RecognizerProvider.AzureRecognizeOnceAsync(Recognize);
        }
        else
        {
            await Close();
        }
    }

    private async Task OnTimeout()
    {
        await Close();
    }

    private Task Recognize(string result)
    {
        Result = result;
        Start = false;
        ButtonText = "开始识别";
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task Close()
    {
        await RecognizerProvider.AzureCloseAsync(Recognize);
    }
}
