using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TMP_Text LiveCountText;
    public TMP_Text DeadCountText;
       
    public void UpdateUI()
    {
        LiveCountText.text = Game2D.Instance.LiverCount.ToString();
        DeadCountText.text = Game2D.Instance.EnemiesCount.ToString();
    }
}
