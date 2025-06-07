## Experiment with Native AOT
- native
- aot
- self-contained
- trimmed

## Control Plane
- needs a service to monitor for shutdown then disconnect all the clients
- needs to remove agents from the registry as they disconnect
```sh
dotnet publish src/Control/Control.csproj -c Release -o publish/control/
./publish/control/Control
```
## Agent
- Needs to accept server disconnects that aren't graceful.
- It should use polly to attempt reconnection.

```sh
dotnet publish src/Agent/Agent.csproj -c Release -o publish/agent/
./publish/agent/Agent abc123
```

