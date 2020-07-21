using System;

namespace FilteringChatLogger
{
    public class Command
    {
        private readonly Action<ChatBot> _action;
        public readonly string Description;
        public readonly string Name;

        public Command(string name, string description, Action<ChatBot> action)
        {
            this.Name = name;
            this.Description = description;
            this._action = action;
        }

        public bool Matches(string invocation)
        {
            return invocation.StartsWith(Name);
        }

        public void Execute(in ChatBot chatBot)
        {
            _action(chatBot);
        }
    }
}