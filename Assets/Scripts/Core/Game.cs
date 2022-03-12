using Core.PlayerMoneyWad;

namespace Core
{
    public class Game
    {
        private MoneyWad _playerMoneyWad;

        private bool _onceTouched = false;

        public Game(MoneyWad playerMoneyWad)
        {
            _playerMoneyWad = playerMoneyWad;
            _playerMoneyWad.Touched += onPlayerTouch;
        }

        public void Start()
        {
            _playerMoneyWad.StartMovement();
        }
        public void Stop()
        {
            _playerMoneyWad.StopMovement();
        }

        private void onPlayerTouch()
        {
            if (_onceTouched)
                return;

            _onceTouched = true;
            Start();
        }
    }
}