using UnityEngine;
using TMPro;
using Project.Interfaces;

namespace Project.UI.BattlefieldNS
{
    public class CooldownUI : MonoBehaviour, IShowHide
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = transform.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetCooldown(int cooldown)
        {
            _text.text = cooldown.ToString();
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

