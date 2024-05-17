import { TodoItem } from '@/api/todo'
import { TodoForm } from '@/components'
import { useEditTodo, useGetTodo } from '@/features/todo'
import { createFileRoute } from '@tanstack/react-router'
import { Spin, message } from 'antd'

export const Route = createFileRoute('/todo/$id')({
  component: EditTodoComponent,
  beforeLoad: (context) => ({
    getTitle: () => context.params.id,
  }),
})

function EditTodoComponent() {
  const [messageApi, contextHolder] = message.useMessage()
  const addTodoMutation = useEditTodo({
    onSuccess: async () => {
      await messageApi.success('Todo has been updated')
    },
    onError: async (err) => {
      await messageApi.error(err.toString())
    },
  })
  const params = Route.useParams()
  const todoQuery = useGetTodo(parseFloat(params.id))

  const handleFormSubmitted = (todo: TodoForm) => {
    addTodoMutation.mutate(todo as TodoItem)
  }

  return (
    <>
      {contextHolder}
      {todoQuery.isLoading && <Spin spinning fullscreen />}
      {todoQuery.data && (
        <TodoForm
          onSubmitted={handleFormSubmitted}
          defaultValues={todoQuery.data}
          submitText="Save"
        />
      )}
    </>
  )
}
