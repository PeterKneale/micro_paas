using Grpc.Core;

namespace ControlPlane.Services;

public class Agent
{
    private Agent(string id, DateTime at, IServerStreamWriter<ControlCommand> commandStream)
    {
        Id = id;
        ConnectedAt = at;
        CommandStream = commandStream;
    }

    public string Id { get; }

    public DateTime ConnectedAt { get; }
    public DateTime? LastHeartbeat { get; private set; }
    
    public void MarkHeartbeatReceived(){LastHeartbeat = DateTime.UtcNow;}

    public IServerStreamWriter<ControlCommand> CommandStream { get; }

    public static Agent CreateInstance(string id, IServerStreamWriter<ControlCommand> commandStream)
    {
        var at = DateTime.UtcNow;
        return new Agent(id, at, commandStream);
    }

    public override string ToString()
    {
        return $"{Id} Connected: {ConnectedAt.Humanize()} Heartbeat: {LastHeartbeat.Humanize()}";
    }
}