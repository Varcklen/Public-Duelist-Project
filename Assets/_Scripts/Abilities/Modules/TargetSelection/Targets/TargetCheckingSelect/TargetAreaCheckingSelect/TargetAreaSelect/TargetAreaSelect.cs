using Project.BattlefieldNS;

namespace Project.Abilities
{
    public class TargetAreaSelect : TargetAreaCheckingSelect
    {
        private Area _targetArea;

        protected override ITargetResult GetTargetResult()
        {
            return _targetArea;
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
