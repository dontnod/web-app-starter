# DNE Web App Starter - Service

## Setup

### Restore dependencies
To install project dependencies, run the following command:
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

### App settings
[appsettings.json](appsettings.json) can be edited to match your needs. This files contains the `scope names` / `app permissions` (user roles) / `claim settings` for Azure AD.

# Run the project
```sh
dotnet run
```


# Code Structure

## `Controllers`

This directory contains all the mvc controllers of the API

- [Controllers/ToDoListController.cs](Controllers/ToDoListController.cs): Example of a CRUD controller to manage a todo list.

## `Models`

This directory contains all the models used by the application.

## `DbContext`

Db Context for the entity framework.

## `Permissions`

Contains custom decorators to protect an endpoint with Azure AD **Scopes** and **Roles**.