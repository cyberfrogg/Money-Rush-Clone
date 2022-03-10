using Core.Level;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuInstaller : MonoBehaviour
    {
        [SerializeField] private LevelSwitchConfig _levelSwitchConfig;
        [SerializeField] private Button _playButton;

        private MainMenuScreen _screen;

        private void Awake()
        {
            _screen = new MainMenuScreen(new LevelSwitch(_levelSwitchConfig), _playButton);
            _screen.Start();
        }
    }
}