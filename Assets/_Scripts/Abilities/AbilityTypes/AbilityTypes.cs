using UnityEngine;

namespace Project.Abilities
{
    /// <summary>
    /// Active abilities can be different for units. They can be used. They cannot be used if the unit is silensed.
    /// </summary>
    public abstract class ActiveAbility : ClickableAbility
    {
        public override AbilityType AbilityType => AbilityType.Active;

        protected override sealed AbilityBan ExtraBanConditions(bool useWarnings)
        {
            if (Caster.UnitStatuses.Silence.Enabled)
            {
                if (useWarnings) Debug.LogWarning("Unit is Silenced!");
                return AbilityBan.Silenced;
            }
            return AbilityBan.None;
        }
    }

    /// <summary>
    /// Passive abilities can be different for units. They cannot be used.
    /// </summary>
    public abstract class PassiveAbility : Ability
    {
        public override AbilityType AbilityType => AbilityType.Passive;

        protected override sealed AbilityBan ExtraBanConditions(bool useWarnings) { return AbilityBan.None; }
    }

    /// <summary>
    /// Skills are almost always the same for all units. They can be used.
    /// </summary>
    public abstract class Skill : ClickableAbility
    {
        public override AbilityType AbilityType => AbilityType.Skill;

        protected override sealed AbilityBan ExtraBanConditions(bool useWarnings) { return AbilityBan.None; }
    }
}
