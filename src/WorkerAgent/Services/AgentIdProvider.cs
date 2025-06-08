namespace WorkerAgent.Services;

public class AgentIdProvider
{
    public string GetAgentId(string? instance="default")
    {
        var filename = $"Agent-{instance}.txt";
        var path = Path.Combine(AppContext.BaseDirectory, filename);
        if (File.Exists(path)) return File.ReadAllText(path).Trim();
        var id = Guid.NewGuid().ToString("N");
        File.WriteAllText(path, id);
        return id;
    }
}