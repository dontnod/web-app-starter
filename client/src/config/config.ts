import { z } from 'zod'

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

export type Config = z.infer<typeof configSchema>

export const CONFIG: Config = configSchema.parse({
  azure: {
    authority: import.meta.env.VITE_AZURE_AUTHORITY,
    clientId: import.meta.env.VITE_AZURE_CLIENT_ID,
  },
  todoApi: {
    endpoint: import.meta.env.VITE_TODO_API_ENDPOINT_URL,
    /**
     * Add here the endpoints and scopes when obtaining an access token for protected web APIs. For more information, see:
     * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/resources-and-scopes.md
     */
    scopes: {
      read: JSON.parse(import.meta.env.VITE_TODO_API_SCOPES_READ),
      write: JSON.parse(import.meta.env.VITE_TODO_API_SCOPES_WRITE),
    },
  },
})
