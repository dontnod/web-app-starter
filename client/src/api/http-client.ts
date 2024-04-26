import { paths } from './todo-schema'
import createClient, { ClientOptions, Middleware } from 'openapi-fetch'
import { IPublicClientApplication } from '@azure/msal-browser'
import { msalInstance } from '@/main'
import { CONFIG } from '@/config/config'

export class HttpError extends Error {
  readonly status: number
  readonly statusText: string
  constructor(status: number, statusText: string, message?: string) {
    const displayedMessage = statusText
    if (message != null) {
      displayedMessage.concat(`, ${message}`)
    }
    super(displayedMessage)
    this.status = status
    this.statusText = statusText
  }
}

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

const throwErrorMiddleware: Middleware = {
  async onResponse(res) {
    if (!res.ok) {
      // Throw an Error on bad response,
      // This is require by tanstack query in order to catch the exception
      const message = await res.text()
      throw new HttpError(res.status, res.statusText, message)
    }
    return res
  },
}

export const getHttpClient = (scopes: string[] = [], clientOptions?: ClientOptions) => {
  const client = createClient<paths>({
    baseUrl: CONFIG.todoApi.endpoint,
    ...clientOptions,
  })

  client.use(throwErrorMiddleware)

  if (msalInstance.getActiveAccount()) {
    client.use(getAuthMiddleware(msalInstance, scopes))
  }

  return client
}
