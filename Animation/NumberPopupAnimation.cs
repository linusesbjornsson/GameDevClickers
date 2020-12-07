using System;
using TMPro;
using UnityEngine;

public class NumberPopupAnimation : MonoBehaviour
{

    private TextMeshPro _number;
    private static readonly float DISAPPEAR_SPEED = 3f;
    private static readonly float DISAPPEAR_TIMER_MAX = 1f;
    private static readonly float INCREASE_SCALE_AMOUNT = 0.5f;

    private Vector3 moveVector;
    private float disappearTimer;
    private Color textColor;
    private static int sortingOrder = 0;

    public static NumberPopupAnimation Create(Vector3 position, string number)
    {
        return Create(position, number, 6);
    }

    public static NumberPopupAnimation Create(Vector3 position, string number, int textSize)
    {
        return Create(position, number, textSize, Color.clear);
    }
    public static NumberPopupAnimation Create(Vector3 position, string number, int textSize, Color color)
    {
        Transform numberTransform = Instantiate(GameAssets.Instance.pfNumberPopup, position, Quaternion.identity);
        NumberPopupAnimation popup = numberTransform.GetComponent<NumberPopupAnimation>();
        popup.Setup(number, textSize, color);
        return popup;
    }

    private void Awake()
    {
        _number = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(string number, int textSize, Color color)
    {
        if (color != Color.clear)
        {
            _number.color = color;
        }
        _number.SetText(number.ToString());
        _number.fontSize = textSize;
        textColor = _number.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        moveVector = new Vector3(.7f, 1) * 60f;
        sortingOrder++;
        _number.sortingOrder = sortingOrder;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 120f * Time.deltaTime;
        transform.localScale -= Vector3.one * INCREASE_SCALE_AMOUNT * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= DISAPPEAR_SPEED * Time.deltaTime;
            _number.color = textColor;
            if (textColor.a < 0)
            {
                sortingOrder--;
                Destroy(gameObject);
            }
        }
    }
}
