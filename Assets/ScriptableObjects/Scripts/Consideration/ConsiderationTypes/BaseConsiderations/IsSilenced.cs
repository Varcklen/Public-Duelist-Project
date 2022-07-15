using UnityEngine;
using Project.Abilities;
using Project.UnitNS;

namespace Project.AI.Considerations
{
	[CreateAssetMenu(fileName ="New IsSilenced", menuName = "Scriptable Objects/Consideration/Base Consideration/IsSilenced")]
	public class IsSilenced : ConsiderationBool
	{
		protected override bool GetScore(Unit unit, ClickableAbility ability)
		{
			if (unit.UnitStatuses.Silence.Enabled && ability is ActiveAbility) return false;
			return true;
		}
	}
}