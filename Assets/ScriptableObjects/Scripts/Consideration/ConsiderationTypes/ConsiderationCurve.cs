using Project.Abilities;
using Project.UnitNS;
using UnityEngine;

namespace Project.AI
{
	public abstract class ConsiderationCurve : Consideration
	{
		[SerializeField] private AnimationCurve _responceCurve;
		protected AnimationCurve ResponceCurve => _responceCurve;

		public override float Score(Unit unit, ClickableAbility ability) => Mathf.Clamp01(GetScore(unit, ability));
		protected abstract float GetScore(Unit unit, ClickableAbility ability);
	}
}