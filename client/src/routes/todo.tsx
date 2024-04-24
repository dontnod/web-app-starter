import { Outlet, createFileRoute } from '@tanstack/react-router'
import { Flex } from 'antd'

export const Route = createFileRoute('/todo')({
  component: TodoComponent,
  beforeLoad: () => ({
    getTitle: () => 'Todo',
  }),
})

function TodoComponent() {
  return (
    <>
      <Flex
        vertical
        align="stretch"
        gap={12}
        style={{
          flex: '1',
          maxWidth: '900px',
        }}
      >
        <Outlet />
      </Flex>
    </>
  )
}
