import { TodoForm, TodoList } from '@/components'
import { useAddTodo } from '@/features/todo'
import { createFileRoute } from '@tanstack/react-router'
import { message } from 'antd'

export const Route = createFileRoute('/todo/')({
  component: TodoComponent,
})

function TodoComponent() {
  const [messageApi, contextHolder] = message.useMessage()
  const addTodoMutation = useAddTodo({
    onSuccess: async () => {
      await messageApi.success('Todo has been created')
    },
    onError: async (err) => {
      await messageApi.error(err.toString())
    },
  })

  const handleFormSubmitted = (todo: TodoForm) => {
    addTodoMutation.mutate(todo)
  }

  return (
    <>
      {contextHolder}

      <TodoForm onSubmitted={handleFormSubmitted} submitText="Add" />
      <TodoList />
    </>
  )
}
