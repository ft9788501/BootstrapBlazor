﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    public partial class TableBase<TItem>
    {
        /// <summary>
        /// 获得 Checkbox 样式表集合
        /// </summary>
        /// <returns></returns>
        protected string? ButtonColumnClass => CssBuilder.Default("table-th-button")
            .Build();

        /// <summary>
        /// 获得/设置 删除按钮提示弹框实例
        /// </summary>
        protected PopoverConfirm? DeleteConfirm { get; set; }

        /// <summary>
        /// 获得/设置 删除按钮提示弹框实例
        /// </summary>
        protected PopoverConfirm? ButtonDeleteConfirm { get; set; }

        /// <summary>
        /// 获得/设置 编辑弹窗 Title 文字
        /// </summary>
        protected string? EditModalTitleString { get; set; }

        /// <summary>
        /// 获得/设置 被选中数据集合
        /// </summary>
        /// <value></value>
        protected List<TItem> SelectedItems { get; set; } = new List<TItem>();

        /// <summary>
        /// 获得/设置 被选中的数据集合
        /// </summary>
        public IEnumerable<TItem> SelectedRows => SelectedItems;

        /// <summary>
        /// 获得/设置 编辑表单实例
        /// </summary>
        protected ValidateForm? ValidateForm { get; set; }

        /// <summary>
        /// 获得/设置 编辑数据弹窗实例
        /// </summary>
        protected Modal? EditModal { get; set; }

        /// <summary>
        /// 获得/设置 编辑数据弹窗 Title
        /// </summary>
        [Parameter] public string EditModalTitle { get; set; } = "编辑数据窗口";

        /// <summary>
        /// 获得/设置 新建数据弹窗 Title
        /// </summary>
        [Parameter] public string AddModalTitle { get; set; } = "新建数据窗口";

        /// <summary>
        /// 获得/设置 新建数据弹窗 Title
        /// </summary>
        [Parameter] public string ColumnButtonTemplateHeaderText { get; set; } = "操作";

        /// <summary>
        /// 获得/设置 EditTemplate 实例
        /// </summary>
        [Parameter] public RenderFragment<TItem?>? EditTemplate { get; set; }

        /// <summary>
        /// 获得/设置 RowButtonTemplate 实例
        /// </summary>
        [Parameter] public RenderFragment<TItem>? RowButtonTemplate { get; set; }

        /// <summary>
        /// 获得/设置 EditModel 实例
        /// </summary>
        [Parameter] public TItem? EditModel { get; set; }

        /// <summary>
        /// 获得/设置 单选模式下点击行即选中本行 默认为 true
        /// </summary>
        [Parameter]
        public bool ClickToSelect { get; set; } = true;

        /// <summary>
        /// 获得/设置 单选模式下双击即编辑本行 默认为 false
        /// </summary>
        [Parameter]
        public bool DoubleClickToEdit { get; set; }

        /// <summary>
        /// 单选模式下选择行时调用此方法
        /// </summary>
        /// <param name="val"></param>
        protected virtual void OnSelectRow(TItem val)
        {
            SelectedItems.Clear();
            SelectedItems.Add(val);

            // TODO: 性能问题此处重新渲染整个 DataGrid
            // 合理做法是将 tbody 做成组件仅渲染 tbody 即可，后期优化此处 
            StateHasChanged();
        }

        /// <summary>
        /// 检查当前行是否被选中方法
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected virtual bool CheckActive(TItem val)
        {
            var ret = false;
            if (!IsMultipleSelect && ClickToSelect)
            {
                ret = SelectedItems.Contains(val);
            }
            return ret;
        }

        /// <summary>
        /// 查询按钮调用此方法
        /// </summary>
        /// <returns></returns>
        public async Task QueryAsync()
        {
            await QueryData();
            StateHasChanged();
        }

        /// <summary>
        /// 调用 OnQuery 回调方法获得数据源
        /// </summary>
        protected async Task QueryData()
        {
            SelectedItems.Clear();
            QueryData<TItem>? queryData = null;
            if (OnQueryAsync != null)
            {
                queryData = await OnQueryAsync(new QueryPageOptions()
                {
                    PageIndex = PageIndex,
                    PageItems = PageItems,
                    SearchText = SearchText,
                    SortOrder = SortOrder,
                    SortName = SortName,
                    Filters = Filters,
                    Searchs = Searchs
                });
            }
            if (queryData != null)
            {
                Items = queryData.Items;
                TotalCount = queryData.TotalCount;
                IsFiltered = queryData.IsFiltered;
                IsSorted = queryData.IsSorted;
                IsSearch = queryData.IsSearch;

                // 外部未过滤，内部自行过滤
                if (!IsFiltered)
                {
                    Items = Items.Where(Filters.GetFilterFunc<TItem>());
                }

                // 外部未处理排序，内部自行排序
                if (!IsSorted && SortOrder != SortOrder.Unset && !string.IsNullOrEmpty(SortName))
                {
                    var invoker = SortLambdaCache.GetOrAdd(typeof(TItem), key => Items.GetSortLambda().Compile());
                    Items = invoker(Items, SortName, SortOrder);
                }
            }
        }

        private static readonly ConcurrentDictionary<Type, Func<IEnumerable<TItem>, string, SortOrder, IEnumerable<TItem>>> SortLambdaCache = new ConcurrentDictionary<Type, Func<IEnumerable<TItem>, string, SortOrder, IEnumerable<TItem>>>();

        /// <summary>
        /// 行尾列编辑按钮点击回调此方法
        /// </summary>
        /// <param name="item"></param>
        protected void ClickEditButton(TItem item)
        {
            SelectedItems.Clear();
            SelectedItems.Add(item);

            // 更新行选中状态
            Edit();
            StateHasChanged();
        }

        /// <summary>
        /// 行尾列按钮点击回调此方法
        /// </summary>
        /// <param name="item"></param>
        protected void ClickDeleteButton(TItem item)
        {
            SelectedItems.Clear();
            SelectedItems.Add(item);
            StateHasChanged();
        }
    }
}
