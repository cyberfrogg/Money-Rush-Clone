using Cinemachine;
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
        public event Action CoinsEmptied;
        public event Action Touched;
        public event Action<float> MoneyCountChanged;
        public event Action Finished;

        public CoinsContainer CoinsContainer { get => _coinsContainer; }

        [SerializeField] private AutoMover _autoMover;
        [SerializeField] private CoinsContainer _coinsContainer;
        [SerializeField] private MoneyWadCountDisplay _countDisplay;
        [SerializeField] private CinemachineVirtualCamera _cineCamera;
        [Space]
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _finishThreshold = 0.1f;

        private LevelRails _rails;
        private IInput _input;
        private Pickupables _pickupables;
        private Vector3 _stopPosition;
        private bool _isStopped = true;
        private bool _isFinished = false;

        public void Initialize(LevelRails rails, IInput input, Pickupables pickuables)
        {
            _rails = rails;
            _input = input;
            _pickupables = pickuables;

            _coinsContainer.CoinsEmptied += onCoinsEmptied;
            _coinsContainer.CountChanged += onMoneyCountChanged;
            _countDisplay.Initialize(this);
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
            if (_autoMover != null)
                _autoMover.StopMoving();

            _isStopped = true;
        }
        public void FollowCamera(Transform target)
        {
            _cineCamera.Follow = target;
            _cineCamera.LookAt = target;
        }
        public void FollowCamera(Transform target, Vector3 newPosition)
        {
            _cineCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = newPosition;
            _cineCamera.Follow = target;
            _cineCamera.LookAt = target;
        }

        private void Update()
        {
            _coinsContainer.UpdateRows((transform.localPosition.x / _rails.Width) * 2);

            if (!_input.Pressed)
                return;

            Touched?.Invoke();
        }
        private void LateUpdate()
        {
            if (_isStopped)
                transform.position = _stopPosition;

            if (_isStopped)
                return;

            if (!_isFinished)
                checkFinish();

            transform.position = new Vector3(
                getXPosition(),
                transform.position.y,
                transform.position.z
                );
            _stopPosition = transform.position;
        }
        private void onMoneyCountChanged(float value)
        {
            MoneyCountChanged?.Invoke(value);
        }
        private void configureAutoMover()
        {
            _rails.ApplyAnchorPoints(_autoMover);
            _autoMover.StopAfter = 1;
            _autoMover.RunOnStart = false;
            _autoMover.Length = getTotalDistanceOfRails() / _speed;
        }
        private void onCoinsEmptied()
        {
            CoinsEmptied?.Invoke();
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
                _isFinished = true;
                Finished?.Invoke();
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
        private void OnDestroy()
        {
            _coinsContainer.CoinsEmptied -= onCoinsEmptied;
            _coinsContainer.CountChanged -= onMoneyCountChanged;
        }
    }
}
