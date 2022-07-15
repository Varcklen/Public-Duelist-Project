using Project.BattlefieldNS.Interfaces;
using Project.Interfaces;
using Project.Stats;
using Project.Utils.Extension.ObjectNS;
using TMPro;
using UnityEngine;

namespace Project.BattlefieldNS
{
    /// <summary>
    /// The class is the base class for UI elements that use any numbers as text to display them in an area.
    /// </summary>
    public abstract class AreaUI : MonoBehaviour, IShowHide, ISetResourceType
    {
        protected Area UnitArea;
        protected StatType StatType;

        protected TextMeshPro Text { set; get; }
        protected abstract Stat Stat { get; }

        protected void OnEnable()
        {
            UnitArea.OnPlacedUnitChanged += UpdateUI;
        }

        protected void OnDisable()
        {
            UnitArea.OnPlacedUnitChanged -= UpdateUI;
        }

        public abstract void SetStatType(StatType statType);

        protected abstract void UpdateUI();

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        protected void Awake()
        {
            UnitArea = transform.GetComponentInParent<Area>().IsNullException();
        }

        protected void ChangeColor()
        {
            if (Stat.MaxValue > Stat.BaseValue)
            {
                Text.color = Color.green;
            }
            else if (Stat.MaxValue < Stat.BaseValue)
            {
                Text.color = Color.red;
            }
            else
            {
                Text.color = Color.white;
            }
        }
    }
}
