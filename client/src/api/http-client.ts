import { paths } from './todo-schema'
import createClient, { ClientOptions, Middleware } from 'openapi-fetch'
import { IPublicClientApplication } from '@azure/msal-browser'
import { msalInstance } from '@/main'

export const getAuthMiddleware = (
  instance: IPublicClientApplication,
  scopes: string[]
): Middleware => {
  return {
    async onRequest(req) {
      const tokenResponse = await instance.acquireTokenSilent({ scopes })
      req.headers.set('Authorization', `Bearer ${tokenResponse.accessToken}`)
      return req
    },
  }
}

export const getHttpClient = (scopes: string[] = [], clientOptions?: ClientOptions) => {
  const client = createClient<paths>({
    baseUrl: import.meta.env.VITE_ENDPOINT_URL,
    ...clientOptions,
  })

  if (msalInstance.getActiveAccount()) {
    client.use(getAuthMiddleware(msalInstance, scopes))
  }

  return client
}
