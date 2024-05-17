import { useMutation, useQueryClient } from '@tanstack/react-query'
import * as GetTodoList from './use-get-todo-list'
import * as GetTodo from './use-get-todo'
import { TodoItem, UpdateTodoItemCommand, editTodo } from '@/api/todo'
import { produce } from 'immer'

export interface UseEditTodoOptions {
  onSuccess?: (editedTodo: TodoItem) => void
  onError?: (error: Error) => void
}

export function useEditTodo({ onSuccess, onError }: UseEditTodoOptions) {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: async (editedTodo: UpdateTodoItemCommand) => {
      const updatedTodo = await editTodo(editedTodo)

      return { ...editedTodo, ...updatedTodo }
    },
    onMutate: async (editedTodo) => {
      // Cancel any outgoing refetches
      // (so they don't overwrite our optimistic update)
      await queryClient.cancelQueries({ queryKey: GetTodoList.getQueryKey() })

      // Snapshot the previous value
      const previousTodoList = queryClient.getQueryData<TodoItem[]>(GetTodoList.getQueryKey())
      const previousTodo = queryClient.getQueryData<TodoItem>(GetTodo.getQueryKey(editedTodo.id))

      // Optimistically update to the new value
      queryClient.setQueryData<TodoItem[]>(
        GetTodoList.getQueryKey(),
        produce((draft) => {
          const todoIndex = draft?.findIndex((todo) => todo.id === editedTodo.id)
          if (draft === undefined || todoIndex === undefined || todoIndex === -1) {
            return
          }
          draft[todoIndex] = { ...draft[todoIndex], ...editedTodo }
        })
      )
      queryClient.setQueryData<UpdateTodoItemCommand>(
        GetTodo.getQueryKey(editedTodo.id),
        editedTodo
      )

      // Return a context object with the snapshotted value
      return { previousTodoList, previousTodo }
    },
    onSuccess,
    // If the mutation fails,
    // use the context returned from onMutate to roll back
    onError: (err, _newTodo, context) => {
      onError?.(err)
      if (context?.previousTodoList) {
        queryClient.setQueryData(GetTodoList.getQueryKey(), context.previousTodoList)
      }

      if (context?.previousTodo) {
        queryClient.setQueryData(GetTodo.getQueryKey(context.previousTodo.id), context.previousTodo)
      }
    },
    // Always refetch after error or success:
    onSettled: async (editedTodo) => {
      await queryClient.invalidateQueries({ queryKey: GetTodoList.getQueryKey() })
      if (editedTodo) {
        await queryClient.invalidateQueries({ queryKey: GetTodo.getQueryKey(editedTodo.id) })
      }
    },
  })
}
