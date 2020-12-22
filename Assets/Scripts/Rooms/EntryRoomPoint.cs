using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryRoomPoint : MonoBehaviour
{
    [SerializeField] private GameObject roomCameraPoint;
    public GameObject RoomCameraPoint { get => roomCameraPoint; set => roomCameraPoint = value; }
}
