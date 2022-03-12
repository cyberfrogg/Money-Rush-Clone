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
            _playerMoneyWad.CoinsEmptied += onCoinsEmptied;
        }
        public void Stop()
        {
            _playerMoneyWad.StopMovement();
            _playerMoneyWad.CoinsEmptied -= onCoinsEmptied;
        }

        private void onPlayerTouch()
        {
            if (_onceTouched)
                return;

            _onceTouched = true;
            Start();
        }
        private void onCoinsEmptied()
        {
            Stop();
        }
    }
}