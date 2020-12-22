using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    CinemachineVirtualCamera playerCamera;

    private void Awake()
    {
        playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EntryRoomPoint entry = collision.GetComponent<EntryRoomPoint>();

        if (entry)
        {
            playerCamera.Follow = entry.RoomCameraPoint.transform;
        }
    }
}
