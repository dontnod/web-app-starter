import { LOGIN_REQUEST } from '@/config/auth-config'
import { IPublicClientApplication } from '@azure/msal-browser'
import { useMsal } from '@azure/msal-react'

export async function loginPopup(instance: IPublicClientApplication) {
  return await instance.loginPopup({
    ...LOGIN_REQUEST,
    redirectUri: '/redirect.html',
  })
}

export async function loginRedirect(instance: IPublicClientApplication) {
  return await instance.loginRedirect({ ...LOGIN_REQUEST })
}

export function useLoginPopup() {
  const { instance } = useMsal()

  /**
   * When using popup and silent APIs, we recommend setting the redirectUri to a blank page or a page
   * that does not implement MSAL. Keep in mind that all redirect routes must be registered with the application
   * For more information, please follow this link: https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/login-user.md#redirecturi-considerations
   */

  return async () => await loginPopup(instance)
}

export function useLoginRedirect() {
  const { instance } = useMsal()

  return async () => await loginRedirect(instance)
}
