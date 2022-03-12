using Core.Enviroment.Coins;
using System;
using System.Linq;
using UnityEngine;

namespace Core.Enviroment.Multipliers
{
    public class Side : MonoBehaviour
    {
        public event Action<Side> SideEntered;

        [SerializeField] private MultiplierAction _action;
        [SerializeField] private float _value;
        [SerializeField] private GameObject _gradientObject;

        private Multiplier _multiplier;

        public void Initialize(Multiplier multiplier)
        {
            _multiplier = multiplier;
        }

        public void OnCoinEnter(Coin coin)
        {
            if (_multiplier.Entered)
                return;
            SideEntered?.Invoke(this);

            float newTotalCoins = applyMultplyAction(_action, coin.Container.TotalCoinsSum, _value);

            if (coin.Container.TotalCoinsSum < newTotalCoins)
                addCoins(coin, newTotalCoins);
            else
                removeCoins(coin, newTotalCoins);

            _gradientObject.SetActive(false);
        }
        private void addCoins(Coin coin, float newTotalCoins)
        {
            while (coin.Container.TotalCoinsSum < newTotalCoins)
            {
                float price = coin.Container.CoinPrices.Max(x => x.Price);
                coin.Container.CreateCoin(price);
            }
        }
        private void removeCoins(Coin coin, float newTotalCoins)
        {
            coin.Container.TakeCoin(newTotalCoins);
        }

        //cringe
        private float applyMultplyAction(MultiplierAction action, float initial, float value)
        {
            switch (action)
            {
                case MultiplierAction.Add:
                    return initial + value;
                case MultiplierAction.Minus:
                    return initial - value;
                case MultiplierAction.Multiply:
                    return initial * value;
                case MultiplierAction.Divide:
                    return initial / value;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
