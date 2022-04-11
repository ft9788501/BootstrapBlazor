// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapBlazor.Shared.Samples;

/// <summary>
/// 
/// </summary>
public partial class BaiduSpeech
{
    [Inject]
    [NotNull]
    private RecognizerService? RecognizerService { get; set; }

    private string? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await RecognizerService.RecognizeInit((s) =>
        {
            return Task.CompletedTask;
        });
    }

    private async Task OnStart()
    {
        await RecognizerService.RecognizeStart((s) =>
        {
            return Task.CompletedTask;
        });
    }

    private async Task OnStop()
    {
        await RecognizerService.BaiduCloseAsync((s) =>
        {
            Value = s;
            StateHasChanged();
            return Task.CompletedTask;
        });
    }
}
