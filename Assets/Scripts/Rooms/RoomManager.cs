using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    EntriesManager entriesManager;
    public EntriesManager EntriesManager { get => entriesManager; }

    RoomGrid.GridPosition gridPosition;
    public RoomGrid.GridPosition GridPosition { get => gridPosition; set => gridPosition = value; }

    private void Awake()
    {
        entriesManager = GetComponent<EntriesManager>();
    }
   
}
