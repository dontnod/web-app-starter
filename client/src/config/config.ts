import { z } from 'zod'

const configSchema = z.object({
  azure: z.object({
    authority: z.string(),
    clientId: z.string(),
  }),
})

export type Config = z.infer<typeof configSchema>

export const CONFIG: Config = configSchema.parse({
  azure: {
    authority: import.meta.env.VITE_AZURE_AUTHORITY,
    clientId: import.meta.env.VITE_AZURE_CLIENT_ID,
  },
})
