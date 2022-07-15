using UnityEngine;
using Project.UnitNS;
using Project.Abilities;

namespace Project.AI.Considerations
{
	[CreateAssetMenu(fileName ="New IsEnoughResource", menuName = "Scriptable Objects/Consideration/Base Consideration/IsEnoughResource")]
	public class IsEnoughResource : ConsiderationBool
	{
		protected override bool GetScore(Unit unit, ClickableAbility ability)
		{
			return ability.ResourceModule.IsEnoughResource;
		}
	}
}