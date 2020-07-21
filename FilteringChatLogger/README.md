# FilteringChatLogger

A C# (.NET Core) application that logs live Twitch chat to a text file.

## Build instructions

```bash
dotnet build
```

## Running

```bash
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
