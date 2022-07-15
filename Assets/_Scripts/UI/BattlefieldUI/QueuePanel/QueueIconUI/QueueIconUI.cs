using Project.BattlefieldNS;
using Project.Interfaces;
using Project.UnitNS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI.BattlefieldNS
{
    public class QueueIconUI : MonoBehaviour, IShowHide, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _image;
        private Unit _unit;
        private AreaArrowHint _unitAreaArrowHint;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void SetUnit(Unit unit)
        {
            _unit = unit;
            _image.sprite = unit.UnitStats.UnitInfo.Icon.Sprite;
            _unitAreaArrowHint = unit.Area.AreaArrowHint;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_unitAreaArrowHint == null)
            {
                Debug.LogWarning("You're trying to enable arrow hint in icon, where _unitAreaArrowHint is null.");
                return;
            }
            _unitAreaArrowHint.EnableArrowHint();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_unitAreaArrowHint == null)
            {
                Debug.LogWarning("You're trying to disable arrow hint in icon, where _unitAreaArrowHint is null.");
                return;
            }
            _unitAreaArrowHint.DisableArrowHint();
        }
    }
}