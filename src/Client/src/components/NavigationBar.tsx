import { useLoginPopup, useLoginRedirect } from '@/hooks/use-log-in'
import { useLogoutPopup, useLogoutRedirect } from '@/hooks/use-log-out'
import { LoginOutlined, LogoutOutlined } from '@ant-design/icons'
import {
  AuthenticatedTemplate,
  UnauthenticatedTemplate,
  useIsAuthenticated,
} from '@azure/msal-react'
import { useNavigate, useRouterState } from '@tanstack/react-router'
import { Layout, Menu, Dropdown, Button, MenuProps } from 'antd'

const TOP_NAVIGATION_MENU_DEFAULT = [
  {
    key: '/',
    label: 'Home',
  },
] as const

export interface NavigationBarProps {
  className?: string
}

export function NavigationBar({ className }: NavigationBarProps) {
  const router = useRouterState()
  const navigate = useNavigate()
  const loginPopup = useLoginPopup()
  const loginRedirect = useLoginRedirect()
  const logoutPopup = useLogoutPopup()
  const logoutRedirect = useLogoutRedirect()

  const isAuthenticated = useIsAuthenticated()

  const topNavigationMenu: MenuProps['items'] = [
    ...TOP_NAVIGATION_MENU_DEFAULT,
    ...(isAuthenticated
      ? [
          {
            key: '/todo',
            label: ' Todo',
          },
        ]
      : []),
  ]

  const signInMenu: MenuProps = {
    items: [
      {
        label: 'Sign in using Popup',
        key: 'popup',
        onClick: loginPopup,
      },
      {
        label: 'Sign in using Redirect',
        key: 'redirect',
        onClick: loginRedirect,
      },
    ],
  }

  const signOutMenu: MenuProps = {
    items: [
      {
        label: 'Sign Out using Popup',
        key: 'popup',
        onClick: logoutPopup,
      },
      {
        label: 'Sign out using Redirect',
        key: 'redirect',
        onClick: logoutRedirect,
      },
    ],
  }

  const handleMenuNavigation = async (key: string) => {
    await navigate({ to: key })
  }

  return (
    <Layout.Header className={className} style={{ display: 'flex', alignItems: 'center' }}>
      <div className="demo-logo" />
      <Menu
        theme="dark"
        mode="horizontal"
        selectedKeys={[router.location.pathname]}
        items={topNavigationMenu}
        style={{ flex: 1 }}
        onClick={(e) => handleMenuNavigation(e.key)}
      />

      <UnauthenticatedTemplate>
        <Dropdown menu={signInMenu}>
          <Button title="Sign In" icon={<LoginOutlined />}>
            Sign In
          </Button>
        </Dropdown>
      </UnauthenticatedTemplate>

      <AuthenticatedTemplate>
        <Dropdown menu={signOutMenu}>
          <Button title="Log out" icon={<LogoutOutlined />}>
            Log Out
          </Button>
        </Dropdown>
      </AuthenticatedTemplate>
    </Layout.Header>
  )
}
