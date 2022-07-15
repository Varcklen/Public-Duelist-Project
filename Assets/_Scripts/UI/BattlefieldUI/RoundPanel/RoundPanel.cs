using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Project.Utils.TaskEx;

public class RoundPanel : MonoBehaviour
{
    private TextMeshProUGUI _tmp;
    private bool _animationPlayed;

    private void Awake()
    {
        _tmp = transform.GetChild(0).Find("RoundNumber").GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    public async Task StartAsync(int number)
    {
        gameObject.SetActive(true);
        _animationPlayed = true;
        _tmp.text = number.ToString();
        await TaskEx.WaitWhile(() => _animationPlayed);
        gameObject.SetActive(false);
    }

    public void AnimationEnd()
    {
        _animationPlayed = false;
    }
}
