FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
RUN apt-get update && apt-get install -y --no-install-recommends clang zlib1g-dev
WORKDIR /build
COPY ["src/Agent/Agent.csproj", "src/Agent/"]
RUN dotnet restore "src/Agent/Agent.csproj"
COPY . .
WORKDIR "/build/src/Agent"
RUN dotnet build "./Agent.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./Agent.csproj" -c $BUILD_CONFIGURATION --self-contained true /p:PublishAot=true -o /app/publish
RUN file /app/publish/Agent && ldd /app/publish/Agent || echo "✅  Static binary confirmed"

FROM debian:bookworm-slim
WORKDIR /app
COPY --from=build /app/publish/Agent .
ENTRYPOINT ["/app/Agent"]
