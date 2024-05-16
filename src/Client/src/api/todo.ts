import { CONFIG } from '@/config/config'
import { getHttpClient } from './http-client'
import { components } from './todo-schema'

export type Todo = components['schemas']['ToDo']
export type CreateTodoDto = components['schemas']['CreateToDoDto']

export async function getTodoList(signal: AbortSignal) {
  const httpClient = getHttpClient(CONFIG.todoApi.scopes.read, { signal })

  const { data } = await httpClient.GET('/api/todolist')

  return data!
}

export async function getTodo(todoId: number, signal: AbortSignal) {
  const httpClient = getHttpClient(CONFIG.todoApi.scopes.read, { signal })

  const { data } = await httpClient.GET('/api/todolist/{id}', {
    params: { path: { id: todoId } },
  })

  return data!
}

export async function addTodo(todo: CreateTodoDto) {
  const httpClient = getHttpClient(CONFIG.todoApi.scopes.write, {})

  const { data } = await httpClient.POST('/api/todolist', { body: todo })

  return data!
}

export async function editTodo(todo: Todo) {
  const httpClient = getHttpClient(CONFIG.todoApi.scopes.write, {})

  const { data } = await httpClient.PUT('/api/todolist/{id}', {
    body: todo,
    params: { path: { id: todo.id } },
  })

  return data!
}

export async function deleteTodo(todoId: number) {
  const httpClient = getHttpClient(CONFIG.todoApi.scopes.write, {})

  const { data } = await httpClient.DELETE('/api/todolist/{id}', {
    params: { path: { id: todoId } },
  })

  return data!
}
