import { IdTokenData } from '@/components'
import { useLoginRedirect } from '@/hooks/use-log-in'
import { useAccount } from '@azure/msal-react'
import { createFileRoute } from '@tanstack/react-router'
import { Alert, Button, Space } from 'antd'

export const Route = createFileRoute('/')({
  component: HomeComponent,
  beforeLoad: () => ({
    getTitle: () => 'Home',
  }),
})

function HomeComponent() {
  const account = useAccount()
  const loginRedirect = useLoginRedirect()

  return (
    <>
      {account ? (
        <IdTokenData idTokenClaims={account} />
      ) : (
        <Space direction="vertical" style={{ width: '100%' }}>
          <Alert
            message="Sign In"
            description="Please sign in to access the app"
            showIcon
            banner
            type="warning"
            action={
              <Button size="small" onClick={loginRedirect}>
                Sign In
              </Button>
            }
          />
        </Space>
      )}
    </>
  )
}
