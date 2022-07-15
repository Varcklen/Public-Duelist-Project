using Project.Attributes;
using Project.BattlefieldNS;
using System;

namespace Project.Abilities
{
    /// <summary>
    /// Stores information about the type of target the ability will be applied to.
    /// </summary>
    [Serializable]
    public struct TargetType
    {
        public TargetSelectionType SelectionType;
        [EnumHide("SelectionType", (int)TargetSelectionType.Self, true, inverse: true)]
        public TargetSide TargetSide;
        [EnumHide("SelectionType", (int)TargetSelectionType.Target, true)]
        public bool CanChooseSelf;
        
        /*public TargetType(TargetSide targetSide, bool canChooseSelf, TargetSelectionType selectionType)
        {
            TargetSide = targetSide;
            CanChooseSelf = canChooseSelf;
            SelectionType = selectionType;
        }*/
    }

    public enum TargetSide : byte
    {
        Ally = SideType.Ally,
        Enemy = SideType.Enemy,
        All
    }

    public enum TargetSelectionType
    {
        Self,
        Target,
        LineVertical,
        LineHorizontal,
        Cross,
        AreaTarget
    }
}
