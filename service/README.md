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
dotnet user-secrets set "AzureAd:ClientId" "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa" --project "service/DNE.Todo.API"

# Tenant ID
dotnet user-secrets set "AzureAd:TenantId" "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb" --project "service/DNE.Todo.API"
```