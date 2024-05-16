/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_AZURE_CLIENT_ID: string
  readonly VITE_AZURE_AUTHORITY: string
  readonly VITE_TODO_API_ENDPOINT_URL: string
  readonly VITE_TODO_API_SCOPES_READ: string
  readonly VITE_TODO_API_SCOPES_WRITE: string
  // more env variables...
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
