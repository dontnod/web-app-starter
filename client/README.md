# DNE Web App Starter - Client

This is a React Single Page Application (SPA) that retrieves a JWT token from Azure AD using MSAL and then consumes a .NET API.

### Install dependencies
To install project dependencies, run the following command:
```sh
yarn install
```

### Generate API Schema
Run the following command after the first build of the API to generate the API schema:
```sh
yarn generate-api
```

# Environment Setup
Duplicate the `.env.example` file and rename it to` .env`. Then, modify the values as follows:
```sh
# Client ID of the SPA Application in Azure
VITE_AZURE_CLIENT_ID=aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa

# Azure authority for the login (generally https://login.microsoftonline.com/{your_tenant_id})
VITE_AZURE_AUTHORITY=https://login.microsoftonline.com/bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb
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
Type-safe fetch client that generates TypeScript types from static [OpenAPI](https://spec.openapis.org/oas/latest.html) schemas using Node.js. It ensures type safety when consuming the .NET API. The  **openapi.json** file is created during the build phase of `DNE.Todo.API` and is located at `/service/DNE.Todo.API/bin/Debug/net8.0/openapi.json`. The `yarn generate-api` command then generates `src/api/todo-schema.d.ts` from this file.

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

## `src/api`

This folder contains all the stuff related to fetching / mutating the data from / to the api.

- [src/api/http-client.ts](src/api/http-client.ts): Expose a `getHttpClient` that create a client to consume the API with type safety from the generated `src/api/todo-schema.d.ts`. This also add **middlewares** to setup the user's jwt token on every request.

- `src/api/todo-schema.d.ts`: Generated type definition of the `DNE.Todo.API` build by the `yarn generated-api` command.

- [src/api/query-client.ts](src/api/query-client.ts): Client instance for the `@tanstack/react-query` used by every queries / mutations.

- [src/api/todo.ts](src/api//todo.ts): Expose `DELETE`/`GET`/`POST`/`PUT` functions on the todo's api with the http client.

## `src/components`

This folder contains generic components used by [route components](src/routes) or other components.

## `src/config`

This folder contains file related to the configuration of the application.

- [src/config/config.ts](src/config/config.ts): Contains the global config of the application, the `CONFIG` object is build from the environment variables (`.env`).

- [src/config/auth-config.ts](src/config/auth-config.ts): Authentication configuration for MSAL.

## `src/features`

Contains all the queries / mutations made with `@tanstack/query` used then by the react components in order to fetch or edit data from the api.

## `src/guards`

Contains **route guards** used by [routes](src/routes) in order to restrict the access of a route.

- [src/guards/authenticate-guard.ts](src/guards/authenticate-guard.ts): Ensure that the user is authenticated, otherwise redirect the user to the login page.

## `src/routes`

Contains all the routes of the application generated with `@tanskactk/router`, you can get more information on naming conventions for routes [there](https://tanstack.com/router/latest/docs/framework/react/guide/file-based-routing)/