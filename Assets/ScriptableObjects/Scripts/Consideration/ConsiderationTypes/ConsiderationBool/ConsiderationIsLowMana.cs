using Project.Abilities;
using Project.UnitNS;
using UnityEngine;

namespace Project.AI.Considerations
{
	[CreateAssetMenu(fileName = "New ConsiderationIsLowMana", menuName = "Scriptable Objects/Consideration/Consideration Bool/ConsiderationIsLowMana")]
	public class ConsiderationIsLowMana : ConsiderationBool
	{
		[SerializeField, Range(0, 1)] float _minPercentCondition;
		protected override bool GetScore(Unit unit, ClickableAbility ability)
		{
			var mana = unit.UnitStats.UnitInfo.Mana;
			bool condition = mana.Value <= (mana.MaxValue * _minPercentCondition);
			return condition;
		}
	}
}