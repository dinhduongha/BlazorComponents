﻿using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorApp30.Components.Accordeon
{
    public class AccordeonBase : BlazorComponent
    {
        [Parameter]
        protected string Title { get; set; }

        [Parameter]
        protected bool CenterTitle { get; set; }

        [Parameter]
        protected RenderFragment ChildContent { get; set; }

        private bool _expanded;

        [Parameter]
        protected bool Expanded
        {
            get { return _expanded; }
            set
            {
                _expanded = value;
                var delay = !value ? 600 : 0;
                new Timer(_ =>
                {
                    ExpandedChanged?.Invoke(_expanded);
                    StateHasChanged();
                }, null, delay, Timeout.Infinite);
            }
        }

        [Parameter]
        protected Action<bool> ExpandedChanged { get; set; }
    }
}
