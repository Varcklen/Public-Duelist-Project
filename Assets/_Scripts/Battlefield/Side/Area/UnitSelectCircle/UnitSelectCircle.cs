using Project.Interfaces;
using UnityEngine;

namespace Project.BattlefieldNS
{
    /// The component is responsible for the circle of the active unit at the bottom of the sprite.
    /// </summary>
    public class UnitSelectCircle : MonoBehaviour, IUnitSelectCircle, IShowHide
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        #region TargectCircle Cast
        private bool WasActive;
        void IUnitSelectCircle.EnableSelectCircle()
        {
            if (WasActive)
            {
                Show();
            }
            WasActive = false;
        }
        void IUnitSelectCircle.DisableSelectCircle()
        {
            if (isActiveAndEnabled)
            {
                WasActive = true;
                Hide();
            }
        }
        #endregion
    }

    /// <summary>
    /// Called on the Target Circle to disable or enable the game object based on its past activity.
    /// </summary>
    public interface IUnitSelectCircle
    {
        void EnableSelectCircle();
        void DisableSelectCircle();
    }
}