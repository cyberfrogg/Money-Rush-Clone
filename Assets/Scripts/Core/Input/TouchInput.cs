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
                return 0;
            }
        }
    }
}
