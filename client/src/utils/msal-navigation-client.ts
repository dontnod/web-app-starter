import { NavigationClient, NavigationOptions } from '@azure/msal-browser'
import { UseNavigateResult } from '@tanstack/react-router'

/**
 * This is an example for overriding the default function MSAL uses to navigate to other urls in your webpage
 */
export class MsalNavigationClient extends NavigationClient {
  private navigate: UseNavigateResult<string>

  constructor(navigate: UseNavigateResult<string>) {
    super()
    this.navigate = navigate
  }

  /**
   * Navigates to other pages within the same web application
   * You can use the useNavigate hook provided by react-router-dom to take advantage of client-side routing
   * @param url
   * @param options
   */
  async navigateInternal(url: string, options: NavigationOptions) {
    const relativePath = url.replace(window.location.origin, '')
    if (options.noHistory) {
      this.navigate({ replace: true, to: relativePath })
    } else {
      this.navigate({ to: relativePath })
    }

    return false
  }
}
