using Core.Enviroment.FinishScoreCounting;
using Core.PlayerMoneyWad;
using Core.UI.ScreenSystem;
using UnityEngine;

namespace Core
{
    public class Game
    {
        private MoneyWad _playerMoneyWad;
        private Screens _screens;
        private ScoreCounter _scoreCounter;

        private bool _onceTouched = false;

        public Game(MoneyWad playerMoneyWad, Screens screens, ScoreCounter scoreCounter)
        {
            _playerMoneyWad = playerMoneyWad;
            _screens = screens;
            _scoreCounter = scoreCounter;
            _playerMoneyWad.Touched += onPlayerTouch;
        }

        public void Start()
        {
            _playerMoneyWad.StartMovement();
            _playerMoneyWad.CoinsEmptied += onCoinsEmptied;
            _playerMoneyWad.Finished += onPlayerFinished;
            _screens.ShowOne(ScreenType.Game);
        }
        public void Stop()
        {
            _playerMoneyWad.StopMovement();
            _playerMoneyWad.CoinsEmptied -= onCoinsEmptied;
            _playerMoneyWad.Finished -= onPlayerFinished;
        }

        private void onPlayerTouch()
        {
            if (_onceTouched)
                return;

            _onceTouched = true;
            Start();
        }
        private void onPlayerFinished()
        {
            _scoreCounter.CountScore(_playerMoneyWad);
            Stop();
        }
        private void onCoinsEmptied()
        {
            Stop();
            _screens.ShowOne(ScreenType.Failed);
        }
    }
}