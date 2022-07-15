using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.UnitNS;
using System.Threading.Tasks;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName="New NoManaOnStart", menuName="Scriptable Objects/Ability/PassiveAbility/NoManaOnStart")]
	public class NoManaOnStart : PassiveAbilityData
	{
		[SerializeField] private NoManaOnStartInfo _NoManaOnStartInfo;
		public override PropertyInfo Property { get => _NoManaOnStartInfo; }
        public override Type PropertyType => typeof(NoManaOnStartComponent);
    }
}

namespace Project.Abilities
{
    [Serializable]
	public class NoManaOnStartInfo : PropertyInfo
	{
		
    }

	public class NoManaOnStartComponent : PassiveAbilityComponent<NoManaOnStartInfo>, IAbilityBattleStart
	{
        public void BattleStart()
        {
			var rand = new System.Random();
			Caster.UnitStats.UnitInfo.Health.SetPercentResource(rand.Next(20, 80));
			Caster.UnitStats.UnitInfo.Mana.SetResource(0);
        }
    }
}