﻿using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using RPedretti.Blazor.Sensors.AmbientLight;
using RPedretti.Blazor.Sensors.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPedretti.Blazor.Sensors.Extensions
{
    public static class SensorsExtensions
    {
        #region Ambient Light

        public static IServiceCollection AddAmbientLightSensor(this IServiceCollection services)
        {
            services.AddSingleton<AmbientLightSensor>();
            return services;
        }

        public static void InitAmbientLightSensor(this IBlazorApplicationBuilder app)
        {
            var sensor = app.Services.GetService<AmbientLightSensor>();
            sensor.Init();
        }

        #endregion Ambient Light

        #region Geolocation

        public static IServiceCollection AddGeolocationSensor(this IServiceCollection services)
        {
            services.AddSingleton<GeolocationSensor>();
            return services;
        }

        #endregion Geolocation
    }
}
