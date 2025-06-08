namespace ControlPlane.Services;

public class AgentQueue(AgentRegistry registry, ILogger<AgentQueue> log)
{
    public async Task PingAllAsync()
    {
        var agents = registry.All().ToList();
        if (agents.Count == 0)
        {
            log.LogInformation("No agents to ping");
            return;
        }

        foreach (var agent in agents)
        {
            log.LogInformation("Pinging {AgentId}", agent.Id);
            var command = new ControlCommand
            {
                Ping = new ControlPing()
            };
            await agent.CommandStream.WriteAsync(command);
        }
    }
}