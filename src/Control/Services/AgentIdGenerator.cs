namespace Control.Services;

public static class AgentIdGenerator
{
    public static string Generate() => Guid.NewGuid().ToString()[..8];
}