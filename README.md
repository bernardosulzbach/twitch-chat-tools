# Twitch chat tools

## filtering-chat-logger

This tool logs live Twitch chat to a text file.
 
```python3
python3 filtering-chat-logger.py [SETTINGS] [CHANNEL]
```

Settings should be a path to a JSON file of the following format:
 
```json
{
  "username": "...",
  "password": "oauth:..."
}
```

## Useful links

Built using [TwitchIO](https://github.com/TwitchIO/TwitchIO).

To get a token, log in to Twitch with the bot's account and visit: [https://twitchapps.com/tmi/](https://twitchapps.com/tmi/).

### Twitch documentation links

+ [https://dev.twitch.tv/docs/authentication](https://dev.twitch.tv/docs/authentication)
+ [https://dev.twitch.tv/docs/irc](https://dev.twitch.tv/docs/irc)
