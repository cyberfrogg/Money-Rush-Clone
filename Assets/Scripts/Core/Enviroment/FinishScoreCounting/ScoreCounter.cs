using Core.Enviroment.Coins;
using Core.PlayerMoneyWad;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enviroment.FinishScoreCounting
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private Row _rowPrefab;
        [SerializeField] private int _rowsCount = 100;
        [SerializeField] private float _yOffset = 0.5f;
        [SerializeField] private float _movementSpeed = 10f;
        [Space]
        [SerializeField] private Vector3 _cameraPosition;

        private MoneyWad _playerMoneyWad;

        public void CountScore(MoneyWad wad)
        {
            _playerMoneyWad = wad;
            IReadOnlyCollection<Coin> coins = _playerMoneyWad.CoinsContainer.DetachAllCoins();

            float initZPosition = transform.position.z;
            for (int i = 0; i < coins.Count; i++)
            {
                Coin coin = coins.ElementAt(i);
                coin.transform.position = new Vector3(0, _yOffset, initZPosition - i);
                coin.StopSpinning();
            }

            IEnumerable<Coin> orderedCoins = coins.OrderBy(x => x.transform.position.z);

            _playerMoneyWad.FollowCamera(orderedCoins.Last().transform, _cameraPosition);
            spawnRows(orderedCoins.Last());

            StartCoroutine(moveCoins(orderedCoins));
        }
        private IEnumerator moveCoins(IEnumerable<Coin> coins)
        {
            bool isAnimationPlaying = true;
            while (isAnimationPlaying)
            {
                foreach (Coin coin in coins)
                {
                    coin.transform.Translate(0, 0, _movementSpeed * Time.deltaTime);
                }
                yield return new WaitForEndOfFrame();

                if (coins.First().transform.position.z > transform.position.z)
                {
                    isAnimationPlaying = false;
                }
            }
        }

        private void spawnRows(Coin firstCoin)
        {
            RowFactory factory = new RowFactory(_rowPrefab, transform, firstCoin, _rowsCount);
            factory.Create();
        }
    }
}