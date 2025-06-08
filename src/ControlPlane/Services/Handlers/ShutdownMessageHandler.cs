namespace ControlPlane.Services.Handlers;

public class ShutdownMessageHandler(AgentRegistry registry, ILogger<ShutdownMessageHandler> log):IAgentMessageHandler
{
    public bool CanHandle(AgentMessage message) => message.Shutdown is not null;

    public Task HandleAsync(AgentMessage message, AgentMessageContext context)
    {
        if (context.CurrentAgent is null)
        {
            log.LogWarning("Shutdown received but Agent is not set");
            return Task.CompletedTask;
        }
        
        registry.RemoveIfExists(context.CurrentAgent);
        return Task.CompletedTask;
    }
}