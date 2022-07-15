using Project.Abilities;
using Project.BattlefieldNS;
using Project.UnitNS;

namespace Project.Utils.Events
{
    /// <summary>
    /// All general events are stored here, which are fired regardless of the class instance.
    /// </summary>
    internal static class Events
    {
        //Unit Events
        public static readonly ActionEvent<Unit> OnUnitCreate = new ActionEvent<Unit>();
        public static readonly ActionEvent<Unit> OnUnitKill = new ActionEvent<Unit>();
        public static readonly ActionEvent<Unit> OnAfterUnitKill = new ActionEvent<Unit>();
        public static readonly ActionEvent<Unit> OnUnitRessurect = new ActionEvent<Unit>();
        public static readonly ActionEvent<Unit> OnUnitTurnStart = new ActionEvent<Unit>();
        public static readonly ActionEvent<Unit> OnUnitTurnEnd = new ActionEvent<Unit>();
        public static readonly ActionEvent OnSpeedStatChanged = new ActionEvent();

        //Side Events
        public static readonly ActionEvent<SideType> OnSideEmpty = new ActionEvent<SideType>();

        //UI Events
        public static readonly ActionEvent OnTargetCircleCursorEnter = new ActionEvent();
        public static readonly ActionEvent OnTargetCircleCursorExit = new ActionEvent();

        //Battle Events
        public static readonly ActionEvent OnBattleStart = new ActionEvent();
        public static readonly ActionEvent<SideType> OnBattleEnd = new ActionEvent<SideType>();

        //Ability Events
        public static readonly ActionEvent<ClickableAbility> OnAbilitySearchTargetStart = new ActionEvent<ClickableAbility>();
        public static readonly ActionEvent<ClickableAbility> OnAbilityCastEnd = new ActionEvent<ClickableAbility>();
        public static readonly ActionEvent<ClickableAbility> OnAbilityCanceled = new ActionEvent<ClickableAbility>();
    }

}