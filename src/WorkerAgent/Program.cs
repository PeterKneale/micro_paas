using Docker.DotNet;
using Microsoft.Extensions.Hosting;
using WorkerAgent.Handlers;
using WorkerAgent.Services;

const string url = "http://localhost:5001";
var token = args[0];

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Create Docker client
        services.AddSingleton<DockerClient>(_ => new DockerClientConfiguration().CreateClient());

        // Register all ICommandHandler implementations
        services.AddSingleton<ICommandHandler, DeployCommandHandler>();

        // Register AgentClient and config
        services.AddSingleton<AgentClient>();
        services.AddSingleton<AgentOptions>(_ => new AgentOptions(url, token));
        services.AddSingleton<AgentIdProvider>();

        services.AddHostedService<AgentClientService>();
        services.AddHostedService<AgentHeartbeatService>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddSimpleConsole(x => { x.SingleLine = true; });
    })
    .Build();
await host.RunAsync();