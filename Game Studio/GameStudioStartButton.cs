using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStudioStartButton : MonoBehaviour
{
    public GameObject activeStudio;
    public GameObject buyStudio;
    public Button startButton;
    public TextMeshProUGUI priceText;
    public Player player;
    public GameStudio studio;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = new NumberFormatter(GameStudio.GAME_STUDIO_PRICE).Format();
        startButton.onClick.AddListener(BuyGameStudio);
        if (studio.IsActivated())
        {
            ActivatePanel();
        }
    }

    void BuyGameStudio()
    {
        studio.Activate();
        player.Money -= new BigRational(GameStudio.GAME_STUDIO_PRICE);
        ActivatePanel();
    }

    void ActivatePanel()
    {
        activeStudio.SetActive(true);
        buyStudio.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Money >= new BigRational(GameStudio.GAME_STUDIO_PRICE))
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }
}
