using System;
using UnityEngine;
using UnityEngine.Events;

public enum ScenarioState
{
    MoveZoomCamera
}

public class MoveAndZoomCamera : MonoBehaviour
{
    public GameObject CameraMoveMessage;

    public CameraMover CameraMover;

    public float RequiredZoom = 0.8f;
    public Transform RequiredPosition;
    public Vector2 AcceptablePosition;

    public UnityEvent OnComplete;

    private ScenarioState State;

    private void Start()
    {
        CameraMoveMessage.SetActive(true);
    }

    private void Update()
    {
        CheckComplete(State);
    }

    private void CheckComplete(ScenarioState state)
    {
        switch (state)
        {
            case ScenarioState.MoveZoomCamera:
                CheckMoveZoomCameraComplete();
                break;

            default:
                break;
        }
    }

    private void CheckMoveZoomCameraComplete()
    {
        if (CameraMover.Zoom < RequiredZoom)
            return;

        //print("Zoom");
        var cameraX = CameraMover.CameraPosition.x;
        var cameraY = CameraMover.CameraPosition.y;
        var requiredX = RequiredPosition.transform.position.x;
        var requiredY = RequiredPosition.transform.position.y;
        if (cameraX > requiredX - AcceptablePosition.x
            && cameraX < requiredX + AcceptablePosition.x
            && cameraY > requiredY - AcceptablePosition.y
            && cameraY < requiredY + AcceptablePosition.y)
        {
            print("OnComplete");
            OnComplete.Invoke();
            //print("Zoom + Position");
        }
    }

    private void OnDrawGizmos()
    {
        if (RequiredPosition == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(RequiredPosition.position, AcceptablePosition * 2);
    }
}
