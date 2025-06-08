namespace WorkerAgent.Services;

public class AgentIdProvider
{
    private const string FileName = "agent-id.txt";
    private readonly string _path = Path.Combine(AppContext.BaseDirectory, FileName);

    public string GetAgentId()
    {
        if (File.Exists(_path)) return File.ReadAllText(_path).Trim();

        var id = Guid.NewGuid().ToString("N");
        File.WriteAllText(_path, id);
        return id;
    }
}