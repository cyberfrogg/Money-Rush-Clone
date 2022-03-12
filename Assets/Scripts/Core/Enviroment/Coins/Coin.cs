using Core.Enviroment.Multipliers;
using Core.Pickupable;
using System;
using UnityEngine;

namespace Core.Enviroment.Coins
{
    public class Coin : MonoBehaviour
    {
        public event Action<Coin, Coin> OnOtherCoinEntered;
        public CoinsContainer Container { get => _container; }
        public CoinPrice Price { get => _price; }

        [SerializeField] private CoinPrice _price;
        [SerializeField] private Transform _model;
        [SerializeField] private float _modelSpinSpeed;
        [SerializeField] private float _pickupRadius = 1f;
        [Space]
        [SerializeField] private GameObject _destroyParticlesPrefab;

        private CoinsContainer _container;
        private Pickupables _pickupables;

        public void InitializeInContainer(CoinsContainer container, Pickupables pickupables)
        {
            _container = container;
            _pickupables = pickupables;
        }
        public void Destroy()
        {
            Destroy(gameObject);
            createDestroyParticles();
        }

        private void Update()
        {
            _model.Rotate(1 * _modelSpinSpeed * Time.deltaTime, 0, 0);

            CoinPickupable pickupedCoin;
            if (_pickupables.GetInRaduis(transform.position, _pickupRadius, out pickupedCoin))
            {
                pickupedCoin.Pickup(transform);
                Coin coin = pickupedCoin.GetCoin();
                _container.AddCoin(coin);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            Side side;
            if (other.gameObject.TryGetComponent(out side))
            {
                side.OnCoinEnter(this);
            }
        }
        private void createDestroyParticles()
        {
            GameObject instance = Instantiate(_destroyParticlesPrefab);
            instance.transform.position = transform.position;
            Destroy(instance, 10f);
        }
    }
}