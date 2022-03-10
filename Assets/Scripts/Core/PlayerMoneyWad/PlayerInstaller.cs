using Core.Input;
using Core.Pickupable;
using Core.Rails;
using UnityEngine;

namespace Core.PlayerMoneyWad
{
    public class PlayerInstaller : MonoBehaviour
    {
        [SerializeField] private MoneyWad _prefab;
        [SerializeField] private TouchInput _input;
        [SerializeField] private LevelRails _rails;
        [SerializeField] private Pickupables _pickupables;

        public MoneyWad Create()
        {
            MoneyWad instance = Instantiate(_prefab);
            instance.transform.position = Vector3.zero;

            instance.Install(_rails, _input, _pickupables);

            return instance;
        }
    }
}
