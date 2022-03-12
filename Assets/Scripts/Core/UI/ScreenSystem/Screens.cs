using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.UI.ScreenSystem
{
    public class Screens
    {
        private IReadOnlyCollection<Screen> _screens;

        public Screens(IReadOnlyCollection<Screen> screens)
        {
            _screens = screens;
        }
        public void ShowOne(ScreenType type)
        {
            foreach (Screen screen in _screens)
            {
                if (screen.Type == type)
                    screen.Toggle(true);
                else
                    screen.Toggle(false);
            }
        }
        public void Toggle(ScreenType type, bool value)
        {
            foreach (Screen screen in _screens)
            {
                if (screen.Type == type)
                    screen.Toggle(value);
            }
        }
        public Screen GetScreen(ScreenType type)
        {
            return _screens.Where(x => x.Type == type).FirstOrDefault();
        }
    }
}
