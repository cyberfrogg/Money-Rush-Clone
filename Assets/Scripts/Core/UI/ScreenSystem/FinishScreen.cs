using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.ScreenSystem
{
    public class FinishScreen : Screen
    {
        public event Action NextButtonClicked;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _nextButton;
        [SerializeField] private string _prefix;

        private void Awake()
        {
            _nextButton.onClick.AddListener(() => { NextButtonClicked?.Invoke(); });
        }

        public void SetScore(int score)
        {
            _text.text = $"{_prefix}{score}00";
        }
    }
}
