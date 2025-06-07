namespace Agent.Services;

public class AgentClient(AgentOptions options, ILogger<AgentClient> log)
{
    public async Task StartAsync(CancellationToken cancel)
    {
        log.LogInformation("Connecting to {url} with token {token} ", options.ControlPlaneUrl, options.AgentToken);
        var channel = GrpcChannel.ForAddress(options.ControlPlaneUrl);
        var client = new ControlPlaneProtocol.ControlPlaneProtocolClient(channel);

        var metadata = new Metadata { { "authorization", $"Bearer {options.AgentToken}" } };

        using var call = client.Connect(metadata, cancellationToken: cancel);

        await SendHandshake(call, cancel);

        await foreach (var command in call.ResponseStream.ReadAllAsync(cancel))
        {
            log.LogInformation("Received command: {Type}", command.Type);
        }
    }

    private async Task SendHandshake(AsyncDuplexStreamingCall<AgentMessage, ControlCommand> call,
        CancellationToken cancel)
    {
        log.LogInformation($"Sending handshake (Token: {options.AgentToken})");
        await call.RequestStream.WriteAsync(new AgentMessage
        {
            Handshake = new AgentHandshake
            {
                Hostname = Environment.MachineName,
                Os = Environment.OSVersion.ToString(),
                AgentVersion = "0.1.0"
            }
        }, cancel);
    }
}