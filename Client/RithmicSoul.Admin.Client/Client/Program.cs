using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using NetCore.AutoRegisterDi;
using RithmicSoul.Admin.Client.Logger;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Admin.Infrastructure.Logging;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace RithmicSoul.Admin.Client.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddScoped<IJsInteropLogger, JsInteropLogger>();

            builder.Services.AddScoped(typeof(ConsoleRedirectLogger<>));
            //builder.Services.AddSingleton(_ => new PeriodicTimerService());
            builder.Services.AddHttpClientInterceptor();
            if (builder.HostEnvironment.IsDevelopment())
            {
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7129/api/") }
                    .EnableIntercept(sp));
            }            else
            {
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{ builder.HostEnvironment.BaseAddress }api/") }
                    .EnableIntercept(sp));
            }

            builder.Services.AddLoadingBar(config =>
            {
                config.LoadingBarColor = "#fc8236";
            });

            var registered = builder.Services.RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Program)), Assembly.GetAssembly(typeof(Infrastructure.Services.QuestionTypeService)))
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
                config.SnackbarConfiguration.VisibleStateDuration = 4000;
                config.SnackbarConfiguration.HideTransitionDuration = 200;
                config.SnackbarConfiguration.ShowTransitionDuration = 200;
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
            });
            builder.UseLoadingBar();
            await builder.Build().RunAsync();
        }
    }
}
