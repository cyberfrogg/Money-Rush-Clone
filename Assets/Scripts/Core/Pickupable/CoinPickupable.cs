using Core.Coins;
using UnityEngine;

namespace Core.Pickupable
{
    public class CoinPickupable : MonoBehaviour, IPickupable
    {
        public bool IsPickedUp { get => _isPickedUp; }

        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private float _positionLerp = 1f;
        [SerializeField] private float _destroyTime = 0.25f;

        private CoinFactory _coinFactory;
        private Pickupables _pickupables;
        private bool _isPickedUp;
        private Vector3 _followOrigin;

        public void Pickup()
        {
            _isPickedUp = true;
            Destroy(gameObject);
            _pickupables.Unregister(this);
        }
        public void Pickup(Vector3 origin)
        {
            _isPickedUp = true;
            Destroy(gameObject, _destroyTime);
            _pickupables.Unregister(this);
            _followOrigin = origin;
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
        private void Update()
        {
            if (!IsPickedUp)
                return;

            transform.position = Vector3.Lerp(transform.position, _followOrigin, _positionLerp * Time.deltaTime);
        }
    }
}
