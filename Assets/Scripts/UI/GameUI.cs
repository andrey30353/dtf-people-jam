using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TMP_Text LiveCountText;
    public TMP_Text DeadCountText;

    public TMP_Text SpeedText;

    public Slider DistanceSlider;

    public void UpdateAgentCount()
    {
        LiveCountText.text = Game2D.Instance.LiverCount.ToString();
        DeadCountText.text = Game2D.Instance.EnemiesCount.ToString();
    }

    public void SetMaxDistance(float maxDistanceSec)
    {
        DistanceSlider.maxValue = maxDistanceSec;
    }

    public void UpdateDistance(float progressDistance)
    {
        //print("UpdateDistance");
        DistanceSlider.value = progressDistance;
    }

    public void UpdateSpeed(float currentSpeed)
    {
        //print("UpdateDistance");
        SpeedText.text = $"Скорость: {Mathf.RoundToInt(currentSpeed * 100).ToString()}%";
        //SpeedText.text = $"Скорость: {currentSpeed.ToString("F1")}";
    }
}
