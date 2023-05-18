namespace Infrastructure.StateMachine
{
    public class GameLoopState : IState
    {
        public void Enter()
        {
            AllServices.Instance.SceneController.UpdateSceneNameText();
        }

        public void Exit()
        {
        }
    }
}