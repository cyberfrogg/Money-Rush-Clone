using Core.Pickupable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enviroment.Coins
{
    public class CoinsContainer : MonoBehaviour
    {
        public event Action CoinsEmptied;
        public CoinPrice[] CoinPrices { get => _coinPrices; }
        public float TotalCoinsSum
        {
            get
            {
                float r = 0;

                foreach (Cell cell in _cells)
                {
                    if (!cell.IsBusy)
                        continue;

                    r += cell.Coin.Price.Price;
                }

                return r;
            }
        }

        [SerializeField] private CoinPrice[] _coinPrices;
        [SerializeField] private Cell[] _cells;
        [SerializeField] private AnimationCurve _movementCurve;

        private IReadOnlyCollection<Row> _rows;
        private Transform _origin;
        private float _railsWidth;
        private Pickupables _pickupables;

        public void Initialize(Transform origin, float railsWidth, Pickupables pickupables)
        {
            _origin = origin;
            _railsWidth = railsWidth;
            _pickupables = pickupables;
            _rows = getRows();
            foreach (Cell cell in _cells)
            {
                cell.Initialize();
                if (cell.IsBusy)
                {
                    cell.Coin.InitializeInContainer(this, _pickupables);
                }
            }
        }
        public void AddCoin(Coin coin)
        {
            Cell nearestCell = getFirstFreeCell();
            nearestCell.Attach(coin);
            coin.InitializeInContainer(this, _pickupables);
        }
        public bool CreateCoin(float price)
        {
            IEnumerable<CoinPrice> prices = _coinPrices.Where(x => x.Price == price);
            if (prices.Count() == 0)
                return false;

            CoinFactory factory = new CoinFactory(prices.First().Prefab);
            Coin coin = factory.Create();
            AddCoin(coin);
            return true;
        }
        public bool TakeCoin(float newTotalCoins)
        {
            if (newTotalCoins < 0)
            {
                CoinsEmptied?.Invoke();
                return false;
            }

            while (TotalCoinsSum > newTotalCoins)
            {
                IEnumerable<Cell> avaliableCells = _cells.Where(x => x.IsBusy);
                if (avaliableCells.Count() == 0)
                    break;

                Coin detachedCoin = avaliableCells.First().Detach();
                detachedCoin.Destroy();
            }

            return true;
        }
        public void UpdateRows(float normalizedCenterXPosition)
        {
            for (int i = 0; i < _rows.Count; i++)
            {
                _rows.ElementAt(i).Update(normalizedCenterXPosition);
            }
        }

        private Cell getFirstFreeCell()
        {
            return _cells
                .Where(x => x.IsBusy == false)
                .OrderBy(x => Vector3.Distance(_origin.position, x.transform.localPosition))
                .FirstOrDefault();
        }
        private IReadOnlyCollection<Row> getRows()
        {
            IEnumerable<Cell> cellRows = _cells.OrderBy(x => x.transform.localPosition.x);
            IEnumerable<float> xPositions = cellRows.Select(x => x.transform.localPosition.x).Distinct();

            List<Row> rows = new List<Row>();
            foreach (float pos in xPositions)
            {
                IEnumerable<Cell> rowCells = cellRows.Where(x => x.transform.localPosition.x == pos);
                rows.Add(new Row(rowCells, pos, _movementCurve, _railsWidth));
            }

            return rows.AsReadOnly();
        }
    }

    public class Row
    {
        private Cell[] _cells;
        private float _xPosition;
        private AnimationCurve _movementCruve;
        private float _cornerGap;
        private float _railsWidth;

        public Row(IEnumerable<Cell> cells, float xPosition, AnimationCurve movementCurve, float railsWidth)
        {
            _cells = cells.ToArray();
            _xPosition = xPosition;
            _movementCruve = movementCurve;
            _railsWidth = railsWidth;
        }
        public void Update(float normalizedCenterXPosition)
        {
            foreach (Cell cell in _cells)
            {
                moveCell(cell, normalizedCenterXPosition);
            }
        }
        private void moveCell(Cell cell, float normalizedCenterXPosition)
        {
            float inverted = _movementCruve.Evaluate(normalizedCenterXPosition);
            cell.transform.localPosition = new Vector3(
                _xPosition * inverted,
                cell.transform.localPosition.y,
                cell.transform.localPosition.z
                );
        }
    }
}