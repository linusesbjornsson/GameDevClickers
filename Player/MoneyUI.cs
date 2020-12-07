using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public Player player;
    private TextMeshProUGUI _moneyText;

    private void Awake()
    {
        player.OnMoneyChanged += OnMoneyChanged;
        _moneyText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Start()
    {
        _moneyText.text = player.Money.Format();
    }

    void OnMoneyChanged(object sender, MoneyArgs args)
    {
        Vector3 newPosition = new Vector3(transform.position.x + 1.75f, transform.position.y + 0.7f);
        if (args.OldMoney > args.NewMoney)
        {
            NumberPopupAnimation.Create(newPosition, "$" + (args.OldMoney - args.NewMoney).Format(), 6, Color.red);
        }
        else
        {
            if (IsBigFactor(args))
            {
                NumberPopupAnimation.Create(newPosition, "$" + (args.NewMoney - args.OldMoney).Format());
            }
        }
        _moneyText.text = player.Money.Format();
    }

    private bool IsBigFactor(MoneyArgs args)
    {
        if (args.OldMoney < BigRational.One)
        {
            return args.NewMoney <= new BigRational(-10);
        }
        BigRational oldMoney = new BigRational(args.OldMoney.WholePart, args.OldMoney.FractionalPart);
        oldMoney = BigRational.Multiply(oldMoney, new BigRational(10));
        return args.NewMoney >= oldMoney;
    }
}
