using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using RithmicSoulDatabaseLibrary.Configuration;
using RithmicSoulDatabaseLibrary.Enums;
using RithmicSoulSharedLibrary.Middleware;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<FunctionExceptionHandlingMiddleware>();
    })
    .ConfigureServices((context, service) =>
    {
        var connectionDict = new Dictionary<DatabaseConnectionNames, string?>
        {
            { DatabaseConnectionNames.Admin, Environment.GetEnvironmentVariable("AdminDatabase") },
            { DatabaseConnectionNames.Survey, Environment.GetEnvironmentVariable("SurveyDatabase") }
        };
        service.AddInfrastructure(connectionDict);
    })
    .ConfigureOpenApi()
    .Build();

await host.RunAsync();
