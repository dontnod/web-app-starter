import {
  Outlet,
  RouterProvider,
  createRootRoute,
  createRoute,
  createRouter,
} from '@tanstack/react-router'
import { render } from '@testing-library/react'

export function renderWithRouter(component: React.ReactNode) {
  const rootRoute = createRootRoute({
    component: Outlet,
  })

  const indexRoute = createRoute({
    getParentRoute: () => rootRoute,
    path: '/',
    component: () => component,
  })

  const routeTree = rootRoute.addChildren([indexRoute])
  const router = createRouter({ routeTree })

  // this is rtl's render
  render(<RouterProvider router={router} />)
}
