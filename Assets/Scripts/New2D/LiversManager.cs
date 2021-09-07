using UnityEngine;

public class LiversManager : MonoBehaviour
{
    private Liver2D[] _livers;

    private void Awake()
    {
        _livers = GetComponentsInChildren<Liver2D>();
    }

    public void CanManage(bool value)
    {
        foreach (var liver in _livers)
        {
            if (liver == null)
                continue;

            liver.CanManage = value;
        }
    }
}
