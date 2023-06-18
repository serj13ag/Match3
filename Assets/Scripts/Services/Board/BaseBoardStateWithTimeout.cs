namespace Services.Board
{
    public abstract class BaseBoardStateWithTimeout
    {
        private float _timeTillExecute;

        protected BaseBoardStateWithTimeout(float timeTillExecute)
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