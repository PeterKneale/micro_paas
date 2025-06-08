using System.Collections.Concurrent;

namespace ControlPlane.Services;

public class AgentRegistry(ILogger<AgentRegistry> log)
{
    private readonly ConcurrentDictionary<string, Agent> _agents = new();

    public void AddOrUpdate(Agent agent)
    {
        log.LogInformation("Adding agent to registry: {agent}", agent);
        _agents[agent.Id] = agent;
    }

    public IEnumerable<Agent> All()
    {
        return _agents.Values;
    }

    public void RemoveIfExists(Agent agent)
    {
        log.LogDebug("Removing agent from registry: {agent}", agent);
        _agents.TryRemove(agent.Id, out _);
    }
}