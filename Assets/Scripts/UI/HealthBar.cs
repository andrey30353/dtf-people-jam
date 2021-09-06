using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void Init(float maxHp, float currentHp)
    {
        _slider.maxValue = maxHp;
        _slider.value = currentHp;
    }

    public void UpdateHp(float currentHp)
    {
        _slider.value = currentHp;
    }
}
