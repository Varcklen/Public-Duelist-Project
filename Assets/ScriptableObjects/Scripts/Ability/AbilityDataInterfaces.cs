using Project.Abilities.Data;

namespace Project.Abilities.Interfaces
{
    /// <summary>
    /// This is received by component classes, which must contain information unique to the ability.
    /// </summary>
    public interface IAbilityComponent
    {
        protected PropertyInfo _info { get; set; }

        internal void SetInfo(PropertyInfo info)
        {
            _info = info;
        }
    }
}