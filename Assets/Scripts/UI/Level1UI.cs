using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1UI : MonoBehaviour
{
    public GameObject TopPanel;

    public TMP_Text LiveCountText;
    public TMP_Text DeadCountText;

    public Slider TimeSlider;
     
    [Space]
    public GameObject StartMessage; 

    [Header("Концовки игры")]
    public GameObject LiversDiedMessage;
    public GameObject ReactorExplosionMessage;    
      
    public void Start()
    {
        //PauseGame(true);
    }

    bool showed;
    public void ShowStartMessage()
    {
        if(!showed)        
            StartMessage.SetActive(true);

        showed = true;
    }

    public void ShowTopPanel()
    {
        TopPanel.SetActive(true);
    }


    public void UpdateAgentCount()
    {
        LiveCountText.text = Game2D.Instance.LiverCount.ToString();
        DeadCountText.text = Game2D.Instance.EnemiesCount.ToString();
    }

    public void SetMaxTime(float fullTime)
    {
        TimeSlider.maxValue = fullTime;
    }

    public void UpdateTime(float timeProgress)
    {
        //print("UpdateDistance");
        TimeSlider.value = timeProgress;
    }  

    public void UpdateMessage(Level1State gameOverState)
    {
       // LiversDiedMessage.SetActive(false);
       // ReactorExplosionMessage.SetActive(false);       

        switch (gameOverState)
        {
            case Level1State.LiversDiedDefeat:
                print("Поражение!");
                PauseGame(true);
                //LiversDiedMessage.SetActive(true);
                break;

            case Level1State.SafersIsArrived:
                print("Победа!");
                PauseGame(true);
               // ReactorExplosionMessage.SetActive(true);
                break;

            default:
                return;
        }
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
