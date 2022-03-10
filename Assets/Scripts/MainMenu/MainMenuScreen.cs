using Core.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuScreen
    {
        private LevelSwitch _levelSwitch;
        private Button _playButton;

        public MainMenuScreen(LevelSwitch levelSwitch, Button playButton)
        {
            _levelSwitch = levelSwitch;
            _playButton = playButton;
        }
        public void Start()
        {
            _playButton.onClick.AddListener(onPlayButtonClicked);
        }

        private void onPlayButtonClicked()
        {
            _playButton.onClick.RemoveListener(onPlayButtonClicked);
            _levelSwitch.LoadNextLevel();
        }
    }
}