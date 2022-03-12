using Core.PlayerMoneyWad;
using Core.Rails;
using Core.UI.ScreenSystem;
using UnityEngine;

namespace Core
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private PlayerInstaller _playerInstaller;
        [SerializeField] private ScreensInstaller _screensInstaller;

        private Game _game;

        private void Awake()
        {
            _game = new Game(_playerInstaller.Create(), _screensInstaller.CreateScreens());
        }
    }
}
