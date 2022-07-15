using Project.Interfaces;
using UnityEngine;

namespace Project.BattlefieldNS
{
    /// <summary>
    /// Performs actions with Arrow Hint
    /// </summary>
    public class ArrowHint : MonoBehaviour, IShowHide
    {
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}