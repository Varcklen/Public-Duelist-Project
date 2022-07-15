using UnityEngine;
using Project.UnitNS;
using Project.Abilities;

namespace Project.AI.Considerations
{
	[CreateAssetMenu(fileName ="New ConsiderationNeverUse", menuName = "Scriptable Objects/Consideration/Consideration Bool/ConsiderationNeverUse")]
	public class ConsiderationNeverUse : ConsiderationBool
	{
		protected override bool GetScore(Unit unit, ClickableAbility ability)
		{
			return false;
		}
	}
}