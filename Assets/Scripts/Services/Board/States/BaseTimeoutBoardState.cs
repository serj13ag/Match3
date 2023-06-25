namespace Services.Board.States
{
    public abstract class BaseTimeoutBoardState
    {
        private float _timeTillExecute;

        protected BaseTimeoutBoardState(float timeTillExecute)
        {
            _timeTillExecute = timeTillExecute;
        }

        public void Update(float deltaTime)
        {
            if (_timeTillExecute < 0f)
            {
                OnTimeoutEnded();
            }
            else
            {
                _timeTillExecute -= deltaTime;
            }
        }

        protected abstract void OnTimeoutEnded();
    }
}