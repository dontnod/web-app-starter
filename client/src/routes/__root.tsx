import { App as AntdApp, Breadcrumb, Layout } from 'antd'
import { QueryClient } from '@tanstack/react-query'
import { Outlet, createRootRouteWithContext } from '@tanstack/react-router'
import { createStyles } from 'antd-style'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'
import { NavigationBar } from '@/components'
import { useBreadcrumbs } from '@/hooks/use-breadcrumbs'

export interface AppRouterContext {
  queryClient: QueryClient
}

export const Route = createRootRouteWithContext<AppRouterContext>()({
  component: RootComponent,
  beforeLoad: () => ({
    getTitle: () => 'Sample Web App',
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
  const breadcrumbs = useBreadcrumbs()

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
          DNE Sample Web App ©{new Date().getFullYear()} - Created by Don't Nod
        </Layout.Footer>
      </Layout>
      <TanStackRouterDevtools />
    </AntdApp>
  )
}
