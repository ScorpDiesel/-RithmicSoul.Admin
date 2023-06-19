using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using NetCore.AutoRegisterDi;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Admin.Client.Logger;
using RithmicSoul.Admin.Infrastructure.Logging;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using RithmicSoul.Admin.Core.Models;

namespace RithmicSoul.Admin.Client.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddMsalAuthentication(options =>
            {
                options.ProviderOptions.LoginMode = "redirect";
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read");
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
            builder.Services.AddScoped<IJsInteropLogger, JsInteropLogger>();
            builder.Services.AddScoped(typeof(ConsoleRedirectLogger<>));
            //builder.Services.AddSingleton(_ => new PeriodicTimerService());

            var registered = builder.Services.RegisterAssemblyPublicNonGenericClasses(typeof(Program).Assembly, typeof(Infrastructure.Services.QuestionTypeService).Assembly, typeof(Application.Dtos.DraftSurveyDto).Assembly)
                .Where(c => c.Name.EndsWith("Service") || c.Name.EndsWith("Repository"))
                .AsPublicImplementedInterfaces();

            if (builder.HostEnvironment.IsDevelopment())
            {
                builder.Services.AddSingleton(_ => new AppSetting { BaseAddress = "http://localhost:7129/api" });
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7129/api/") }
                    .EnableIntercept(sp));
            }
            else
            {
                builder.Services.AddSingleton(_ => new AppSetting { BaseAddress = $"{builder.HostEnvironment.BaseAddress}api" });
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/") }
                    .EnableIntercept(sp));
            }
            
            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddLoadingBar(config =>
            {
                config.LoadingBarColor = "#fc8236";
            });

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
