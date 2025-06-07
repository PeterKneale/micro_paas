namespace Control.Services;

public class AgentRegistryMonitor(AgentRegistry registry, ILogger<AgentRegistryMonitor> logger)
    : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(10);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var agents = registry.All().ToList();
            var count = agents.Count;
            
            logger.LogInformation("Connected agents: {Count}", count);
            foreach (var agent in agents)
            {
                logger.LogInformation($"Agent {agent.Id} {agent.Hostname}");
            }
            await Task.Delay(Interval, stoppingToken);
        }
    }
}