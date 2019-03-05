﻿
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Newtonsoft.Json;

using RPedretti.Blazor.BingMap.Entities;
using RPedretti.Blazor.BingMap.Entities.Pushpin;

namespace RPedretti.Blazor.BingMap
{
    public sealed class DevToolService
    {
        public static async Task<List<BingMapPushpin>> GetPushpins(int ammount, LocationRectangle bounds = null, PushpinOptions options = null)
        {
            var p = await JSRuntime.Current.InvokeAsync<List<BingMapPushpin>>("rpedrettiBlazorComponents.bingMaps.devTools.getPushpins", ammount, bounds, options);
            return p;
         }
    }
}
