using System;
using UnityEngine;

namespace Core.UI.ScreenSystem
{
    public class Screen : MonoBehaviour
    {
        public ScreenType Type { get => _type; }

        [SerializeField] private ScreenType _type;

        public void Toggle(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
