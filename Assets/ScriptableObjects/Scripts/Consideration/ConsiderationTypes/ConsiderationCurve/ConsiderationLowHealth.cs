using Project.Abilities;
using Project.UnitNS;
using UnityEngine;

namespace Project.AI.Considerations
{
	[CreateAssetMenu(fileName ="New ConsiderationTestCurve", menuName = "Scriptable Objects/Consideration/Consideration Curve/ConsiderationTestCurve")]
	public class ConsiderationLowHealth : ConsiderationCurve
	{
		protected override float GetScore(Unit unit, ClickableAbility ability)
		{
			var health = unit.UnitStats.UnitInfo.Health;
			var curve = ResponceCurve.Evaluate((float)health.Value / health.MaxValue);
			return curve;
		}
	}
}