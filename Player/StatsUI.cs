using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI technologyText;
    public TextMeshProUGUI designText;
    public TextMeshProUGUI audioText;

    void Start()
    {
        UpdateUI();
        player.OnLevelGained += OnPlayerLevel;
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        levelText.text = "Level " + player.Level;
        technologyText.text = player.Technology.Format();
        designText.text = player.Design.Format();
        audioText.text = player.Audio.Format();
    }

    private void OnPlayerLevel(object sender, LevelArgs args)
    {
        UpdateUI();
    }
}
