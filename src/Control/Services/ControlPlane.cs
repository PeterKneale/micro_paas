using Grpc.Core;

namespace Control.Services;

public class ControlPlane(AgentRegistry registry, ILogger<ControlPlane> log) : ControlPlaneProtocol.ControlPlaneProtocolBase
{
    public override async Task Connect(IAsyncStreamReader<AgentMessage> request,IServerStreamWriter<ControlCommand> response, ServerCallContext context)
    {
        var agentId = AgentIdGenerator.Generate();
        
        try
        {
            await foreach (var message in request.ReadAllAsync(context.CancellationToken))
            {
                if (message.Handshake is not null)
                {
                    log.LogInformation("Handshake from: Agent {agentId} {Hostname}",agentId, message.Handshake.Hostname);
                    var agent = ConnectedAgent.CreateInstance(message.Handshake, response);
                    registry.AddOrUpdate(agent);
                }
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            log.LogInformation("Agent {AgentId} disconnected.", agentId);
        }
        finally
        {
            log.LogInformation("Removing agent {AgentId} from registry.", agentId);
            registry.RemoveIfExists(agentId);
        }
    }
}