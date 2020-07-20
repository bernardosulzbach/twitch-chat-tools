import argparse
import datetime
import json
import pathlib
from contextlib import ContextDecorator

import semantic_version

import twitchio.dataclasses
from twitchio.ext import commands

VERSION = semantic_version.Version('1.0.0')

HOSTNAME = "irc.chat.twitch.tv"

SETTINGS_ARGUMENT = "settings"
CHANNEL_ARGUMENT = "channel"

LINE_BUFFERED = 1


class Settings:
    def __init__(self, username: str, password: str):
        assert username
        assert password.startswith("oauth:"), "Password is a OAuth Twitch API token."
        self.username: str = username
        self.password: str = password


def make_settings_from_path(path: pathlib.Path) -> Settings:
    text = path.read_text()
    json_object = json.loads(text)
    username = json_object["username"]
    password = json_object["password"]
    return Settings(username, password)


class LoggerBot(commands.Bot, ContextDecorator):
    def __init__(self, settings: Settings, channel: str):
        username, password = settings.username, settings.password
        super().__init__(irc_token=password, client_id=username, nick=username, prefix="!", initial_channels=[channel])
        self.channel: str = channel
        self.file_handle = None
        self.echos: int = 0

    def __enter__(self):
        self.file_handle = open(self.channel + "-" + datetime.datetime.now().isoformat() + ".txt", "w", buffering=LINE_BUFFERED)
        return self

    def __exit__(self, exception_type, exception_value, traceback):
        self.file_handle.close()
        return False

    async def event_ready(self):
        print(f"Ready as {self.nick}.")

    # Events don't need decorators when subclassed
    async def event_message(self, message: twitchio.dataclasses.Message):
        print(f"[{message.timestamp}] {message.author.display_name}: {message.content}", file=self.file_handle)
        self.echos += 1
        await self.handle_commands(message)

    async def event_pubsub(self, data):
        pass


def main():
    print(f"Running v{str(VERSION)}.")
    argument_parser = argparse.ArgumentParser()
    argument_parser.add_argument(SETTINGS_ARGUMENT)
    argument_parser.add_argument(CHANNEL_ARGUMENT)
    arguments = vars(argument_parser.parse_args())
    settings: Settings = make_settings_from_path(pathlib.Path(arguments[SETTINGS_ARGUMENT]))
    channel: str = arguments[CHANNEL_ARGUMENT]
    with LoggerBot(settings, channel) as bot:
        bot.run()


if __name__ == "__main__":
    main()
