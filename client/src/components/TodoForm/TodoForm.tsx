import { z } from 'zod'
import { Button, Form, Input } from 'antd'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { FormItem } from 'react-hook-form-antd'
import { useEffect } from 'react'

const notEmptyString = z.string().trim().min(1, { message: 'Required' })

const todoFormSchema = z.object({
  id: z.number().optional(),
  description: notEmptyString,
  owner: z.string().optional(),
})

export type TodoForm = z.input<typeof todoFormSchema>

export interface TodoFormProps {
  onSubmitted: (todo: TodoForm) => void
  defaultValues?: TodoForm
  submitText: string
}

export function TodoForm({ onSubmitted, submitText, defaultValues }: TodoFormProps) {
  const form = useForm<TodoForm>({
    defaultValues,
    mode: 'onSubmit',
    resolver: zodResolver(todoFormSchema),
  })

  useEffect(() => {
    form.reset(defaultValues)
  }, [defaultValues, form])

  const handleFormSubmitted = (values: TodoForm) => {
    form.reset()
    onSubmitted(values)
  }

  return (
    <Form
      onFinish={form.handleSubmit(handleFormSubmitted)}
      layout="inline"
      style={{ display: 'flex' }}
    >
      <FormItem
        name="description"
        control={form.control}
        fieldId="descriptionInput"
        style={{ flex: 1 }}
      >
        <Input placeholder="Enter task description" id="descriptionInput" />
      </FormItem>
      <Form.Item>
        <Button type="primary" htmlType="submit">
          {submitText}
        </Button>
      </Form.Item>
    </Form>
  )
}
