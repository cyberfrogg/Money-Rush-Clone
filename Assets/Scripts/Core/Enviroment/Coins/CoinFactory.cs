using UnityEngine;

namespace Core.Enviroment.Coins
{
    public class CoinFactory
    {
        private Coin _prefab;

        public CoinFactory(Coin prefab)
        {
            _prefab = prefab;
        }

        public Coin Create()
        {
            Coin instance = GameObject.Instantiate(_prefab);
            return instance;
        }
    }
}
