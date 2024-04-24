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
    onSuccess: () => {
      messageApi.success('Todo has been created')
    },
    onError: (err) => {
      messageApi.error(err.toString())
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
