import { getTodo } from '@/api/todo'
import { useQuery, keepPreviousData, queryOptions } from '@tanstack/react-query'
import * as GetTodoList from './use-get-todo-list'

export function getQueryKey(todoId: number) {
  return [...GetTodoList.getQueryKey(), todoId]
}

export function useGetTodo(todoId: number) {
  const query = useQuery(todoQueryOptions(todoId))

  return query
}

export const todoQueryOptions = (todoId: number) =>
  queryOptions({
    queryKey: getQueryKey(todoId),
    queryFn: ({ signal }) => getTodo(todoId, signal),
    placeholderData: keepPreviousData,
  })
