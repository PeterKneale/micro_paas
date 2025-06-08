namespace ControlPlane.Services.Handlers;

public class HeartbeatMessageHandler(ILogger<ShutdownMessageHandler> log):IAgentMessageHandler
{
    public bool CanHandle(AgentMessage message) => message.Heartbeat is not null;

    public Task HandleAsync(AgentMessage message, AgentMessageContext context)
    {
        if (context.CurrentAgent is null)
        {
            log.LogWarning("Heartbeat received but no agent connected");
            return Task.CompletedTask;
        }
        context.CurrentAgent.MarkHeartbeatReceived();
        return Task.CompletedTask;
    }
}