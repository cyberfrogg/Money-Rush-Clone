using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Coins
{
    public class CoinsContainer : MonoBehaviour
    {
        [SerializeField] private Cell[] _cells;
        [SerializeField] private float _rowsGap = 0.25f;
        [SerializeField] private AnimationCurve _movementCurve;

        private IReadOnlyCollection<Row> _rows;
        private Transform _origin;

        public void Initialize(Transform origin)
        {
            _origin = origin;
            _rows = getRows();
        }
        public void AddCoin(Coin coin)
        {
            Cell nearestCell = getFirstFreeCell();
            nearestCell.Attach(coin);
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
                rows.Add(new Row(rowCells, pos, _rowsGap, _movementCurve));
            }

            return rows.AsReadOnly();
        }
    }

    public class Row
    {
        private Cell[] _cells;
        private float _xPosition;
        private float _gap;
        private AnimationCurve _movementCruve;

        public Row(IEnumerable<Cell> cells, float xPosition, float gap, AnimationCurve movementCurve)
        {
            _cells = cells.ToArray();
            _xPosition = xPosition;
            _gap = gap;
            _movementCruve = movementCurve;
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

            cell.transform.localPosition = new Vector3(_xPosition * inverted, cell.transform.localPosition.y, cell.transform.localPosition.z);
        }
    }
}