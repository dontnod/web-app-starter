import { useRouter } from '@tanstack/react-router'
import { BreadcrumbProps } from 'antd'
import { ItemType } from 'antd/es/breadcrumb/Breadcrumb'

type BreadcrumbItem = ItemType

export function useBreadcrumbs(): BreadcrumbProps['items'] {
  const router = useRouter()

  return router.state.matches
    .map((match) => {
      const routeContext: { getTitle?: () => string } = match.routeContext as object
      const title = routeContext.getTitle?.()

      if (title === undefined) {
        return []
      }

      const splitedPath = title.split('/')

      if (splitedPath.length > 0) {
        return splitedPath.map<BreadcrumbItem>((value, index) => {
          const to = index === splitedPath.length - 1 ? match.pathname : undefined
          return {
            title: value,
            href: to,
          }
        })
      }

      return [
        {
          title,
          href: match.pathname,
        },
      ]
    })
    .flat()
    .map((item) => ({
      ...item,
      ...(item.href && {
        onClick: (e) => {
          // Prevents browser to refresh the page when navigating to a breadcrumb item
          e.preventDefault()
          router.navigate({ to: item.href })
        },
      }),
    }))
}
