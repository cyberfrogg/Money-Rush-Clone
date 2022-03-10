using UnityEngine;

namespace Core.Input
{

    public class TouchInput : MonoBehaviour, IInput
    {
        public bool Pressed { get => UnityEngine.Input.GetMouseButtonDown(0); }

        public float XPosition
        {
            get
            {
                float touchPos = UnityEngine.Input.mousePosition.x;
                float x = touchPos / Screen.width;
                return x;
            }
        }
    }
}
