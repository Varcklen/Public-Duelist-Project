using UnityEngine;
using Project.UnitNS;
using Project.Abilities;

namespace Project.AI.Considerations
{
	[CreateAssetMenu(fileName = "New IsUsable", menuName = "Scriptable Objects/Consideration/Base Consideration/IsUsable")]
	public class IsUsable : ConsiderationBool
	{
		protected override bool GetScore(Unit unit, ClickableAbility ability)
		{
			return ability.BanModule.IsUsable;
		}
	}
}