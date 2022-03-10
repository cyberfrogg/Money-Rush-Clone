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
        [SerializeField, Range(0, 1f)] private float _finishDistanceThreshold;

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
            _autoMover.RunOnStart = false;
            _autoMover.Length = getTotalDistanceOfRails() / _speed;
        }
        private void Update()
        {
            checkFinish();

            if (!_input.Pressed)
                return;

            Touched?.Invoke();

            transform.position = new Vector3(
                0,
                transform.position.y,
                transform.position.z
                );
        }
        private void checkFinish()
        {
            float distanceToFinish = Vector3.Distance(transform.position, _autoMover.Pos.Last());
            if (distanceToFinish <= _finishDistanceThreshold)
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
