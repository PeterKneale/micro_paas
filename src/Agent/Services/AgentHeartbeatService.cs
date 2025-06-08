using Microsoft.Extensions.Hosting;

namespace Agent.Services;

public class AgentHeartbeatService(AgentClient client, ILogger<AgentHeartbeatService> log) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        log.LogInformation("Starting");
        while (!stoppingToken.IsCancellationRequested)
        {
            if (client.IsConnected)
            {
                log.LogDebug("Sending heartbeat...");
                await client.SendHeartbeatAsync(stoppingToken);
            }
            else
            {
                log.LogDebug("Not connected, not sending heartbeat");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        log.LogInformation("Stopping");
        await base.StopAsync(stoppingToken);
    }
}