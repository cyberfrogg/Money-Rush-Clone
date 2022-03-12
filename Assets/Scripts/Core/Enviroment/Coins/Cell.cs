using System;
using UnityEngine;

namespace Core.Enviroment.Coins
{
    public class Cell : MonoBehaviour
    {
        public bool IsBusy { get => _busyCoin != null; }

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
        public void Detach()
        {
            _busyCoin.transform.position = transform.position;
            _busyCoin.transform.SetParent(null);
            _busyCoin = null;
        }
    }
}