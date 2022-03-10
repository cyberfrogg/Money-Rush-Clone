using System;
using UnityEngine;

namespace Core.Pickupable
{
    public interface IPickupable
    {
        public bool IsPickedUp { get; }
        public void Pickup();
    }
}
