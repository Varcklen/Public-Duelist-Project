using UnityEngine;
using Project.BattlefieldNS;

namespace Project.Abilities
{
    public sealed class CrossSelect : AreaSelect
    {
        protected override void OnCursorEnable(Area unitArea)
        {
            SetTarget(unitArea, Vector2.down);
            SetTarget(unitArea, Vector2.up);
            SetTarget(unitArea, Vector2.left);
            SetTarget(unitArea, Vector2.right);
        }

        private void SetTarget(Area unitArea, Vector2 vectorToAdd)
        {
            Vector2 vector = unitArea.VectorPosition + vectorToAdd;
            var unitAreaCheck = unitArea.Side.GetUnitArea(vector, false);
            if (unitAreaCheck != null)
            {
                TryAddContains(unitAreaCheck);
            }
            //If area not found, try to add mirror area 
            else if (unitArea.VectorPosition.x == 1)
            {
                var opositeArea = unitArea.GetMirrorUnitArea();
                TryAddContains(opositeArea);
            }
        }
    }
}
