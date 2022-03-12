using System;
using UnityEngine;

namespace Core.UI.ScreenSystem
{
    public class ScreensInstaller : MonoBehaviour
    {
        [SerializeField] private Screen[] _screenList;

        private Screens _screens;

        public Screens CreateScreens()
        {
            _screens = new Screens(_screenList);
            _screens.ShowOne(ScreenType.Start);
            _screens.Toggle(ScreenType.Game, true);
            return _screens;
        }
    }
}
