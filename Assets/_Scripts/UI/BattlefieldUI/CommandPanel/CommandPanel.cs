using Project.BattlefieldNS;
using Project.Interfaces;
using Project.UnitNS;
using Project.Utils.Events;
using Project.Utils.Extension.AnimatorNS;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.UI.BattlefieldNS
{
    public class CommandPanel : MonoBehaviour, IShowHideAsync
    {
        private ButtonPanel _buttonPanel;
        private Animator _animator;
        private BattlefieldStages _battlefieldStages;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _battlefieldStages = BattlefieldStages.Instance;
            var panel = GetComponentInChildren<Transform>();
            _buttonPanel = panel.GetComponentInChildren<ButtonPanel>(includeInactive:true);
            Subscribe();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        #region Subscribes
        private bool isSubscribed;
        private void Subscribe()
        {
            if (isSubscribed) return;
            isSubscribed = true;
            _battlefieldStages.OnActionEnded.AddListener(Check);
            _battlefieldStages.OnRoundChanged.AddListener(HideButtonPanel);
        }

        private void Unsubscribe()
        {
            if (isSubscribed == false) return;
            isSubscribed = false;
            _battlefieldStages.OnActionEnded.RemoveListener(Check);
            _battlefieldStages.OnRoundChanged.RemoveListener(HideButtonPanel);
        }
        #endregion

        private void HideButtonPanel(int round)
        {
            _buttonPanel.Hide();
        }

        private void Check()
        {
            var unit = _battlefieldStages.CurrentTurnUnit;
            if (unit?.GetSideType() == SideType.Ally)
            {
                _buttonPanel.Show();
                _buttonPanel.SetUnitAbilities(unit);
            }
            else
            {
                _buttonPanel.Hide();
            }
        }

        public Task Show()
        {
            gameObject.SetActive(true);
            int animationDuration = Mathf.CeilToInt(_animator.GetCurrentAnimationLength());
            return Task.Delay((animationDuration * 1000));
        }

        public async Task Hide()
        {
            _animator.Play("Hide");
            int animationDuration = Mathf.CeilToInt(_animator.GetCurrentAnimationLength());
            await Task.Delay(animationDuration * 1000);
            gameObject.SetActive(false);
        }
    }
}
