import { useMutation, useQueryClient } from '@tanstack/react-query'
import * as GetTodoList from './use-get-todo-list'
import { Todo, deleteTodo } from '@/api/todo'
import { produce } from 'immer'

export interface UseAddTodoOptions {
  onSuccess?: () => void
  onError?: (error: Error) => void
}

export function useDeleteTodo({ onSuccess, onError }: UseAddTodoOptions) {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: async (todoId: number) => {
      await deleteTodo(todoId)
      return todoId
    },
    onMutate: async (deletedTodoId) => {
      // Cancel any outgoing refetches
      // (so they don't overwrite our optimistic update)
      await queryClient.cancelQueries({ queryKey: GetTodoList.getQueryKey() })

      // Snapshot the previous value
      const previousTodoList = queryClient.getQueryData<Todo[]>(GetTodoList.getQueryKey())

      // Optimistically delete the value
      queryClient.setQueryData<Todo[]>(
        GetTodoList.getQueryKey(),
        produce((draft) => {
          const todoIndex = draft?.findIndex((todo) => todo.id === deletedTodoId)
          if (draft === undefined || todoIndex === undefined || todoIndex === -1) {
            return
          }
          draft.splice(todoIndex, 1)
        })
      )

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
    onSettled: async () => {
      await queryClient.invalidateQueries({ queryKey: GetTodoList.getQueryKey() })
    },
  })
}
