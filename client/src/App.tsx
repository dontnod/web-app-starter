import { QueryClientProvider } from '@tanstack/react-query'
import { queryClient } from '@/api/query-client'

// Import the generated route tree
import { routeTree } from './routeTree.gen'
import { RouterProvider, createRouter } from '@tanstack/react-router'
import { IPublicClientApplication } from '@azure/msal-browser'
import { MsalNavigationClient } from '@/utils/msal-navigation-client'
import { MsalProvider } from '@azure/msal-react'

// Create a new router instance
const router = createRouter({
  routeTree,
  context: {
    queryClient,
  },
})

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
  // The next 2 lines are optional.
  // This is how you configure MSAL to take advantage of the router's navigate functions when MSAL redirects between pages in your app
  const navigationClient = new MsalNavigationClient(router.navigate)
  msalInstance.setNavigationClient(navigationClient)

  return (
    <MsalProvider instance={msalInstance}>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </MsalProvider>
  )
}

export interface InnerAppProps {
  pca: IPublicClientApplication
}
