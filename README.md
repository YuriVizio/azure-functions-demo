
# Azure Functions demo (C# .NET 8 isolated) — Single Container with f1, f2, f3

- Endpoints: `/api/f1`, `/api/f2`, `/api/f3` (GET) return `text/plain` — "Hello from <function>".
- HTTP port configurable via `APP_PORT` (default `8444`).
- **Dev compose includes Azurite (Blob/Queue/Table) and Cosmos DB Emulator**.

## Prereqs
- Docker Desktop
- .NET 8 SDK (to run tests)

## Run Locally — Dev (default)
This uses Azurite and Cosmos emulator automatically.
```bash
docker compose up --build -d
curl http://localhost:8444/api/f1
curl http://localhost:8444/api/f2
curl http://localhost:8444/api/f3
```

### Ports
- Functions: `${APP_PORT}` (defaults to 8444)
- Azurite: 10000 (Blob), 10001 (Queue), 10002 (Table)
- Cosmos Emulator: 8081 (HTTPS)

> If you're on Apple Silicon, build for amd64: `docker buildx build --platform linux/amd64 ...` or add `platform: linux/amd64` to services.

## Run Locally — Stage (app only, no emulators)
Provide a real Storage connection and explicitly set the stage profile:
```bash
AzureWebJobsStorage="DefaultEndpointsProtocol=https;AccountName=<acct>;AccountKey=<key>;EndpointSuffix=core.windows.net" COMPOSE_PROFILES=stage docker compose up --build -d
curl http://localhost:8444/api/f1
curl http://localhost:8444/api/f2
curl http://localhost:8444/api/f3
```

## Tests
### Unit tests (per function)
```bash
# From repo root
dotnet test ./src/tests/unit/F1.UnitTests/F1.UnitTests.csproj
dotnet test ./src/tests/unit/F2.UnitTests/F2.UnitTests.csproj
dotnet test ./src/tests/unit/F3.UnitTests/F3.UnitTests.csproj
```

### Integration tests (verify /api endpoints)
```bash
# Uses APP_PORT from environment (defaults to 8444)
dotnet test ./src/tests/integration/IntegrationTests.csproj
```

## CI (GitHub Actions)
- On `push`/`pull_request`, CI restores, builds, runs **unit tests**, spins up the **dev stack (Functions + Azurite + Cosmos)**, then runs **integration tests**.

## Cloud Deployment (Azure Container Apps)
1. Build the image:
   ```bash
   COMPOSE_PROFILES=stage docker compose build
   ```
2. Tag & push to your registry.
3. Deploy to ACA, set ingress to `APP_PORT`, and map `AzureWebJobsStorage` (or use Connections / managed identity).

## Notes
- Base image: `mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0`.
- `AzureWebJobsScriptRoot` is `/home/site/wwwroot` so Functions host indexes your app correctly.
- Cosmos env vars are present for convenience; the sample functions do not call Cosmos.
