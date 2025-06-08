using ControlPlane.Services;
using ControlPlane.Services.Handlers;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<AgentRegistry>();
builder.Services.AddSingleton<IAgentMessageHandler, HandshakeMessageHandler>();
builder.Services.AddSingleton<IAgentMessageHandler, ShutdownMessageHandler>();
builder.Services.AddSingleton<IAgentMessageHandler, HeartbeatMessageHandler>();
builder.Services.AddScoped<AgentQueue>();
builder.Services.AddHostedService<AgentRegistryMonitor>();
builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel(options =>
{
    // REST API
    options.ListenAnyIP(5000, listenOptions => { listenOptions.Protocols = HttpProtocols.Http1; });
    // gRPC
    options.ListenAnyIP(5001, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
});
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddSimpleConsole(x => { x.SingleLine = true; });
});
var app = builder.Build();
app.MapGrpcService<ControlPlane.Services.ControlPlane>();

app.MapPost("/api/agents/ping", async (AgentQueue queue) =>
{
    await queue.PingAllAsync();
    return Results.Ok("Agents pinged.");
});
app.Run();