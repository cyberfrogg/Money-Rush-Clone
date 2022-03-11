using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Coins
{
    public class CoinsContainer : MonoBehaviour
    {
        [SerializeField] private Cell[] _cells;

        private IReadOnlyCollection<Row> _rows;
        private Transform _origin;

        public void Install(Transform origin)
        {
            _origin = origin;
            _rows = getRows();
        }
        public void AddCoin(Coin coin)
        {
            Cell nearestCell = getFirstFreeCell();
            nearestCell.Attach(coin);
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
                rows.Add(new Row(rowCells, pos));
            }

            return rows.AsReadOnly();
        }
    }

    public class Row
    {
        private IEnumerable<Cell> _cells;
        private float _xPosition;

        public Row(IEnumerable<Cell> cells, float xPosition)
        {
            _cells = cells;
            _xPosition = xPosition;
        }
    }
}