using Grpc.Core;

namespace ControlPlane.Services;

public class ControlPlane(AgentRegistry registry, ILogger<ControlPlane> log)
    : ControlPlaneProtocol.ControlPlaneProtocolBase
{
    public override async Task Connect(IAsyncStreamReader<AgentMessage> request,
        IServerStreamWriter<ControlCommand> response, ServerCallContext context)
    {
        await foreach (var message in request.ReadAllAsync(context.CancellationToken))
        {
            if (message.Handshake is not null)
            {
                log.LogInformation("Handshake");
                var agent = ConnectedAgent.CreateInstance(message.Handshake, response);
                registry.AddOrUpdate(agent);
            }

            if (message.Heartbeat is not null) log.LogInformation("Heartbeat");

            if (message.Pong is not null) log.LogInformation("Pong");
        }
    }
}