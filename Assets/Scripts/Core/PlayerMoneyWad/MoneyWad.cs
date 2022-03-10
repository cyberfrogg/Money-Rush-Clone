using Core.Input;
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
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _finishThreshold = 0.1f;

        private LevelRails _rails;
        private IInput _input;

        public void Install(LevelRails rails, IInput input)
        {
            _rails = rails;
            _input = input;
            configureAutoMover();
        }
        public void StartMovement()
        {
            _autoMover.StartMoving();
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
            checkFinish();

            if (!_input.Pressed)
                return;

            Touched?.Invoke();
        }
        private void LateUpdate()
        {
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
