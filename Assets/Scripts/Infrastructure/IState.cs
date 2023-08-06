namespace Infrastructure
{
    public interface IExitableState
    {
        bool IsGameLoopState { get; }
        void Exit();
    }

    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
}