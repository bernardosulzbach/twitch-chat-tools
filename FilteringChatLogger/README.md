# FilteringChatLogger

```bash
dotnet build
dotnet bin/Debug/netcoreapp3.1/FilteringChatLogger.dll [SETTINGS] [CHANNEL]
```

Where SETTINGS should point to a JSON file with the following structure:

```json
{
  "username": "...",
  "password": "oauth:...",
  "path-to-chat-files": "/...",
  "path-to-log-files": "/..."
}
```
