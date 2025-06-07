using System.Collections.Concurrent;

namespace Control.Services;

public class AgentRegistry
{
    private readonly ConcurrentDictionary<string, ConnectedAgent> _agents = new();

    public void AddOrUpdate(ConnectedAgent agent)
    {
        _agents[agent.Id] = agent;
    }

    public bool TryGet(string agentId, out ConnectedAgent? agent)
        => _agents.TryGetValue(agentId, out agent);

    public IEnumerable<ConnectedAgent> All() => _agents.Values;

    public void Remove(string agentId)
        => _agents.TryRemove(agentId, out _);
}