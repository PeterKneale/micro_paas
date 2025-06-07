using Grpc.Core;

public class ControlPlaneService : ControlPlane.ControlPlaneBase
{
    public override async Task Connect(IAsyncStreamReader<AgentMessage> request,IServerStreamWriter<ControlCommand> response, ServerCallContext context)
    {
        // Example: log handshake and echo a dummy command
        await foreach (var message in request.ReadAllAsync(context.CancellationToken))
        {
            if (message.Handshake is not null)
            {
                Console.WriteLine($"Handshake from: {message.Handshake.Hostname}");
                await response.WriteAsync(new ControlCommand
                {
                    Type = "ping"
                });
            }
        }
    }
}