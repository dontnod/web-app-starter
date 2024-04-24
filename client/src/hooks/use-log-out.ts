import { useMsal } from '@azure/msal-react'

export function useLogoutPopup() {
  const { instance } = useMsal()

  return async () =>
    instance.logoutPopup({
      mainWindowRedirectUri: '/', // redirects the top level app after logout
      account: instance.getActiveAccount(),
    })
}

export function useLogoutRedirect() {
  const { instance } = useMsal()

  return async () =>
    await instance.logoutRedirect({
      account: instance.getActiveAccount(),
    })
}
