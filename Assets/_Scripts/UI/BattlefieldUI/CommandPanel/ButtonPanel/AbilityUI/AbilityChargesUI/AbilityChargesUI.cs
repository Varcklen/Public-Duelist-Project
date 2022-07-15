using Project.Interfaces;
using TMPro;
using UnityEngine;

namespace Project.UI.BattlefieldNS
{
    public class AbilityChargesUI : MonoBehaviour, IShowHide
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = transform.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetCharges(int charges)
        {
            _text.text = charges.ToString();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject?.SetActive(false);
        }
    }
}