// jest-dom adds custom jest matchers for asserting on DOM nodes.
// allows you to do things like:
// expect(element).toHaveTextContent(/react/i)
// learn more: https://github.com/testing-library/jest-dom
import { afterEach } from 'vitest'
import { cleanup } from '@testing-library/react'

// runs a clean after each test case (e.g. clearing jsdom)
afterEach(() => {
  cleanup()
})
