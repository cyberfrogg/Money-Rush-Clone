using Core.Coins;
using UnityEngine;

namespace Core.Pickupable
{
    public class CoinPickupable : MonoBehaviour, IPickupable
    {
        public bool IsPickedUp { get => _isPickedUp; }

        [SerializeField] private Coin _coinPrefab;

        private CoinFactory _coinFactory;
        private Pickupables _pickupables;
        private bool _isPickedUp;

        public void Pickup()
        {
            _isPickedUp = true;
            Destroy(gameObject);
            _pickupables.Unregister(this);
        }
        public Coin GetCoin()
        {
            return _coinFactory.Create();
        }

        private void Awake()
        {
            _coinFactory = new CoinFactory(_coinPrefab);

            _pickupables = FindObjectOfType<Pickupables>();
            _pickupables.Register(this);
        }
    }
}
