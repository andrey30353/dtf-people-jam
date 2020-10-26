using System.Collections;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public float Timer = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);

        var enemy = collision.GetComponent<Enemy2D>();
        if (enemy != null)
        {
            StartCoroutine(EnemyDeadCor(Timer, enemy));

            return;
        }

        var liver = collision.GetComponent<Liver2D>();
        if (liver != null)
        {
            StartCoroutine(LiverDeadCor(Timer, liver));
            return;
        }

        Destroy(collision.gameObject, Timer);

    }

    private IEnumerator EnemyDeadCor(float time, Enemy2D enemy)
    {
        yield return new WaitForSeconds(time);

        enemy?.Dead(false);
    }

    private IEnumerator LiverDeadCor(float time, Liver2D liver)
    {
        yield return new WaitForSeconds(time);

        liver?.Dead(false);
    }
}
