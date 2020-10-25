using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowAnimation : MonoBehaviour
{
    private Vector3 StartScale;
    public Vector3 TargetScale;

    public float GrowTime;

    float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        transform.localScale = Vector3.Lerp(StartScale, TargetScale, elapsedTime / GrowTime);
    }
}
