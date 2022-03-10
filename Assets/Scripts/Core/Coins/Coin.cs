using UnityEngine;

namespace Core.Coins
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Transform _model;
        [SerializeField] private float _modelSpinSpeed;

        private void Update()
        {
            _model.Rotate(1 * _modelSpinSpeed * Time.deltaTime, 0, 0);
        }
    }
}