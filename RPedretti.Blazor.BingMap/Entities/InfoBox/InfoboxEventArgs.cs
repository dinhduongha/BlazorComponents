﻿using Microsoft.AspNetCore.Components;

namespace RPedretti.Blazor.BingMap.Entities.InfoBox
{
    public class InfoboxEventArgs
    {
        public string EventName { get; set; }
        public UIMouseEventArgs OriginalEvent { get; set; }
        public double PageX { get; set; }
        public double PageY { get; set; }
        public object Target { get; set; }
        public string TargetType { get; set; }
    }
}