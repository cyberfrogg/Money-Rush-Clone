using System;
using TMPro;
using UnityEngine;

namespace Core.Enviroment.FinishScoreCounting
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Cell : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private Material _selectMaterial;

        private MeshRenderer _meshRenderer;
        private Material _defaultMaterial;
        private int _scoreNumber;

        public void Initialize(int scoreNumber)
        {
            _scoreNumber = scoreNumber;
            _countText.text = $"{(_scoreNumber + 1) * 100}";
        }
        public void Select()
        {
            _defaultMaterial = _meshRenderer.material;
            _meshRenderer.material = _selectMaterial;
        }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}