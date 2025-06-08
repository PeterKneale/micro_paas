namespace WorkerAgent.Services;

public class AgentClient(AgentOptions options, AgentIdProvider ids, ILogger<AgentClient> log)
{
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private IClientStreamWriter<AgentMessage>? _requestStream;

    public bool IsConnected => _requestStream != null;

    public async Task StartAsync(CancellationToken cancel)
    {
        log.LogInformation("Connecting to {url} with token {token} ", options.ControlPlaneUrl, options.AgentToken);
        var channel = GrpcChannel.ForAddress(options.ControlPlaneUrl);
        var client = new ControlPlaneProtocol.ControlPlaneProtocolClient(channel);

        var metadata = new Metadata { { "authorization", $"Bearer {options.AgentToken}" } };

        using var call = client.Connect(metadata, cancellationToken: cancel);
        _requestStream = call.RequestStream;

        await WriteAsync(new AgentMessage
        {
            Handshake = new AgentHandshake { Id = ids.GetAgentId() }
        }, cancel);

        try
        {
            await foreach (var command in call.ResponseStream.ReadAllAsync(cancel))
            {
                log.LogInformation("Received command: {Type}", command);
                if (command != null)
                {
                    log.LogInformation("Sending pong");
                    await WriteAsync(new AgentMessage
                    {
                        Pong = new AgentPong()
                    }, cancel);
                }
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            log.LogInformation("gRPC stream cancelled — shutting down agent");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unexpected error while reading command stream");
        }
    }

    public async Task SendHeartbeatAsync(CancellationToken cancellationToken = default)
    {
        await WriteAsync(new AgentMessage { Heartbeat = new AgentHeartbeat() }, cancellationToken);
    }

    public async Task SendDisconnectAsync(string reason, CancellationToken cancellationToken)
    {
        try
        {
            await WriteAsync(new AgentMessage
            {
                Shutdown = new AgentShutdown { Reason = reason }
            }, cancellationToken);
        }
        catch (Exception e)
        {
            log.LogError(e, "error attempting to send shutdown");
        }

        // closes the client stream gracefully
        if (_requestStream != null)
            await _requestStream.CompleteAsync();
    }

    private async Task WriteAsync(AgentMessage message, CancellationToken cancellationToken)
    {
        if (_requestStream is null) throw new InvalidOperationException("The connection is closed.");

        await _writeLock.WaitAsync(cancellationToken);
        try
        {
            await _requestStream.WriteAsync(message, cancellationToken);
        }
        finally
        {
            _writeLock.Release();
        }
    }
}