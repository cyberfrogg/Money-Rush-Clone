using System;
using UnityEngine;

namespace Core.Enviroment.Coins
{
    [CreateAssetMenu(menuName = "Money Rush/Core/Coin Price")]
    public class CoinPrice : ScriptableObject
    {
        public float Price { get => _price; }
        public Coin Prefab { get => _prefab; }

        [SerializeField] private float _price;
        [SerializeField] private Coin _prefab;
    }
}
