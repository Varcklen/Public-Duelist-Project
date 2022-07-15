using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.UnitNS;
using System.Threading.Tasks;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName="New PermanentSilence", menuName="Scriptable Objects/Ability/PassiveAbility/PermanentSilence")]
	public class PermanentSilence : PassiveAbilityData
	{
		[SerializeField] private PermanentSilenceInfo _PermanentSilenceInfo;
		public override PropertyInfo Property { get => _PermanentSilenceInfo; }
        public override Type PropertyType => typeof(PermanentSilenceComponent);
    }
}

namespace Project.Abilities
{
    [Serializable]
	public class PermanentSilenceInfo : PropertyInfo
	{
		
    }

	public class PermanentSilenceComponent : PassiveAbilityComponent<PermanentSilenceInfo>, IAbilityUsable
    {
        private bool silenced; 

        protected override bool ExtraUsableConditions()
        {
            return Caster.UnitStats.UnitInfo.Mana.Value <= 3;
        }

        public void OnUsable()
        {
            if (silenced) return;
            silenced = true;
            Caster.UnitStatuses.Silence.Add();
        }

        public void OnUnusable()
        {
            if (silenced == false) return;
            silenced = false;
            Caster.UnitStatuses.Silence.Remove();
        }
    }
}