import { useDeleteTodo, useGetTodoList } from '@/features/todo'
import { DeleteOutlined, EditOutlined } from '@ant-design/icons'
import { Link, useNavigate } from '@tanstack/react-router'
import { Button, List, Popconfirm, message } from 'antd'

export function TodoList() {
  const [messageApi, contextHolder] = message.useMessage()

  const navigate = useNavigate({ from: '/todo/$id' })
  const todoListQuery = useGetTodoList()

  const deleteTodoMutation = useDeleteTodo({
    onSuccess: async () => {
      await messageApi.success('Todo has been deleted')
    },
    onError: async (err) => {
      await messageApi.error(err.toString())
    },
  })

  return (
    <>
      {contextHolder}
      <List
        size="small"
        style={{ flex: 1 }}
        itemLayout="horizontal"
        loading={todoListQuery.isFetching}
        dataSource={todoListQuery.data}
        renderItem={(todo) => (
          <List.Item
            key={todo.id}
            actions={[
              <Button
                type="default"
                name="edit"
                icon={<EditOutlined />}
                onClick={() => navigate({ to: '/todo/$id', params: { id: todo.id.toString() } })}
              ></Button>,
              <Popconfirm
                title="Delete the task"
                description="Are you sure to delete this task?"
                okText="Yes"
                cancelText="No"
                onConfirm={() => deleteTodoMutation.mutate(todo.id)}
              >
                <Button name="delete" danger icon={<DeleteOutlined />}></Button>
              </Popconfirm>,
            ]}
          >
            <List.Item.Meta
              description={`id: ${todo.id}`}
              title={
                <Link to="/todo/$id" params={{ id: todo.id.toString() }}>
                  {todo.description}
                </Link>
              }
            />
          </List.Item>
        )}
      />
    </>
  )
}
