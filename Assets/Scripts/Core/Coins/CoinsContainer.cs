using System.Linq;
using UnityEngine;

namespace Core.Coins
{
    public class CoinsContainer : MonoBehaviour
    {
        [SerializeField] private Cell[] _cells;

        private Transform _origin;

        public void Install(Transform origin)
        {
            _origin = origin;
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
                .OrderBy(x => Vector3.Distance(_origin.position, x.transform.position))
                .FirstOrDefault();
        }
    }
}