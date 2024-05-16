import { fireEvent, screen, waitFor } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'
import '@testing-library/jest-dom/vitest'
import { TodoList } from './TodoList'
import { renderWithRouter } from '@/__tests__/mocks/render-with-router'

const mockTodos = [
  { id: 1, description: 'Buy milk' },
  { id: 2, description: 'Read book' },
]

// Mocking the useGetTodoList and useDeleteTodo hooks
vi.mock('@/features/todo', () => ({
  useGetTodoList: () => ({
    data: mockTodos,
    isFetching: false,
  }),
  useDeleteTodo: () => ({
    mutate: vi.fn(),
  }),
}))

describe('TodoList Component', () => {
  it('renders the todo list', async () => {
    renderWithRouter(<TodoList />)

    await waitFor(() => {
      expect(screen.getByText('Buy milk')).toBeInTheDocument()
      expect(screen.getByText('Read book')).toBeInTheDocument()
    })
  })

  it('handles navigation on edit button click', async () => {
    const navigateMock = vi.hoisted(() => vi.fn())

    vi.mock('@tanstack/react-router', async (importOriginal) => ({
      ...(await importOriginal<typeof import('@tanstack/react-router')>()),
      useNavigate: () => navigateMock,
    }))

    renderWithRouter(<TodoList />)
    await waitFor(() => {
      fireEvent.click(screen.getAllByRole('button', { name: 'edit' })[0])
    })

    expect(navigateMock).toHaveBeenCalledOnce()
  })

  it('triggers delete mutation on delete confirm', async () => {
    const deleteMock = vi.hoisted(() => vi.fn())
    vi.mock('@/features/todo', () => ({
      useDeleteTodo: () => ({
        mutate: deleteMock,
      }),
      useGetTodoList: () => ({
        data: mockTodos,
        isFetching: false,
      }),
    }))

    renderWithRouter(<TodoList />)
    await waitFor(() => {
      fireEvent.click(screen.getAllByRole('button', { name: 'delete' })[0])
    })

    await waitFor(() => {
      fireEvent.click(screen.getByText('Yes'))
    })

    expect(deleteMock).toHaveBeenCalledOnce()
  })
})
