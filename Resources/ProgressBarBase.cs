using UnityEngine;
using UnityEngine.UI;

public class ProgressBarBase : MonoBehaviour
{
    public Slider slider;
    private static readonly float FILL_SPEED = 100f;

    public virtual float GetProgress(float val, float nextVal)
    {
        return Mathf.Abs(val / nextVal * 100);
    }
    public virtual float GetProgress(BigRational val, BigRational nextVal)
    {
        return (float) BigRational.Abs(BigRational.Multiply(BigRational.Divide(val, nextVal), new BigRational(100)));
    }

    public virtual float GetFillSpeed()
    {
        return FILL_SPEED;
    }
}
