using UnityEngine;
using Project.UnitNS;
using Project.Abilities;

namespace Project.AI.Considerations
{
	[CreateAssetMenu(fileName ="New IsOnCooldown", menuName = "Scriptable Objects/Consideration/Base Consideration/IsOnCooldown")]
	public class IsOnCooldown : ConsiderationBool
	{
		protected override bool GetScore(Unit unit, ClickableAbility ability)
		{
			return !ability.CooldownModule.OnCooldown;
		}
	}
}