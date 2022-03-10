using System;
using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(menuName = "Money Rush/Core/Level Switch Config")]
    public class LevelSwitchConfig : ScriptableObject
    {
        [SerializeField] private string[] _levels = new string[0];

        public string GetLevel(int index)
        {
            return _levels[index];
        }
    }
}
