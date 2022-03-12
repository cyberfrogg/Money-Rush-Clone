using Core.Enviroment.Coins;
using UnityEngine;

namespace Core.Enviroment.FinishScoreCounting
{
    public class Row : MonoBehaviour
    {
        public int ScoreNumber { get => _scoreNumber; }
        public bool Selected { get => _selected; }
        public float Length { get => _length; }

        [SerializeField] private float _length = 2;
        [SerializeField] private Cell[] _cells;

        private Coin _firstCoin;
        private int _scoreNumber;
        private bool _selected;

        public void Initialize(Coin firstCoin, int scoreNumber)
        {
            _firstCoin = firstCoin;
            _scoreNumber = scoreNumber;

            foreach (Cell cell in _cells)
            {
                cell.Initialize(_scoreNumber);
            }
        }
        private void Update()
        {
            if (_firstCoin == null)
                return;

            if (_selected)
                return;

            if (_firstCoin.transform.position.z >= transform.position.z)
            {
                _selected = true;

                foreach (Cell cell in _cells)
                {
                    cell.Select();
                }
            }
        }
    }
}
