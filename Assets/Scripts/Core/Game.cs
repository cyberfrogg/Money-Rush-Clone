using Core.Enviroment.FinishScoreCounting;
using Core.Level;
using Core.PlayerMoneyWad;
using Core.UI.ScreenSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class Game
    {
        private MoneyWad _playerMoneyWad;
        private Screens _screens;
        private ScoreCounter _scoreCounter;
        private LevelSwitch _levelSwitch;

        private bool _onceTouched = false;

        public Game(MoneyWad playerMoneyWad, Screens screens, ScoreCounter scoreCounter, LevelSwitch levelSwitch)
        {
            _playerMoneyWad = playerMoneyWad;
            _screens = screens;
            _scoreCounter = scoreCounter;
            _levelSwitch = levelSwitch;
            _playerMoneyWad.Touched += onPlayerTouch;

            FailScreen failScreen = _screens.GetScreen(ScreenType.Failed) as FailScreen;
            failScreen.RestartButtonClicked += onRestartButtonClicked;
        }

        public void Start()
        {
            _playerMoneyWad.StartMovement();
            _playerMoneyWad.CoinsEmptied += onCoinsEmptied;
            _playerMoneyWad.Finished += onPlayerFinished;
            _scoreCounter.ScoreCountingDone += onScoreCountingDone;
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
        private void onScoreCountingDone(int score)
        {
            _scoreCounter.ScoreCountingDone -= onScoreCountingDone;
            _screens.ShowOne(ScreenType.Finish);
            FinishScreen finishScreen = _screens.GetScreen(ScreenType.Finish) as FinishScreen;
            finishScreen.NextButtonClicked += onNextLevelButtonClicked;
            finishScreen.SetScore(score);
        }
        private void onRestartButtonClicked()
        {
            _levelSwitch.RestartLevel();
        }
        private void onNextLevelButtonClicked()
        {
            _levelSwitch.LoadNextLevel();
        }
        private void onCoinsEmptied()
        {
            Stop();
            _screens.ShowOne(ScreenType.Failed);
        }
    }
}