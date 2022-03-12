using TMPro;
using UnityEngine;

namespace Core.PlayerMoneyWad
{
    public class MoneyWadCountDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private MoneyWad _moneyWad;

        public void Initialize(MoneyWad moneyWad)
        {
            _moneyWad = moneyWad;
            _moneyWad.MoneyCountChanged += onMoneyCountChanged;
        }

        private void onMoneyCountChanged(float count)
        {
            _text.text = $"{count.ToString("0.00")}$";
        }
        private void OnDestroy()
        {
            _moneyWad.MoneyCountChanged -= onMoneyCountChanged;
        }
    }
}
