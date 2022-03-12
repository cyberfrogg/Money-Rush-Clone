using Core.PlayerMoneyWad;
using Core.UI.ScreenSystem;
using UnityEngine;

namespace Core
{
    public class Game
    {
        private MoneyWad _playerMoneyWad;
        private Screens _screens;

        private bool _onceTouched = false;

        public Game(MoneyWad playerMoneyWad, Screens screens)
        {
            _playerMoneyWad = playerMoneyWad;
            _screens = screens;
            _playerMoneyWad.Touched += onPlayerTouch;
        }

        public void Start()
        {
            _playerMoneyWad.StartMovement();
            _playerMoneyWad.CoinsEmptied += onCoinsEmptied;
            _screens.ShowOne(ScreenType.Game);
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
            _screens.ShowOne(ScreenType.Failed);
        }
    }
}