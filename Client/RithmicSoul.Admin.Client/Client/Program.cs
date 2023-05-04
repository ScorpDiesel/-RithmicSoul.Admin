using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using NetCore.AutoRegisterDi;

namespace RithmicSoul.Admin.Client.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            //builder.Services.AddSingleton<ILoggerProvider, ConsoleLogProvider>();
            //builder.Services.AddLogging(logging =>
            //{
            //    //logging.ClearProviders();
            //    //logging.AddProvider(new ConsoleLogProvider());
            //});
            if (builder.HostEnvironment.IsDevelopment())
            {
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7129/api/") });
            }
            else
            {
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{ builder.HostEnvironment.BaseAddress }api/") });
            }


            var registered = builder.Services.RegisterAssemblyPublicNonGenericClasses()
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();

            //builder.Services.AddSingleton<ConsoleLogProvider>();
            builder.Services.AddMudServices();
            await builder.Build().RunAsync();
        }
    }
}
