using UnityEngine;
using System;
using Project.UnitNS.Interfaces;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName="New ExtraActions", menuName="Scriptable Objects/Ability/PassiveAbility/ExtraActions")]
	public class ExtraActions : PassiveAbilityData
	{
		[SerializeField] private ExtraActionsInfo _ExtraActionsInfo;
		public override PropertyInfo Property { get => _ExtraActionsInfo; }
        public override Type PropertyType => typeof(ExtraActionsComponent);
    }
}

namespace Project.Abilities
{
    [Serializable]
	public class ExtraActionsInfo : PropertyInfo
	{
		public int ExtraActions;
    }

	public class ExtraActionsComponent : PassiveAbilityComponent<ExtraActionsInfo>
	{
		protected override void Init()
        {
			((IActionPoints)Caster.UnitTurn).AddActionPoints(Info.ExtraActions);
        }
    }
}