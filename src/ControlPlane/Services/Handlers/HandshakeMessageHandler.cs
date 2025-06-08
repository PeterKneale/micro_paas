namespace ControlPlane.Services.Handlers;

public class HandshakeMessageHandler(AgentRegistry registry):IAgentMessageHandler
{
    public bool CanHandle(AgentMessage message) => message.Handshake is not null;

    public Task HandleAsync(AgentMessage message, AgentMessageContext context)
    {
        var id = message.Handshake.Id;
        var agent = Agent.CreateInstance(id, context.Stream);
        registry.AddOrUpdate(agent);
        context.SetCurrentAgent( agent);
        return Task.CompletedTask;
    }
}