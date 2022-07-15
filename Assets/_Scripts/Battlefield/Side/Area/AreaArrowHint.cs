using Project.Interfaces;
using UnityEngine;

namespace Project.BattlefieldNS
{
    /// <summary>
    /// This class is responsible for interacting with the Area Arrow Hint.
    /// </summary>
    public class AreaArrowHint : MonoBehaviour
    {
        private IShowHide _arrowHintGameObject;

        private void Awake()
        {
            _arrowHintGameObject = GetComponentInChildren<IShowHide>(includeInactive: true);
        }

        public void EnableArrowHint()
        {
            _arrowHintGameObject.Show();
        }

        public void DisableArrowHint()
        {
            _arrowHintGameObject.Hide();
        }
    }

}
