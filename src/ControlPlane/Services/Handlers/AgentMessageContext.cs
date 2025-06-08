using Grpc.Core;

namespace ControlPlane.Services;

public class AgentMessageContext(IServerStreamWriter<ControlCommand> stream)
{
    public IServerStreamWriter<ControlCommand> Stream { get; } = stream;
    public Agent? CurrentAgent { get; private set; }

    public void SetCurrentAgent(Agent agent)
    {
        if (CurrentAgent is not null)
            throw new InvalidOperationException("Agent is already set");
        CurrentAgent = agent;
    }
}