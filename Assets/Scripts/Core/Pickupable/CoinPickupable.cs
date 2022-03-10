using UnityEngine;

namespace Core.Pickupable
{
    public class CoinPickupable : MonoBehaviour, IPickupable
    {
        public bool IsPickedUp { get => _isPickedUp; }

        private Pickupables _pickupables;
        private bool _isPickedUp;

        public void Pickup()
        {
            _isPickedUp = true;
            Destroy(gameObject);
            _pickupables.Unregister(this);
        }

        private void Awake()
        {
            _pickupables = FindObjectOfType<Pickupables>();
            _pickupables.Register(this);
        }
    }
}
