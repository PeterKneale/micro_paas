using System.Collections.Concurrent;

namespace Control.Services;

public class AgentRegistry(ILogger<AgentRegistry> log)
{
    private readonly ConcurrentDictionary<string, ConnectedAgent> _agents = new();

    public void AddOrUpdate(ConnectedAgent agent)
    {
        log.LogInformation("Adding agent to registry");
        _agents[agent.Id] = agent;
    }

    public IEnumerable<ConnectedAgent> All() => _agents.Values;

    public void RemoveIfExists(string agentId)
    {
        if (_agents.TryRemove(agentId, out _))
        {
            log.LogInformation("Removing agent {AgentId} from registry, removed.", agentId);
        }
        else
        {
            log.LogInformation("Removing agent {AgentId} from registry, not found.", agentId);
        }
    }
}