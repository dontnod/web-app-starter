import { getTodoList } from '@/api/todo'
import { useQuery, keepPreviousData, queryOptions } from '@tanstack/react-query'

const QUERY_KEY = 'todo'

export function getQueryKey() {
  return [QUERY_KEY]
}

export function useGetTodoList() {
  const query = useQuery(todoListQueryOptions())

  return query
}

export const todoListQueryOptions = () =>
  queryOptions({
    queryKey: getQueryKey(),
    queryFn: ({ signal }) => getTodoList(signal),
    placeholderData: keepPreviousData,
  })
