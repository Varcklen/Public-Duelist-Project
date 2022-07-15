using Project.BattlefieldNS;
using Project.Utils.Events;
using UnityEngine;

public class EndBattlePanel : MonoBehaviour
{

    private void Awake()
    {
        Events.OnBattleEnd.AddListener(Show);

        Hide();
    }

    private void OnDestroy()
    {
        Events.OnBattleEnd.RemoveListener(Show);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show(SideType sideType)
    {
        gameObject.SetActive(true);
    }

    public void ExitBattle()
    {
        Debug.Log("You leave current battle!");
    }
}
