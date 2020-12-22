using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntriesManager : MonoBehaviour
{
    public enum EntriesPosition
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }

    [Header("Entries Walls & Grounds")]
    [SerializeField] GameObject leftEntryWallTilemap;
    [SerializeField] GameObject rightEntryWallTilemap;
    [SerializeField] GameObject topEntryGroundTilemap;
    [SerializeField] GameObject bottomEntryGroundTilemap;

    [Header("Entries colliders")]
    [SerializeField] GameObject leftEntryCollider;
    [SerializeField] GameObject rightEntryCollider;
    [SerializeField] GameObject topEntryCollider;
    [SerializeField] GameObject bottomEntryCollider;

    public void OpenEntry(EntriesPosition entryPosition)
    {
        switch (entryPosition)
        {
            case EntriesPosition.TOP:
                topEntryCollider.SetActive(true);
                topEntryGroundTilemap.SetActive(false);
                break;
            case EntriesPosition.BOTTOM:
                bottomEntryCollider.SetActive(true);
                bottomEntryGroundTilemap.SetActive(false);
                break;
            case EntriesPosition.LEFT:
                leftEntryCollider.SetActive(true);
                leftEntryWallTilemap.SetActive(false);
                break;
            case EntriesPosition.RIGHT:
                rightEntryCollider.SetActive(true);
                rightEntryWallTilemap.SetActive(false);
                break;
        }
    }
}
