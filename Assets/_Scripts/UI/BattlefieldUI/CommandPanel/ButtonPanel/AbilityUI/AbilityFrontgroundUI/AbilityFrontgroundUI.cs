using Project.Abilities;
using Project.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.BattlefieldNS
{
    public class AbilityFrontgroundUI : MonoBehaviour, IShowHide
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void SetFrontgroundColor(AbilityBan frontGroundBan)
        {
            Color _color;

            //They must be in this order.
            switch (frontGroundBan)
            {
                case AbilityBan.None:
                    Debug.LogWarning($"Current ban is not setted: {frontGroundBan}. Setted default color: grey.");
                    _color = Color.gray;
                    break;
                case AbilityBan.OnCooldown:
                    _color = Color.grey;
                    break;
                case AbilityBan.NotEnoughResource:
                    _color = Color.blue;
                    break;
                case AbilityBan.Silenced:
                    _color = Color.magenta;
                    break;
                case AbilityBan.NotPlayable:
                    _color = Color.grey;
                    break;
                default: throw new Exception("This parameter must not be None.");
            }
            _color.a = _image.color.a;
            _image.color = _color;
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


