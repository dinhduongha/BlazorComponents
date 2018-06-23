﻿using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using RPedretti.Blazor.Components.Pager;
using System;

namespace RPedretti.Blazor.Components.PagedGrid
{
    public class PagedGridBase : BaseAccessibleComponent
    {
        [Parameter] protected Action<int> OnRequestPage { get; set; }
        [Parameter] protected int MaxIndicators { get; set; }
        [Parameter] protected bool SmallPager { get; set; }
        [Parameter] protected int CurrentPage { get; set; }
        [Parameter] protected int PageCount { get; set; }
        [Parameter] protected bool Loading { get; set; }
        [Parameter] protected bool HasContent { get; set; }
        [Parameter] protected string NoContentMessage { get; set; } = "No content";
        [Parameter] protected RenderFragment ChildContent { get; set; }
        [Parameter] protected PagerPosition PagerPosition { get; set; } = PagerPosition.CENTER;
    }
}