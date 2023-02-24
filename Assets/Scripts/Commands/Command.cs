using System;

namespace Commands
{
    public class Command : ICommand
    {
        private readonly Action _action;

        public float Timeout { get; }

        public Command(Action action, float timeout)
        {
            Timeout = timeout;
            _action = action;
        }

        public void Execute()
        {
            _action?.Invoke();
        }
    }
}