﻿using RPedretti.Blazor.Shared;
using System;

namespace RPedretti.Blazor.BingMaps.Entities
{
    public abstract class BaseBingMapEntity : BindableBase, IDisposable
    {
        public string Type => GetType().Name.ToLower();
        public string Id { get; set; }

        public abstract void Dispose();
    }
}
