using Core.Enviroment.Coins;
using Core.Input;
using Core.Pickupable;
using Core.Rails;
using System;
using System.Linq;
using UnityEngine;

namespace Core.PlayerMoneyWad
{
    public class MoneyWad : MonoBehaviour
    {
        public event Action Touched;

        [SerializeField] private AutoMover _autoMover;
        [SerializeField] private CoinsContainer _coinsContainer;
        [Space]
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _finishThreshold = 0.1f;

        private LevelRails _rails;
        private IInput _input;
        private Pickupables _pickupables;
        private bool _isStopped = true;

        public void Initialize(LevelRails rails, IInput input, Pickupables pickuables)
        {
            _rails = rails;
            _input = input;
            _pickupables = pickuables;
            _coinsContainer.Initialize(transform, _rails.Width, _pickupables);
            configureAutoMover();
        }
        public void StartMovement()
        {
            _autoMover.StartMoving();
            _isStopped = false;
        }
        public void StopMovement()
        {
            _autoMover.StopMoving();
            _isStopped = true;
        }

        private void onCoinsEmptied()
        {

        }
        private void configureAutoMover()
        {
            _rails.ApplyAnchorPoints(_autoMover);
            _autoMover.StopAfter = 1;
            _autoMover.RunOnStart = false;
            _autoMover.Length = getTotalDistanceOfRails() / _speed;
        }
        private void Update()
        {
            _coinsContainer.UpdateRows((transform.localPosition.x / _rails.Width) * 2);

            checkFinish();

            if (!_input.Pressed)
                return;

            Touched?.Invoke();
        }
        private void LateUpdate()
        {
            if (_isStopped)
                return;

            checkFinish();

            transform.position = new Vector3(
                getXPosition(),
                transform.position.y,
                transform.position.z
                );
        }
        private float getXPosition()
        {
            float halfWidth = _rails.Width / 2;
            float targetPosition = _input.XPosition * _rails.Width - halfWidth;
            return Mathf.Clamp(targetPosition, -halfWidth, halfWidth);
        }
        private void checkFinish()
        {
            if (transform.position.z + _finishThreshold >= _autoMover.Pos.Last().z)
            {
                _autoMover.StopMoving();
                transform.position = _autoMover.Pos.Last();
            }
        }
        private float getTotalDistanceOfRails()
        {
            float totalDistance = 0;
            for (int i = 0; i < _autoMover.Pos.Count; i++)
            {
                if (i + 1 >= _autoMover.Pos.Count)
                    break;

                totalDistance += Vector3.Distance(_autoMover.Pos[i], _autoMover.Pos[i + 1]);
            }

            return totalDistance;
        }
    }
}
