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
        LiveCountText.text = Game.Instance.LiveCount.ToString();
        DeadCountText.text = Game.Instance.DeadCount.ToString();
    }
}
