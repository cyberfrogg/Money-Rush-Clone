using Core.Enviroment.Coins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Enviroment.FinishScoreCounting
{
    public class RowFactory
    {
        private Row _prefab;
        private Transform _parent;
        private Coin _firstCoin;
        private int _count;

        public RowFactory(Row prefab, Transform parent, Coin firstCoin, int count)
        {
            _prefab = prefab;
            _parent = parent;
            _firstCoin = firstCoin;
            _count = count;
        }

        public IReadOnlyCollection<Row> Create()
        {
            List<Row> rows = new List<Row>();

            for (int i = 0; i < _count; i++)
            {
                Row newRow = instantiate(i * _prefab.Length);
                newRow.Initialize(_firstCoin, i);
                rows.Add(newRow);
            }

            return rows.AsReadOnly();
        }
        private Row instantiate(float zPosition)
        {
            Row instance = GameObject.Instantiate(_prefab);
            instance.transform.SetParent(_parent);
            instance.transform.localPosition = new Vector3(0, 0, zPosition);
            return instance;
        }
    }
}
