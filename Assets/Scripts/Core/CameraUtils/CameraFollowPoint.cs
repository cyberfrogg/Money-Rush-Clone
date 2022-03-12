using UnityEngine;
using System;
using Cinemachine;

namespace Core.CameraUtils
{
    public class CameraFollowPoint : MonoBehaviour
    {
        [SerializeField] private float _positionLerp = 10f;
        [SerializeField] private float _offsetLerp = 10f;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        private CinemachineTransposer _transposer;

        private Transform _currentFollow;
        private Vector3 _currentFollowOffset;


        private void Awake()
        {
            transform.SetParent(null);
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _currentFollowOffset = _transposer.m_FollowOffset;
        }

        public void Follow(Transform follow)
        {
            _currentFollow = follow;
        }
        public void Follow(Transform follow, Vector3 followOffset)
        {
            _currentFollow = follow;
            _currentFollowOffset = followOffset;
        }

        private void Update()
        {
            if (_currentFollow == null)
                return;

            transform.position = Vector3.Lerp(transform.position, _currentFollow.position, _positionLerp * Time.deltaTime);
            _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset, _currentFollowOffset, _offsetLerp * Time.deltaTime);
        }
    }
}
