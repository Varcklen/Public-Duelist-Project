using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.BattlefieldNS
{
    public class PassiveAbilityUI : AbilityUI
    {
        protected override Image Image { get; set; }
        //Always null
        protected override Button Button { get; set; }

        private new void Awake()
        {
            base.Awake();
            Image = transform.Find("Background").GetComponentInChildren<Image>();
        }
    }
}