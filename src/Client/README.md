# DNE Web App Starter - Client

This is a React Single Page Application (SPA) that retrieves a JWT token from Azure AD using MSAL and then consumes a .NET API.

## EntraID/AzureAD setup
### Create an App registration
It must be single-tenant. This identity is used to manage user access.
#### Manually
1. Navigate to the [Azure portal](https://portal.azure.com/) and select the `Microsoft Entra ID`.
1. Select `Manage > App Registrations` blade on the left, then select `New registration`.
1. In the Register an application page that appears, under `Supported account types`, select `Accounts in this organizational directory only` and click on `Register`.
1. In the `Overview` blade, find and note the `Application (client) ID`. You use this value in your app's configuration file(s) later in your code.

#### CLI
First install az cli and sign in `az login --allow-no-subscriptions`, then run:
```
echo "Name of Client Identity? (Type and press enter to continue)" && read -r CLIENT_NAME
CLIENT_ID=$(az ad app create --display-name $CLIENT_NAME --sign-in-audience AzureADMyOrg --query appId --output tsv)
```

### Setup redirect URIs
It must be single-tenant. This identity is used to manage user access.
#### Manually
1. In the Overview blade, find and note the `Application (client) ID`. You use this value in your app's configuration file(s) later in your code.
1. In the app's registration screen, select the Authentication blade to the left.
1. If you don't have a platform added, select Add a platform and select the Single-page application option.
1. In the Redirect URI section enter the following redirect URIs:
   1.`http://localhost:3002`
   1.`http://localhost:3002/redirect.html`
1. Click Save to save your changes.
![redirect_uri.png](doc_assets/redirect_uri.png)
#### CLI
```
cat <<EOF > redirectUris.json 
{
    "redirectUris": [
      "http://localhost:3002",
      "http://localhost:3002/redirect.html"
    ]
}
EOF
az ad app update --id $CLIENT_ID --set spa=@redirectUris.json
```

### Setup delegated permissions

#### Manually
Since this app signs-in users, we will now proceed to select delegated permissions, which is is required by apps signing-in users.
1. Select `Manage > API permissions` blade in the left to open the page where we add access to the APIs that your application needs:
1. Select the `Add a permission` button and then:
   1. Ensure that the `Microsoft APIs` tab is selected.
   1. In the `Commonly used Microsoft APIs` section, select `Microsoft Graph`
      ![create_api_permission.png](doc_assets/create_api_permission.png)
   1. In the `Delegated permissions` section, select `User.read` in the list. Use the search box if necessary.
      ![user_read_permissions.png](doc_assets/user_read_permissions.png)
   1. Select the `Add permissions` button at the bottom.
1. Select the Add a permission button and then:
   1. Ensure that the `My APIs` tab is selected.
   1. In the list of APIs, select the API `DNE Tools Sample API`.
   1. In the Delegated permissions section, select `ToDoList.Read`, `ToDoList.ReadWrite` in the list. Use the search box if necessary.
   1. Select the `Add permissions` button at the bottom.
   2. ![my_api_permissions.png](doc_assets/my_api_permissions.png)
1. At this stage, the permissions are assigned correctly, the users themselves cannot consent to these permissions. To get around this problem, we'd let the [tenant administrator consent on behalf of all users in the tenant](https://learn.microsoft.com/en-us/entra/identity-platform/v2-admin-consent). Select the `Grant admin consent for {tenant} button`, and then select Yes when you are asked if you want to grant consent for the requested permissions for all accounts in the tenant. You need to be a tenant admin to be able to carry out this operation.

#### CLI
NB: `e1fe6dd8-ba31-4d61-89e7-88639da4683d` resource ID is `Microsoft Graph > User.read` permission.
```
APP_ID=$(az ad app list --filter "displayname eq 'DNE Tools Sample API'" --query [0].appId  | sed 's,",,g')
ressource_readwrite_id=$(az ad app show --id $APP_ID --query api.oauth2PermissionScopes[0].id | sed 's,",,g')
ressource_read_id=$(az ad app show --id $APP_ID --query api.oauth2PermissionScopes[1].id | sed 's,",,g')
cat <<EOF > requiredResourceAccess.json 
[{
   "resourceAppId": "$APP_ID",
   "resourceAccess": [
       {
           "id": "$ressource_readwrite_id",
           "type": "Scope"
       },
       {
           "id": "$ressource_read_id",
           "type": "Scope"
       }
   ]
},
{
   "resourceAppId": "00000003-0000-0000-c000-000000000000",
   "resourceAccess": [
       {
           "id": "e1fe6dd8-ba31-4d61-89e7-88639da4683d",
           "type": "Scope"
       }
   ]
}]
EOF

az ad app update --id $CLIENT_ID --required-resource-accesses @requiredResourceAccess.json
```

## Setup client application 
### Install dependencies
To install project dependencies, run the following command:
```sh
npm install
```

### Generate API Schema
Run the following command after the first build of the API to generate the API schema:
```sh
npm run api:generate:dev
```

# Environment Setup
Duplicate the `.env.example` file and rename it to` .env`. Then, modify the values as follows:
```sh
# Client ID of the SPA Application in Azure
VITE_AZURE_CLIENT_ID=aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa

# Azure authority for the login (generally https://login.microsoftonline.com/{your_tenant_id})
VITE_AZURE_AUTHORITY=https://login.microsoftonline.com/bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb
```

# Run the project
```sh
npm run dev
```

# Libraries and dependencies

## vite ([documentation](https://vitejs.dev/))
Vite is a modern frontend tooling solution used for serving the client in development and building/packaging the app for production. It is considered a modern alternative to **webpack**. The configuration file is located at [vite.config.ts](vite.config.ts).

## @azure/msal-browser & @azure/msal-react ([documentation](https://github.com/AzureAD/microsoft-authentication-library-for-js))
These libraries facilitate user redirection to the authentication page and retrieval of the JWT token. The MSAL instance is set up in [src/main.tsx](src/main.tsx) as follows:

```typescript
export const msalInstance = new PublicClientApplication(MSAL_CONFIG)
```

The configuration for MSAL is found in [src/config/auth-config.ts](src/config/auth-config.ts)

[Documentation for MSAL specific to React](https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/getting-started.md)

## @tanstack/react-query ([documentation](https://tanstack.com/query/latest/docs/framework/react/overview))
A simple state management library used to manage API services. It supports auto-caching, optimistic results, and request cancellation. Queries and mutations are managed within the`src/features` folder.

## @tanstack/react-router ([documentation](https://tanstack.com/router/latest/docs/framework/react/overview))
A fully type-safe React router with built-in data fetching, stale-while-revalidate caching, and first-class search-param APIs. Routes are defined by the folder structure of the components inside `src/routes`, with route path types generated automatically by the `@tanstack/router-vite-plugin` into  `src/routeTree.gen.ts`. 

For example, [src/routes/todo/$id.tsx](src/routes/todo/$id.tsx) maps to the route  `/todo/{id_of_the_todo}` and loads the corresponding view component.

## eslint ([documentation](https://eslint.org/))
A static code analysis tool that helps enforce coding standards for the frontend. The configuration file is located in  [.eslintrc.cjs](.eslintrc.cjs).

## antd  ([documentation](https://ant.design/components/overview/))
A comprehensive suite of React UI components that enhance UI development efficiency, similar to Bootstrap.

## openapi-fetch / openapi-typescript ([documentation](https://github.com/drwpow/openapi-typescript))
Type-safe fetch client that generates TypeScript types from static [OpenAPI](https://spec.openapis.org/oas/latest.html) schemas using Node.js. It ensures type safety when consuming the .NET API. The  **openapi.json** file is created during the build phase of `WebAppStarter.Api` and is located at `/src/Api/bin/Debug/net8.0/openapi.json`. The `npm run api:generate:dev` command then generates `src/api/todo-schema.d.ts` from this file.

## react-hook-form ([documentation](https://react-hook-form.com/))
Provides efficient and flexible form management in React, supporting form validation, nested forms, array forms, and more.

## zod  ([documentation](https://zod.dev/))
Zod is a TypeScript library for schema validation with static type inference, primarily used with`react-hook-form` for validating form schemas. It can also be used for general JSON schema validation.

For example, the configuration file [src/config/config.ts](src/config/config.ts) validates the `CONFIG` variable, which is derived from the environment variables using a Zod schema.

```typescript
// Create the validation schema
const configSchema = z.object({
  azure: z.object({
    authority: z.string(),
    clientId: z.string(),
  }),
  todoApi: z.object({
    endpoint: z.string(),
    scopes: z.object({
      read: z.array(z.string()),
      write: z.array(z.string()),
    }),
  }),
})

// Infer the typescript type of the config from the schema
export type Config = z.infer<typeof configSchema>

// Create and parse the config schema (this will throw an error is the schema is not respected )
export const CONFIG: Config = configSchema.parse({
  azure: {
    authority: import.meta.env.VITE_AZURE_AUTHORITY,
    clientId: import.meta.env.VITE_AZURE_CLIENT_ID,
  },
  todoApi: {
    endpoint: import.meta.env.VITE_TODO_API_ENDPOINT_URL,
    scopes: {
      read: JSON.parse(import.meta.env.VITE_TODO_API_SCOPES_READ),
      write: JSON.parse(import.meta.env.VITE_TODO_API_SCOPES_WRITE),
    },
  },
})

```

# Code Structure
This documentation outlines the organizational structure of the React application's source code, detailing the purpose and functionality of each directory and key files within the project.

## `src/api`

This directory manages all interactions with the API, including data fetching and mutations.

- [src/api/http-client.ts](src/api/http-client.ts): Provides a `getHttpClient` function that creates a client for consuming the API with type safety, leveraging the generated types from `src/api/todo-schema.d.ts`. It includes **middlewares** to append the user's JWT token to every request.

- `src/api/todo-schema.d.ts`: Contains the generated type definitions for the `WebAppStarter.Api`, created by the  `npm run generated-api` command.

- [src/api/query-client.ts](src/api/query-client.ts): An instance of `@tanstack/react-query` used for handling queries and mutations throughout the application.

- [src/api/todo.ts](src/api//todo.ts): Defines CRUD functions (`DELETE`, `GET`, `POST`, `PUT`) for interacting with the todo API, utilizing the http client.

## `src/components`

This folder houses reusable components that are used either within [route components](src/routes) or elsewhere in the application.

## `src/config`

This directory contains configuration files for the application.

- [src/config/config.ts](src/config/config.ts): Stores the global configuration of the application, assembling the `CONFIG` object from environment variables defined in `.env`.

- [src/config/auth-config.ts](src/config/auth-config.ts): Contains the authentication settings for MSAL.

## `src/features`

Includes all the `@tanstack/react-query` queries and mutations, which are utilized by React components to fetch or update data from the API.

## `src/guards`

Contains **route guards** used by [routes](src/routes) in order to restrict the access of a route.

- [src/guards/authenticate-guard.ts](src/guards/authenticate-guard.ts): Verifies user authentication and redirects unauthenticated users to the login page if necessary.

## `src/routes`

Manages all application routes, structured and handled via @tanstack/router. For naming conventions and routing structures, refer to the [File-Based Routing Guide](https://tanstack.com/router/latest/docs/framework/react/guide/file-based-routing).

## `src/utils`

Contains utility functions and classes that support various functionalities across the application.

- [src/utils/claim-utils.ts](src/utils/claim-utils.ts): A helper utility for constructing a claims table from a [ID Token Claims](https://learn.microsoft.com/en-us/entra/identity-platform/id-token-claims-reference),  demonstrating how to extract claim information for a user.

- [src/utils/msal-navigation-client.ts](src/utils/msal-navigation-client.ts): A custom [NavigationClient](https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/performance.md) class to bind `@tanstack/react-router` to the instance of **MSAL** this allows MSAL to redirect to the user coming page when doing a login.

This structure ensures a modular and clear organization of code, simplifying maintenance and scalability of the application.