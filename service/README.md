# DNE Web App Starter - Service

## Setup

### Restore dependencies
```sh
# Restore the nuget packages
dotnet restore

# Restore the tools
cd service/DNE.Todo.API 
dotnet tool restore
```

### App secrets
The Client ID and the Tenant ID need to be set as user secrets in order to use the Azure ad for the authentication / authorization
```sh
# Client ID
dotnet user-secrets set "AzureAd:ClientId" "aa1d2548-9192-4081-9ca8-ce681c39f254" --project "service/DNE.Todo.API"

# Tenant ID
dotnet user-secrets set "AzureAd:TenantId" "6fe0426d-9ec0-4ebc-9716-c7a87fa52a2d" --project "service/DNE.Todo.API"
```