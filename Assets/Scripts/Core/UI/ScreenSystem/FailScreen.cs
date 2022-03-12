using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.ScreenSystem
{
    public class FailScreen : Screen
    {
        public event Action RestartButtonClicked;
        [SerializeField] private Button _restartButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(() => { RestartButtonClicked?.Invoke(); });
        }
    }
}
