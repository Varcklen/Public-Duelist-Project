using Project.Abilities;
using Project.UI.Cursors;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI.BattlefieldNS
{
    public abstract class ClickableAbilityUI : AbilityUI, IPointerEnterHandler, IPointerExitHandler
    {
        private Button _button;

        private new void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _button = transform.Find("Background").GetComponentInChildren<Button>();
        }

        /*private void OnDisable()
        {
            CursorController.SetCursor(CursorType.Default);
        }*/

        public void SetButtonInteractability(bool isActive)
        {
            _button.interactable = isActive;
            if (isCursorOnButton && !isActive)
            {
                CursorController.SetCursor(CursorType.Default);
            }
        }

        #region OnPointer
        private bool isCursorOnButton;
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_button.interactable == false)
            {
                return;
            }
            isCursorOnButton = true;
            CursorController.SetCursor(CursorType.Pointer);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isCursorOnButton = false;
            CursorController.SetCursor(CursorType.Default);
        }
        #endregion

    }
}
