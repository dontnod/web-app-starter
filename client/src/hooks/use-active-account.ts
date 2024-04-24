import { AccountInfo } from '@azure/msal-browser'
import { useMsal } from '@azure/msal-react'

export function useActiveAccount(): AccountInfo | null {
  const { instance } = useMsal()

  return instance.getActiveAccount()
}
