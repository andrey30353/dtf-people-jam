using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorsManager : MonoBehaviour
{
    public Door2D[] _doors;

    private void Awake()
    {
        _doors = GetComponentsInChildren<Door2D>();
    }

    public void CanManage(bool value)
    {
        foreach (var door in _doors)
        {
            if (door == null)
                continue;

            door.CanManage = value;
        }
    }
}
