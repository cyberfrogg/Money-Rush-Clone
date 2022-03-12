using System;
using UnityEngine;

namespace Core.Enviroment.Multipliers
{
    public class Multiplier : MonoBehaviour
    {
        public bool Entered { get => _entered; }

        [SerializeField] private Side[] _sides;

        private bool _entered;

        private void Awake()
        {
            foreach (Side side in _sides)
            {
                side.Initialize(this);
                side.SideEntered += onSideEntered;
            }
        }
        private void OnDestroy()
        {
            foreach (Side side in _sides)
            {
                side.SideEntered -= onSideEntered;
            }
        }
        private void onSideEntered(Side side)
        {
            _entered = true;
        }
    }
}