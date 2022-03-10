using Core.PlayerMoneyWad;
using Core.Rails;
using UnityEngine;

namespace Core
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private PlayerInstaller _playerInstaller;

        private Game _game;

        private void Awake()
        {
            _game = new Game(_playerInstaller.Create());
        }
    }
}
