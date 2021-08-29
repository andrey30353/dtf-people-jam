using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedKillAllEnemies : MonoBehaviour
{
    public List<Enemy2D> Enemies;

    public GameObject MessageUI;

    public UnityEvent OnComplete;

    private void Start()
    {
        MessageUI?.SetActive(true);
    }

    private void Update()
    {
        if (Game2D.Instance.EnemiesCount == 0)
        {
            OnComplete?.Invoke();

            MessageUI?.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
