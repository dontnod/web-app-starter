import { QueryClientProvider } from '@tanstack/react-query'
import { queryClient } from '@/api/query-client'
import { routeTree } from './routeTree.gen'
import { RouterProvider, createRouter } from '@tanstack/react-router'
import { EventMessageUtils, IPublicClientApplication, InteractionStatus } from '@azure/msal-browser'
import { MsalProvider } from '@azure/msal-react'
import { useEffect } from 'react'
import { BehaviorSubject } from 'rxjs'

// Create a new router instance
const router = createRouter({
  routeTree,
  context: {
    queryClient,
    // Will be initialized inside the `App` function
    msalInstance: null!,
    isMsalReady: new BehaviorSubject(false),
  },
})

export type RouterType = typeof router

// Register the router instance for type safety
declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

export interface AppProps {
  msalInstance: IPublicClientApplication
}

export function App({ msalInstance }: AppProps) {
  // Attach the msalInstance to the router context
  router.options.context.msalInstance = msalInstance

  useEffect(() => {
    // Emit a value when the MSAL is ready, this is particularly useful for the authentication guard to work
    const callbackId = msalInstance.addEventCallback((message) => {
      const status = EventMessageUtils.getInteractionStatusFromEvent(message)
      if (status === InteractionStatus.None) {
        // Cast the observable in order to emit the value (but hide the implementation for the consumers)
        const isMsalReady = router.options.context.isMsalReady as BehaviorSubject<boolean>
        isMsalReady.next(true)
      }
    })

    return () => {
      callbackId && msalInstance.removeEventCallback(callbackId)
    }
  }, [msalInstance])

  return (
    <MsalProvider instance={msalInstance}>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </MsalProvider>
  )
}
