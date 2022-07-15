using Project.BattlefieldNS;
using Project.UnitNS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Abilities
{
    /// <summary>
    /// Is a class for calling a select action and getting the targets of that ability.
    /// </summary>
    public abstract class TargetSelection : ITargetSelection
    {
        protected TargetType _targetType;
        protected Unit Caster;

        protected virtual void Init() {}

        /// <summary>
        /// Returns the target or targets of the ability.
        /// </summary>
        public abstract Task<ITargetResult> GetTargetResultAsync(Func<Area, bool> condition, bool choosedRandomly = false);

        #region Create
        private static Dictionary<TargetSelectionType, Type> _dictionary = new Dictionary<TargetSelectionType, Type>()
        {
            [TargetSelectionType.Self] = typeof(SelfSelect),
            [TargetSelectionType.Target] = typeof(TargetSelect),
            [TargetSelectionType.LineVertical] = typeof(LineVerticalSelect),
            [TargetSelectionType.LineHorizontal] = typeof(LineHorizontalSelect),
            [TargetSelectionType.Cross] = typeof(CrossSelect),
            [TargetSelectionType.AreaTarget] = typeof(TargetAreaSelect),
        };
        /// <exception cref="ArgumentException"></exception>
        public static TargetSelection Create(Ability ability, TargetType targetType)
        {
            TargetSelectionType selectionType = targetType.SelectionType;
            bool isSuccesfull = _dictionary.TryGetValue(selectionType, out Type type);
            if (isSuccesfull == false)
            {
                throw new ArgumentException($"_dictionary variable has no mapping [{selectionType}, Type?]. Maybe you forgot to add a new map?");
            }
            TargetSelection targetSelection = (TargetSelection)Activator.CreateInstance(type);
            targetSelection._targetType = targetType;
            targetSelection.Caster = ability.GetComponent<Unit>();
            targetSelection.Init();
            return targetSelection;
        }
        #endregion
    }

    public interface ITargetSelection
    {
        Task<ITargetResult> GetTargetResultAsync(Func<Area, bool> condition, bool choosedRandomly = false);
    }
}
