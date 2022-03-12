using System;
using TMPro;
using UnityEngine;

namespace Core.UI.ScreenSystem
{
    public class FinishScreen : Screen
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _prefix;

        public void SetScore(int score)
        {
            _text.text = $"{_prefix}{score}00";
        }
    }
}
