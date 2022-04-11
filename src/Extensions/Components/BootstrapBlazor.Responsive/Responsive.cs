// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BootstrapBlazor.Components;

/// <summary>
/// 响应监听 组件
/// </summary>
public class Responsive : IdComponentBase, IAsyncDisposable
{
    private ResponsiveModule? _jsModuleRef;

    private DotNetObjectReference<Responsive>? _dotNetObjectRef;

    private BreakPoint _breakpoint = BreakPoint.None;

    /// <summary>
    /// 获得/设置 是否触发内容刷新 返回 true 时刷新
    /// </summary>
    [Parameter]
    public Func<BreakPoint, BreakPoint, Task<bool>>? OnRenderCondition { get; set; }

    /// <summary>
    /// 获得/设置 子组件内容 
    /// </summary>
    [Parameter]
    public RenderFragment<BreakPoint>? ChildContent { get; set; }

    /// <summary>
    /// 已渲染
    /// </summary>
    public bool Rendered { get; private set; }

    /// <summary>
    /// 获取断点元素 id
    /// </summary>
    /// <returns></returns>
    [JSInvokable()]
    public string? GetElementId() => Id;

    /// <summary>
    /// 客户端通知断点已改变
    /// </summary>
    /// <param name="breakpoint">断点名称</param>
    /// <returns></returns>
    [JSInvokable()]
    public async Task OnBreakpoint(string? breakpoint)
    {
        if (IsChange(_breakpoint, breakpoint))
        {
            var lastBreakpoint = _breakpoint;
            _breakpoint = GetBreakpoint(breakpoint);

            if (OnRenderCondition == null || await OnRenderCondition(lastBreakpoint, _breakpoint))
            {
                await InvokeAsync(StateHasChanged);
            }
        }
    }

    /// <summary>
    /// 断点是否改变
    /// </summary>
    /// <param name="checkBreakpoint">要比对的断点</param>
    /// <param name="currentBreakpointString">当前断点名称</param>
    /// <returns></returns>
    public static bool IsChange(BreakPoint checkBreakpoint, string? currentBreakpointString)
        => checkBreakpoint != GetBreakpoint(currentBreakpointString);

    /// <summary>
    /// 根据名称获取 <see cref="BreakPoint"/> 断点。
    /// </summary>
    /// <param name="breakpoint">断点名称</param>
    /// <returns><see cref="BreakPoint"/></returns>
    private static BreakPoint GetBreakpoint(string? breakpoint)
        => breakpoint switch
        {
            "xs" => BreakPoint.Small,
            "sm" => BreakPoint.Small,
            "md" => BreakPoint.Medium,
            "lg" => BreakPoint.Large,
            "xl" => BreakPoint.ExtraLarge,
            "xxl" => BreakPoint.ExtraLarge,
            _ => BreakPoint.None,
        };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (ChildContent != null && Rendered)
        {
            builder.AddContent(0, ChildContent, _breakpoint);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _jsModuleRef = new ResponsiveModule(JSRuntime);
            _dotNetObjectRef = DotNetObjectReference.Create(this);

            Rendered = true;

            if (!string.IsNullOrEmpty(Id))
            {
                await _jsModuleRef!.RegisterBreakpoint(_dotNetObjectRef, Id);
            }
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
            if (_jsModuleRef is not null)
            {
                await _jsModuleRef.UnregisterBreakpoint(Id);
                await _jsModuleRef.DisposeAsync();
            }

            if (_dotNetObjectRef != null)
            {
                _dotNetObjectRef.Dispose();
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
