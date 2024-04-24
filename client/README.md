# DNE Web App Starter - Client

### Install dependencies
```sh
yarn install
```

### Generate api schema (need to be run after the first build of the API)
```sh
yarn generate-api
```

# Environment
Copy the `.env.example` to `.env`, then edit the following value:
```sh
# Client ID of the SPA Application in Azure
VITE_AZURE_CLIENT_ID=aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa

# Azure authority for the login (generally https://login.microsoftonline.com/{your_tenant_id})
VITE_AZURE_AUTHORITY=https://login.microsoftonline.com/bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb
```