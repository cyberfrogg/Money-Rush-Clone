using System;
using UnityEngine;

namespace Core.Enviroment.Coins
{
    public class Cell : MonoBehaviour
    {
        public bool IsBusy { get => _busyCoin != null; }
        public Coin Coin { get => _busyCoin; }

        private Coin _busyCoin;

        public void Initialize()
        {
            if (transform.childCount == 0)
                return;

            Coin childCoin = null;
            if (transform.GetChild(0).gameObject.TryGetComponent(out childCoin))
            {
                Attach(childCoin);
            }
        }
        public void Attach(Coin coin)
        {
            _busyCoin = coin;
            _busyCoin.transform.position = transform.position;
            _busyCoin.transform.SetParent(transform);
        }
        public Coin Detach()
        {
            _busyCoin.transform.position = transform.position;
            _busyCoin.transform.SetParent(null);
            Coin returnCoin = _busyCoin;
            _busyCoin = null;
            return returnCoin;
        }
    }
}