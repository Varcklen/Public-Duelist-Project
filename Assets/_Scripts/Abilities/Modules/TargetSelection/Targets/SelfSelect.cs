using Project.BattlefieldNS;
using System;
using System.Threading.Tasks;

namespace Project.Abilities
{
    public sealed class SelfSelect : TargetSelection
    {
        public override Task<ITargetResult> GetTargetResultAsync(Func<Area, bool> condition, bool choosedRandomly = false)
        {
            return Task.FromResult((ITargetResult)Caster);
        }
    }
}
