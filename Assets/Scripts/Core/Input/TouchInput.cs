using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Input
{

    public class TouchInput : MonoBehaviour, IInput, IPointerDownHandler, IPointerUpHandler
    {
        public bool Pressed { get => _pressed; }
        public float XPosition { get => _xPosition; }

        [SerializeField] private RectTransform _rect;

        private float _xPosition = 0.5f;
        private float _touchOffset;
        private bool _pressed;

        public void OnPointerDown(PointerEventData eventData)
        {
            _pressed = true;
            _touchOffset = XPosition - getTouchPositionOnRect();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
        }

        private void Update()
        {
            if (_pressed)
            {
                _xPosition = getTouchPositionOnRect() + _touchOffset;
            }

            _xPosition = Mathf.Clamp01(XPosition);
        }
        private float getTouchPositionOnRect()
        {
            float rectWidth = (_rect.anchorMax.x - _rect.anchorMin.x) * Screen.width;
            return UnityEngine.Input.mousePosition.x / rectWidth;
        }
    }
}
