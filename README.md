## Build a native binary for apple silicon (while on apple silicon)
```sh
dotnet publish src/Agent/Agent.csproj -c Release --self-contained true /p:PublishAot=true -o publish
./publish/Agent
```
