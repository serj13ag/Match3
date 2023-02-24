namespace Commands
{
    public interface ICommand
    {
        void Execute();
        float Timeout { get; }
    }
}