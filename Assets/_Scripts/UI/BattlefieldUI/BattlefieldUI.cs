using System.Collections.Generic;
using Project.Singleton.MonoBehaviourSingleton;
using System.Threading.Tasks;
using Project.Utils.Events;
using Project.BattlefieldNS;

namespace Project.UI.BattlefieldNS
{
    public class BattlefieldUI : MonoBehaviourSingleton<BattlefieldUI>
    {
        private CommandPanel _commandPanel;
        private QueuePanel _queuePanel;
        private RoundPanel _roundPanel;
        public RoundPanel RoundPanel => _roundPanel;
        private new void Awake()
        {
            base.Awake();
            _commandPanel = GetComponentInChildren<CommandPanel>(includeInactive: true);
            _queuePanel = GetComponentInChildren<QueuePanel>(includeInactive: true);
            _roundPanel = GetComponentInChildren<RoundPanel>(includeInactive: true);
        }

        private void OnEnable()
        {
            Events.OnBattleEnd.AddListener(HideUIElementsAsync);
        }

        private void OnDisable()
        {
            Events.OnBattleEnd.RemoveListener(HideUIElementsAsync);
        }

        public async Task ShowUIElementsAsync()
        {
            var tasks = new List<Task>();
            tasks.Add(_commandPanel.Show());
            tasks.Add(_queuePanel.Show());

            await Task.WhenAll(tasks);
        }

        public async void HideUIElementsAsync(SideType sideType)
        {
            var tasks = new List<Task>();
            tasks.Add(_commandPanel.Hide());
            tasks.Add(_queuePanel.Hide());

            await Task.WhenAll(tasks);
        }
    }
}

