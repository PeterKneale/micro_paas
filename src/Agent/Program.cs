using Agent.Handlers;
using Agent.Services;
using Docker.DotNet;
using Microsoft.Extensions.Hosting;

const string url = "http://localhost:5001";
var token = args[0];

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Create Docker client
        services.AddSingleton<DockerClient>(_ => new DockerClientConfiguration().CreateClient());

        // Register all ICommandHandler implementations
        services.AddSingleton<ICommandHandler, DeployCommandHandler>();

        // Register AgentClient and config
        services.AddSingleton<AgentClient>();
        services.AddSingleton<AgentOptions>(_ => new AgentOptions(url, token));
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddSimpleConsole(x => { x.SingleLine = true; });
    })
    .Build();

var log = host.Services.GetRequiredService<ILogger<Program>>();
var client = host.Services.GetRequiredService<AgentClient>();
try
{
    await client.StartAsync(CancellationToken.None);
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
{
    log.LogError("Failed to connect to control plane on {url}. Is the server running?", url);
    Environment.Exit(2);
}