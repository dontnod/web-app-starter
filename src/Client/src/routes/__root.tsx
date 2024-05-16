import { App as AntdApp, Breadcrumb, Layout } from 'antd'
import { QueryClient } from '@tanstack/react-query'
import { Outlet, createRootRouteWithContext, useRouter } from '@tanstack/react-router'
import { createStyles } from 'antd-style'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'
import { NavigationBar } from '@/components'
import { useBreadcrumbs } from '@/hooks/use-breadcrumbs'
import { IPublicClientApplication } from '@azure/msal-browser'
import { Observable } from 'rxjs'
import { useMsal } from '@azure/msal-react'
import { MsalNavigationClient } from '@/utils/msal-navigation-client'
import { useEffect } from 'react'

export interface AppRouterContext {
  queryClient: QueryClient
  msalInstance: IPublicClientApplication
  isMsalReady: Observable<boolean>
}

export const Route = createRootRouteWithContext<AppRouterContext>()({
  component: RootComponent,
  beforeLoad: () => ({
    getTitle: () => 'Web App Starter',
  }),
})

const useStyles = createStyles(({ token }) => ({
  app: { minHeight: '100%', display: 'flex' },
  layout: {
    flex: 1,
    display: ' flex',
    flexWrap: 'wrap',
  },
  navigationBar: {},
  contentWrapper: {
    flex: '1',
    margin: '24px',
    minHeight: 'initial',
    display: 'flex',
    flexDirection: 'column',
  },
  content: {
    flex: '1',
    padding: '24px',
    margin: '12px',
    minHeight: 'initial',
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'center',
    background: token.colorBgContainer,
    borderRadius: token.borderRadiusLG,
  },
  footer: {
    textAlign: 'center',
  },
}))

function RootComponent() {
  const { styles } = useStyles()
  const { instance } = useMsal()
  const router = useRouter()
  const breadcrumbs = useBreadcrumbs()

  useEffect(() => {
    // Configuring the navigation client in order to bind MSAL to the app router
    // This allows MSAL to redirect to the asked route after a login
    // Note: this has to be under the `<RouterProvider router={router} />`
    // in order for navigation to work
    const navigationClient = new MsalNavigationClient(router)
    instance.setNavigationClient(navigationClient)
  }, [router, instance])

  return (
    <AntdApp className={styles.app}>
      <Layout className={styles.layout}>
        <NavigationBar className={styles.navigationBar} />
        <Layout.Content className={styles.contentWrapper}>
          <Breadcrumb items={breadcrumbs} />
          <div className={styles.content}>
            <Outlet />
          </div>
        </Layout.Content>
        <Layout.Footer className={styles.footer}>
          DNE Web App Starter ©{new Date().getFullYear()} - Created by Don't Nod
        </Layout.Footer>
      </Layout>
      <TanStackRouterDevtools />
    </AntdApp>
  )
}
