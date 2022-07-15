using Project.BattlefieldNS;

namespace Project.Abilities
{
    public sealed class TargetSelect : TargetUnitCheckingSelect
    {
        private Area _targetArea;

        protected override ITargetResult GetTargetResult()
        {
            var result = _targetArea?.PlacedUnit;
            _targetArea = null;
            return result;
        }

        protected override void OnCorrectClick(Area unitArea)
        {
            SetTargets(unitArea);
        }

        protected override void SetTargets(Area unitArea)
        {
            _targetArea = unitArea;
        }
    }
}
