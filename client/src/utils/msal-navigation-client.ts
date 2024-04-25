import { RouterType } from '@/App'
import { NavigationClient, NavigationOptions } from '@azure/msal-browser'
import { Router } from '@tanstack/react-router'

/**
 * This is an example for overriding the default function MSAL uses to navigate to other urls in your webpage
 */
export class MsalNavigationClient extends NavigationClient {
  private readonly router: RouterType

  constructor(router: Router) {
    super()
    this.router = router
  }

  /**
   * Navigates to other pages within the same web application
   * You can use the useNavigate hook provided by react-router-dom to take advantage of client-side routing
   * @param url
   * @param options
   */
  async navigateInternal(url: string, options: NavigationOptions) {
    const relativePath = url.replace(window.location.origin, '')

    this.router.state.location.href = relativePath
    if (options.noHistory) {
      this.router.navigate({ replace: true, to: relativePath })
    } else {
      this.router.navigate({ to: relativePath })
    }

    return false
  }
}
