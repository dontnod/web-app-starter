import { act, render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import '@testing-library/jest-dom/vitest'
import { describe, beforeEach, it, expect, vi } from 'vitest'
import { TodoForm } from '../TodoForm'

describe('TodoForm Component', () => {
  const mockOnSubmitted = vi.fn()

  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('renders without crashing', () => {
    render(<TodoForm onSubmitted={mockOnSubmitted} submitText="Add Todo" />)
    expect(screen.getByPlaceholderText('Enter task description')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Add Todo' })).toBeInTheDocument()
  })

  it('submits valid form data', async () => {
    render(<TodoForm onSubmitted={mockOnSubmitted} submitText="Add Todo" />)

    await userEvent.type(screen.getByPlaceholderText('Enter task description'), 'Buy milk')
    await act(async () => {
      await userEvent.click(screen.getByRole('button', { name: 'Add Todo' }))
    })

    await waitFor(() => {
      expect(mockOnSubmitted).toHaveBeenCalledWith({
        description: 'Buy milk',
      })
    })
  })

  it('handles default values', () => {
    render(
      <TodoForm
        onSubmitted={mockOnSubmitted}
        submitText="Edit Todo"
        defaultValues={{ description: 'Read book' }}
      />
    )
    expect(screen.getByDisplayValue('Read book')).toBeInTheDocument()
  })

  it('validates input fields', async () => {
    render(<TodoForm onSubmitted={mockOnSubmitted} submitText="Add Todo" />)
    await act(async () => {
      await userEvent.click(screen.getByRole('button', { name: 'Add Todo' }))
    })

    await waitFor(() => {
      // Assuming your form displays error messages for validation
      expect(screen.getByText('Required')).toBeInTheDocument()
    })
  })
})
