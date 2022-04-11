using System.Reflection;
using Microsoft.JSInterop;

namespace BootstrapBlazor.Components;

/// <summary>
/// Js模块引用
/// </summary>
public sealed class ResponsiveModule : IAsyncDisposable
{
    /// <summary>
    /// 版本号
    /// </summary>
    public static Lazy<Version?> Version => new(() => Assembly.GetExecutingAssembly()?.GetName()?.Version);

    /// <summary>
    /// 模块JS文件
    /// </summary>
    public static string JsModuleName => $"./_content/BootstrapBlazor.Responsive/responsive.min.js?v={Version.Value}";

    /// <summary>
    /// 指示是否可用。
    /// </summary>
    public bool Available => _module is not null;

    private IJSObjectReference? _module;

    private readonly Lazy<ValueTask<IJSObjectReference>> _moduleTask;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="jsRuntime"></param>
    public ResponsiveModule(IJSRuntime jsRuntime)
        => _moduleTask = new(() =>jsRuntime.InvokeAsync<IJSObjectReference>("import", JsModuleName));

    private async Task<bool> CreateModule()
    {
        if (!Available)
        {
            _module = await _moduleTask.Value;
        }

        return Available;
    }

    /// <summary>
    /// 注册断点监听
    /// </summary>
    /// <param name="dotNetObjectRef"></param>
    /// <param name="elementId"></param>
    /// <returns></returns>
    public async ValueTask RegisterBreakpoint(DotNetObjectReference<Responsive>? dotNetObjectRef, string? elementId)
    {
        if (string.IsNullOrEmpty(elementId) || dotNetObjectRef is null)
        {
            return;
        }

        if (await CreateModule())
        {
            await _module!.InvokeVoidAsync("registerBreakpointComponent", dotNetObjectRef, elementId);
        }
    }

    /// <summary>
    /// 注销断点监听
    /// </summary>
    /// <param name="elementId"></param>
    /// <returns></returns>
    public async ValueTask UnregisterBreakpoint(string? elementId)
    {
        if (string.IsNullOrEmpty(elementId))
        {
            return;
        }

        if (await CreateModule())
        {
            await _module!.InvokeVoidAsync("unregisterBreakpointComponent", elementId);
        }
    }

    /// <summary>
    /// 获取当前断点
    /// </summary>
    /// <returns></returns>
    public async ValueTask<string?> GetBreakpoint()
    {
        return await CreateModule()
            ? await _module!.InvokeAsync<string?>("getBreakpoint")
            : await Task.FromResult<string?>(null);
    }

    /// <summary>
    /// DisposeAsync 方法
    /// </summary>
    /// <param name="disposing"></param>
    private async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing && Available)
        {
            await _module!.DisposeAsync();
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
