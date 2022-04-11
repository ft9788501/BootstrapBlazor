// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Common;

namespace BootstrapBlazor.Shared.Samples;

/// <summary>
/// Responsives 类
/// </summary>
public partial class Responsives
{
    private IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
        new AttributeItem()
        {
            Name = nameof(Responsive.OnRenderCondition),
            Description = "获得/设置 是否触发内容刷新 返回 true 时刷新",
            Type = "Func<BreakPoint, BreakPoint, Task<bool>>",
            ValueList = " — ",
            DefaultValue = "true"
        },
        new AttributeItem()
        {
            Name = nameof(Responsive.ChildContent),
            Description = "获得/设置 子组件内容 ",
            Type = "RenderFragment",
            ValueList = " — ",
            DefaultValue = " — "
        }
    };
}
