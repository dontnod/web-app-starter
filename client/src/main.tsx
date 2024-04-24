import React from 'react'
import ReactDOM from 'react-dom/client'
import { App } from './App.tsx'

import { AuthenticationResult, EventType, PublicClientApplication } from '@azure/msal-browser'
import { MSAL_CONFIG } from '@/config/auth-config.ts'

// Reset css rules for all browsers
import 'antd/dist/reset.css'
import './index.css'

/**
 * MSAL should be instantiated outside of the component tree to prevent it from being re-instantiated on re-renders.
 * For more, visit: https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/getting-started.md
 */
export const msalInstance = new PublicClientApplication(MSAL_CONFIG)

// Default to using the first account if no account is active on page load
if (!msalInstance.getActiveAccount() && msalInstance.getAllAccounts().length > 0) {
  // Account selection logic is app dependent. Adjust as needed for different use cases.
  msalInstance.setActiveAccount(msalInstance.getAllAccounts()[0])
}

// Optional - This will update account state if a user signs in from another tab or window
msalInstance.enableAccountStorageEvents()

// Listen for sign-in event and set active account
msalInstance.addEventCallback((event) => {
  if (event.eventType === EventType.LOGIN_SUCCESS && event.payload) {
    const payload = event.payload as AuthenticationResult
    const account = payload.account

    msalInstance.setActiveAccount(account)
  }
})

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App msalInstance={msalInstance} />
  </React.StrictMode>
)
