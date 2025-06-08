namespace ControlPlane.Services.Handlers;

public interface IAgentMessageHandler
{
    bool CanHandle(AgentMessage message);
    Task HandleAsync(AgentMessage message, AgentMessageContext context);
}