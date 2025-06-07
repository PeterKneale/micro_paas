## Building the agent binary
- native
- aot
- self-contained

```sh
dotnet publish src/Agent/Agent.csproj -c Release -o publish
ls -lah publish
./publish/Agent
```
