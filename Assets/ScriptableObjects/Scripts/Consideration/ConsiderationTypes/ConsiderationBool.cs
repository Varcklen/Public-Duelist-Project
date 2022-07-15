using Project.Abilities;
using Project.UnitNS;
using UnityEngine;

namespace Project.AI
{
	public abstract class ConsiderationBool : Consideration
	{
		public override float Score(Unit unit, ClickableAbility ability) => GetScore(unit, ability) ? 1 : 0;
		protected abstract bool GetScore(Unit unit, ClickableAbility ability);
	}
}