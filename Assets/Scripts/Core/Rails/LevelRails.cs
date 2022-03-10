using System;
using UnityEngine;

namespace Core.Rails
{
    public class LevelRails : MonoBehaviour
    {
        public float Width { get => _width; }
        [SerializeField] private Transform[] _anchors;
        [SerializeField] private float _width = 4f;

        public void ApplyAnchorPoints(AutoMover autoMover)
        {
            foreach (Transform point in _anchors)
            {
                autoMover.AddAnchorPoint(point.position, point.rotation.eulerAngles);
            }
        }
    }
}
