using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace Agent;

public class AgentClient(string controlPlaneUrl, string agentToken, ILogger<AgentClient> logger)
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var channel = GrpcChannel.ForAddress(controlPlaneUrl);
        var client = new ControlPlane.ControlPlaneClient(channel);

        var metadata = new Metadata { { "authorization", $"Bearer {agentToken}" } };

        using var call = client.Connect(metadata, cancellationToken: cancellationToken);

        // Send one-time handshake
        await call.RequestStream.WriteAsync(new AgentMessage
        {
            Handshake = new AgentHandshake
            {
                Hostname = Environment.MachineName,
                Os = Environment.OSVersion.ToString(),
                AgentVersion = "0.1.0"
            }
        }, cancellationToken);

        // Listen for incoming commands
        await foreach (var command in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            logger.LogInformation("Received command: {Type}", command.Type);
        }
    }
}