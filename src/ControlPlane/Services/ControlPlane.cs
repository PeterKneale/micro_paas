using ControlPlane.Services.Handlers;
using Grpc.Core;

namespace ControlPlane.Services;

public class ControlPlane(IEnumerable<IAgentMessageHandler> handlers, ILogger<ControlPlane> log)
    : ControlPlaneProtocol.ControlPlaneProtocolBase
{
    public override async Task Connect(IAsyncStreamReader<AgentMessage> request,
        IServerStreamWriter<ControlCommand> response, ServerCallContext context)
    {
        var messageContext = new AgentMessageContext(response);
        await foreach (var message in request.ReadAllAsync(context.CancellationToken))
        {
            var handled = false;
            foreach (var handler in handlers)
            {
                if (!handler.CanHandle(message))
                {
                    continue;
                }

                log.LogDebug($"Handling with {handler.GetType().Name}");
                await handler.HandleAsync(message, messageContext);
                handled = true;
                break;
            }

            if (!handled)
            {
                log.LogWarning("Unknown message received");
            }
        }
    }
}