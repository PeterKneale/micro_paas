## Build a native binary for apple silicon (while on apple silicon)
```sh
dotnet publish src/Agent/Agent.csproj -c Release --self-contained true /p:PublishAot=true -o publish
./publish/Agent
```

## Building a docker image for intel/amx silicon (while on apple silicon)

Note: Because Docker runs Linux containers, you can’t run a macOS binary inside a Docker container.

```sh
docker build . -f src/Agent/Dockerfile --platform=linux/amd64 -t agent
docker run --platform=linux/amd64 agent
```

# Issue
would be better if the statically compiled agent could run on the scratch image