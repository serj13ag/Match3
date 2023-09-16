using Services;

namespace Infrastructure.StateMachine
{
    public interface IGameStateMachine : IService
    {
        bool InGameLoopState { get; }
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        void Update(float deltaTime);
    }
}