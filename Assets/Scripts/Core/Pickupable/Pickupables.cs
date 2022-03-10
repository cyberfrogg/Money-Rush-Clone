using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Core.Pickupable
{
    public class Pickupables : MonoBehaviour
    {
        private List<IPickupable> _pickupables = new List<IPickupable>();

        public void Register(IPickupable pickupable)
        {
            if (_pickupables.Contains(pickupable))
                throw new ArgumentException("Item is already registred");

            _pickupables.Add(pickupable);
        }
        public void Unregister(IPickupable pickupable)
        {
            if (!_pickupables.Remove(pickupable))
                throw new ArgumentException("Item is not registred");
        }

        public bool GetInRaduis<T>(Vector3 origin, float radius, out T output)
        {
            IEnumerable<T> targets = _pickupables.OfType<T>();
            if (targets.Count() == 0)
            {
                output = default(T);
                return false;
            }

            IEnumerable<T> raduisTargets = targets.Where(x => Vector3.Distance((x as MonoBehaviour).transform.position, origin) <= radius);
            if (raduisTargets.Count() == 0)
            {
                output = default(T);
                return false;
            }

            output = raduisTargets.First();
            return true;
        }
    }
}
