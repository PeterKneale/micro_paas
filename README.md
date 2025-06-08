## Experiment with Native AOT

- native
- aot
- self-contained
- trimmed

## Control Plane

- needs a service to monitor for shutdown then disconnect all the clients
- needs to remove agents from the registry as they disconnect

```sh
dotnet watch --project src/ControlPlane/ControlPlane.csproj
```

## Worker Agent

- Needs to accept server disconnects that aren't graceful.
- It should use polly to attempt reconnection.

```sh
dotnet watch --project src/WorkerAgent/WorkerAgent.csproj abc
```

