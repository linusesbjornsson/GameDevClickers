using TMPro;
using UnityEngine;

public class AwardsUI : MonoBehaviour
{
    public Player player;
    private TextMeshProUGUI _awardsText;

    private void Awake()
    {
        player.OnAwardsChanged += OnAwardsChanged;
        _awardsText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Start()
    {
        _awardsText.text = new NumberFormatter(player.Awards).Format();
    }

    void OnAwardsChanged(object sender, AwardsArgs args)
    {
        _awardsText.SetText(new NumberFormatter(player.Awards).Format());
    }
}
