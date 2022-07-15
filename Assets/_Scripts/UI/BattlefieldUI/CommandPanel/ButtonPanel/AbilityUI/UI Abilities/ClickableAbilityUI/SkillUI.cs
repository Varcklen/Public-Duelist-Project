using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.BattlefieldNS
{
    public class SkillUI : ClickableAbilityUI
    {
        protected override Image Image { get; set; }
        protected override Button Button { get; set; }

        private new void Awake()
        {
            base.Awake();
            Transform background = transform.Find("Background");
            Button = background.GetComponentInChildren<Button>();
            Image = Button.GetComponent<Image>();
        }
    }
}