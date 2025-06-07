using Grpc.Core;

namespace Control.Services;

public class ConnectedAgent
{
    private ConnectedAgent(string id, string hostname, IServerStreamWriter<ControlCommand> commandStream)
    {
        Id = id;
        Hostname = hostname;
        CommandStream = commandStream;
    }

    public static ConnectedAgent CreateInstance(AgentHandshake handshake, IServerStreamWriter<ControlCommand> commandStream)
    {
        var id = AgentIdGenerator.Generate();
        var hostname = handshake.Hostname;
        return new ConnectedAgent(id, hostname, commandStream);
    }

    public string Id { get; init; }
    public string Hostname { get; init; }
    
    public DateTime ConnectedAt { get; } = DateTime.UtcNow;
    
    public IServerStreamWriter<ControlCommand> CommandStream { get; init; }
}