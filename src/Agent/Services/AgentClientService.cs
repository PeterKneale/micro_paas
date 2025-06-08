using Microsoft.Extensions.Hosting;

namespace Agent.Services;

public class AgentClientService(AgentClient client, ILogger<AgentClientService> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        log.LogInformation("Starting");
        await client.StartAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        log.LogInformation("Stopping");
        await client.SendDisconnectAsync("Agent shutting down", cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}