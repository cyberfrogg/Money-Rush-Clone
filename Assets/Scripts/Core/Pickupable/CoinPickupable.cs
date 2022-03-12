using Core.Enviroment.Coins;
using UnityEngine;

namespace Core.Pickupable
{
    public class CoinPickupable : MonoBehaviour, IPickupable
    {
        public bool IsPickedUp { get => _isPickedUp; }

        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private float _positionLerp = 10f;
        [SerializeField] private float _rotationLerp = 10f;
        [SerializeField] private float _destroyTime = 0.25f;
        [SerializeField] private AudioSource _pickSourcePrefab;

        private CoinFactory _coinFactory;
        private Pickupables _pickupables;
        private bool _isPickedUp;
        private Transform _followOrigin;

        public void Pickup()
        {
            _isPickedUp = true;
            Destroy(gameObject);
            _pickupables.Unregister(this);
        }
        public void Pickup(Transform origin)
        {
            _isPickedUp = true;
            Destroy(gameObject, _destroyTime);
            playSound();
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

            transform.position = Vector3.Lerp(transform.position, _followOrigin.position, _positionLerp * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _followOrigin.rotation, _rotationLerp * Time.deltaTime);
        }
        private void playSound()
        {
            AudioSource s = Instantiate(_pickSourcePrefab);
            s.Play();
            Destroy(s.gameObject, 5);
        }
    }
}
