# DNE Web App Starter
# Client
[Client documentation](/client/README.md)

# Service
[Service documentation](/service/README.md)

# Docker
Create a `.env` at the root of the project with the following values :
```sh
AZURE_TENANT_ID={your_azure_tenant_id}
AZURE_CLIENT_ID={your_azure_api_application_client_id}
VITE_AZURE_CLIENT_ID={your_azure_spa_application_client_id}
VITE_AZURE_AUTHORITY=https://login.microsoftonline.com/{your_tenant_id}
VITE_TODO_API_ENDPOINT_URL=http://localhost:8080
VITE_TODO_API_SCOPES_READ=["api://{your_azure_api_application_id}/ToDoList.Read"]
VITE_TODO_API_SCOPES_WRITE=["api://{your_azure_api_application_id}/ToDoList.ReadWrite"]
```
```bash
docker compose up
```

# Dev Container
TODO