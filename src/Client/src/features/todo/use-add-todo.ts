import { useMutation, useQueryClient } from '@tanstack/react-query'
import * as GetTodoList from './use-get-todo-list'
import * as GetTodo from './use-get-todo'
import { CreateTodoItemCommand, TodoItem, addTodo } from '@/api/todo'

export interface UseAddTodoOptions {
  onSuccess?: (addedTodo: TodoItem) => void
  onError?: (error: Error) => void
}

export function useAddTodo({ onSuccess, onError }: UseAddTodoOptions) {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (newTodo: CreateTodoItemCommand) => {
      return addTodo(newTodo)
    },
    onMutate: async (newTodo) => {
      // Cancel any outgoing refetches
      // (so they don't overwrite our optimistic update)
      await queryClient.cancelQueries({ queryKey: GetTodoList.getQueryKey() })

      // Snapshot the previous value
      const previousTodoList = queryClient.getQueryData<TodoItem[]>(GetTodoList.getQueryKey())

      // Makes a guess about the next todo id
      const nextId = (previousTodoList?.at(-1)?.id ?? 0) + 1

      // Optimistically update to the new value
      queryClient.setQueryData(GetTodoList.getQueryKey(), (old: TodoItem[]) => [
        ...old,
        { id: nextId, ...newTodo },
      ])

      // Return a context object with the snapshotted value
      return { previousTodoList }
    },
    onSuccess,
    // If the mutation fails,
    // use the context returned from onMutate to roll back
    onError: (err, _newTodo, context) => {
      onError?.(err)
      if (!context?.previousTodoList) {
        return
      }
      queryClient.setQueryData(GetTodoList.getQueryKey(), context.previousTodoList)
    },
    // Always refetch after error or success:
    onSettled: async (addedTodo) => {
      await queryClient.invalidateQueries({ queryKey: GetTodoList.getQueryKey() })
      if (addedTodo) {
        await queryClient.invalidateQueries({ queryKey: GetTodo.getQueryKey(addedTodo.id) })
      }
    },
  })
}
