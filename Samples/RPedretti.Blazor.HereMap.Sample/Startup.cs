using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using RPedretti.Blazor.HereMap.Extensions;

namespace RPedretti.Blazor.HereMap.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.UseHereMap();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
