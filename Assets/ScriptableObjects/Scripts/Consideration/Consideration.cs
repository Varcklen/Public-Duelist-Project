using Project.Abilities;
using Project.UnitNS;
using UnityEngine;

namespace Project.AI
{
	public abstract class Consideration : ScriptableObject
	{
		public abstract float Score(Unit unit, ClickableAbility ability);
	}
}