namespace Services.Board.States
{
    public abstract class BaseTimeoutBoardState : IBoardState
    {
        private float _timeTillExecute;
        private bool _isExecuted;

        protected BaseTimeoutBoardState(float timeTillExecute)
        {
            _timeTillExecute = timeTillExecute;
        }

        public void Update(float deltaTime)
        {
            if (_isExecuted)
            {
                return;
            }

            if (_timeTillExecute < 0f)
            {
                OnTimeoutEnded();
                _isExecuted = true;
            }
            else
            {
                _timeTillExecute -= deltaTime;
            }
        }

        protected abstract void OnTimeoutEnded();
    }
}