﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enviroment.Coins
{
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
