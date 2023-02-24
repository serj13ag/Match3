using System.Collections.Generic;

namespace Commands
{
    public class CommandBlock
    {
        private readonly Queue<ICommand> _commands;

        private float _timeTillExecute;
        private ICommand _activeCommand;

        public bool IsActive => _activeCommand != null;

        public CommandBlock()
        {
            _commands = new Queue<ICommand>();
        }

        public void Update(float deltaTime)
        {
            if (_activeCommand == null)
            {
                return;
            }

            if (_timeTillExecute < 0f)
            {
                ExecuteActiveCommand();

                if (_commands.Count > 0)
                {
                    ActivateCommand(_commands.Dequeue());
                }
            }
            else
            {
                _timeTillExecute -= deltaTime;
            }
        }

        public void AddCommand(ICommand command)
        {
            _commands.Enqueue(command);

            if (_activeCommand == null)
            {
                ActivateCommand(_commands.Dequeue());
            }
        }

        private void ExecuteActiveCommand()
        {
            _activeCommand.Execute();
            _activeCommand = null;
        }

        private void ActivateCommand(ICommand command)
        {
            _activeCommand = command;
            _timeTillExecute = _activeCommand.Timeout;
        }
    }
}