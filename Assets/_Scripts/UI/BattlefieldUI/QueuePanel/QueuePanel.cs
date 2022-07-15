using Project.BattlefieldNS;
using Project.Interfaces;
using Project.Singleton.ConfigurationNS;
using Project.UnitNS;
using Project.Utils.Events;
using Project.Utils.Extension.AnimatorNS;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.UI.BattlefieldNS
{
    public class QueuePanel : MonoBehaviour, IShowHideAsync
    {
        private LimitList<QueueIconUI> _queueIcons;
        private BattlefieldStages _battlefieldStages;
        private Transform _panel;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _battlefieldStages = BattlefieldStages.Instance;
            Subscribe();
        }

        private void Start()
        {
            _panel = transform.GetChild(0);
            _queueIcons = new LimitList<QueueIconUI>(Configuration.Instance.AllSlotsCount);
            var currentUnitIcon = _panel.GetComponentInChildren<QueueIconUI>(includeInactive: true);
            _queueIcons.Add(currentUnitIcon);

            var queue = _panel.Find("Queue");
            int count = queue.childCount;
            for (int i = 0; i < count; i++)
            {
                _queueIcons.Add(queue.GetChild(i).GetComponent<QueueIconUI>());
            }
            DisableAllIcons();
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
            _battlefieldStages.OnActionEnded.AddListener(RefreshIcons);
        }

        private void Unsubscribe()
        {
            if (isSubscribed == false) return;
            isSubscribed = false;
            _battlefieldStages.OnActionEnded.RemoveListener(RefreshIcons);
        }
        #endregion

        private void DisableAllIcons()
        {
            foreach (var icon in _queueIcons)
            {
                icon.Hide();
            }
        }

        private void RefreshIcons()
        {
            var units = BattlefieldStages.Instance.AllUnitsInQueue;
            for (int i = 0; i < _queueIcons.Count; i++)
            {
                if (i >= units.Count)
                {
                    _queueIcons[i].Hide();
                }
                else
                {
                    _queueIcons[i].Show();
                    _queueIcons[i].SetUnit(units[i]);
                }
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

