using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(menuName = "Money Rush/Core/Level Switch Config")]
    public class LevelSwitchConfig : ScriptableObject
    {
        public IReadOnlyCollection<string> Levels { get => _levels.ToList().AsReadOnly(); }

        [SerializeField] private string[] _levels = new string[0];

        public string GetLevel(int index)
        {
            return _levels[index];
        }
    }
}
