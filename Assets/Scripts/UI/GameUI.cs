using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TMP_Text LiveCountText;
    public TMP_Text DeadCountText;

    public TMP_Text SpeedText;

    public GameObject AlarmPanel;
    public TMP_Text DestroyTimerText;

    public Slider DistanceSlider;

    public Slider EngineSlider1;
    public Slider EngineSlider2;
    public Slider ReactorSlider;
    public Image CapitanImage;

    public Image EngineImageMap1;
    public Image EngineImageMap2;
    public Image ReactorImageMap;
    public Image CapitanImageMap;

    public Color normColor;
    public Color errorColor;


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
        SpeedText.text = $"{Mathf.RoundToInt(currentSpeed * 100).ToString()}%";
        SpeedText.color = currentSpeed > 0 ? normColor : errorColor;
        //SpeedText.text = $"Скорость: {currentSpeed.ToString("F1")}";
    }

    public void UpdateDestroyTimer(int time, bool show)
    {
        //DestroyTimerText.text = $"{time / 60}:{time % 60}";
        //var timeSec = Mathf.RoundToInt(time);

        AlarmPanel.SetActive(show);

        //DestroyTimerText.text = $"Время до уничтожения: {timeSec}";
        if (time < 0)
            time = 0;

        var str = time < 10 ? $"0{time}" : time.ToString();
        DestroyTimerText.text = str;
    }

    public void UpdateEngine1(float hp)
    { 
        EngineSlider1.value = hp;
        EngineImageMap1.color = hp > 0 ? normColor : errorColor;
    }
    public void UpdateEngine2(float hp)
    {
        EngineSlider2.value = hp;
        EngineImageMap2.color = hp > 0 ? normColor : errorColor;
    }
    public void UpdateReactor(float hp)
    {
        ReactorSlider.value = hp;
        ReactorImageMap.color = hp > 0 ? normColor : errorColor;
    }
    public void UpdateCapitan(bool error)
    {
        CapitanImage.color = error ? errorColor : normColor;
        CapitanImageMap.color = error ? errorColor : normColor;        
    }

    public void SetSpaceModules(float engineHp1, float engineHp2, float reactorHp1, bool error )
    {
        EngineSlider1.maxValue = engineHp1;    
        EngineSlider2.maxValue = engineHp2;   
        ReactorSlider.maxValue = reactorHp1;     

        EngineSlider1.value = engineHp1;
        EngineSlider2.value = engineHp2;
        ReactorSlider.value = reactorHp1;

        UpdateCapitan(error);
    }


    public void PauseGame(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }


}
