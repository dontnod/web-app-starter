import { PROTECTED_RESOURCES } from '@/config/auth-config'
import { getHttpClient } from './http-client'
import { components } from './todo-schema'

export type Todo = components['schemas']['ToDo']
export type CreateTodoDto = components['schemas']['CreateToDoDto']

export async function getTodoList(signal: AbortSignal) {
  const httpClient = getHttpClient(PROTECTED_RESOURCES.toDoListAPI.scopes.read, { signal })

  const { data, error } = await httpClient.GET('/api/todolist')

  if (data === undefined) {
    throw new Error(error)
  }

  return data
}

export async function getTodo(todoId: number, signal: AbortSignal) {
  const httpClient = getHttpClient(PROTECTED_RESOURCES.toDoListAPI.scopes.read, { signal })

  const { data, error } = await httpClient.GET('/api/todolist/{id}', {
    params: { path: { id: todoId } },
  })

  if (data === undefined) {
    throw new Error(error)
  }

  return data
}

export async function addTodo(todo: CreateTodoDto) {
  const httpClient = getHttpClient(PROTECTED_RESOURCES.toDoListAPI.scopes.write, {})

  const { data, error } = await httpClient.POST('/api/todolist', { body: todo })

  if (data === undefined) {
    throw new Error(error)
  }

  return data
}

export async function editTodo(todo: Todo) {
  const httpClient = getHttpClient(PROTECTED_RESOURCES.toDoListAPI.scopes.write, {})

  const { data, error } = await httpClient.PUT('/api/todolist/{id}', {
    body: todo,
    params: { path: { id: todo.id } },
  })

  if (data === undefined) {
    throw new Error(error)
  }

  return data
}

export async function deleteTodo(todoId: number) {
  const httpClient = getHttpClient(PROTECTED_RESOURCES.toDoListAPI.scopes.write, {})

  const { data } = await httpClient.DELETE('/api/todolist/{id}', {
    params: { path: { id: todoId } },
  })

  return data
}
