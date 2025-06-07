

using Agent;
using Agent.Handlers;
using Docker.DotNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Create Docker client
        services.AddSingleton<DockerClient>(_ => new DockerClientConfiguration().CreateClient());

        // Register all ICommandHandler implementations
        services.AddSingleton<ICommandHandler, DeployCommandHandler>();
        // services.AddSingleton<ICommandHandler, StopCommandHandler>(); // (when implemented)

        // Register AgentClient with constructor dependencies
        services.AddSingleton<AgentClient>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<AgentClient>>();
            return new AgentClient("http://localhost:5000", "agent-token", logger);
        });
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

var client = host.Services.GetRequiredService<AgentClient>();
await client.StartAsync(CancellationToken.None);
