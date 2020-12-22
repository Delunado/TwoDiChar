using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private int gridHeight = 4, gridWidth = 4;
    [SerializeField] private int maxRooms = 6;
    [SerializeField] RoomManager principalRoom;
    [SerializeField] GameObject[] rooms;

    RoomGrid roomGrid;

    Vector2 offsetPosition;
    int principalRoomGridPosX; 
    int principalRoomGridPosY; 

    private void Start()
    {
        roomGrid = new RoomGrid(gridWidth, gridHeight);

        if (maxRooms > gridHeight * gridWidth)
        {
            maxRooms = (gridHeight * gridWidth) - 1;
            Debug.LogWarning("Max Rooms can't be greater than grid area. Max Rooms have been adjusted to the grid area");
        }

        //We add the central room and calculate the offset position
        principalRoomGridPosX = (gridWidth / 2);
        principalRoomGridPosY = (gridHeight / 2);
        Debug.Log(principalRoomGridPosX);
        Debug.Log(principalRoomGridPosY);

        RoomGrid.GridPosition centerRoomGridPosition = new RoomGrid.GridPosition(principalRoomGridPosX, principalRoomGridPosY);
        roomGrid.AddRoom(principalRoom, centerRoomGridPosition);

        offsetPosition = new Vector2(-28, 16);

        //We generate the world
        GenerateWorld();

        //We config the rooms.
        roomGrid.ConfigRooms();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void GenerateWorld()
    {
        List<RoomGrid.GridPosition> positionsList = new List<RoomGrid.GridPosition>();
        positionsList.Add(new RoomGrid.GridPosition(gridHeight / 2, gridWidth / 2));

        int generatedRooms = 0;
        while (generatedRooms < maxRooms)
        {
            //We get a random position
            RoomGrid.GridPosition actualPos = positionsList[Random.Range(0, positionsList.Count)];

            //We create a room in that position
            if (roomGrid.GetRoom(actualPos) == null)
            {
                RoomManager room = Instantiate(rooms[Random.Range(0, rooms.Length)], CalculateInstantiatonPosition(actualPos), Quaternion.identity).GetComponent<RoomManager>();
                roomGrid.AddRoom(room, actualPos);
                generatedRooms++;
            }

            //Then we add to the posible positions list all the reachable positions
            RoomGrid.GridPosition leftPos = new RoomGrid.GridPosition(actualPos.GridPosX - 1, actualPos.GridPosY);
            RoomGrid.GridPosition rightPos = new RoomGrid.GridPosition(actualPos.GridPosX + 1, actualPos.GridPosY);
            RoomGrid.GridPosition upPos = new RoomGrid.GridPosition(actualPos.GridPosX, actualPos.GridPosY - 1);
            RoomGrid.GridPosition downPos = new RoomGrid.GridPosition(actualPos.GridPosX, actualPos.GridPosY + 1);

            if (roomGrid.ValidPosition(leftPos))
                positionsList.Add(leftPos);

            if (roomGrid.ValidPosition(rightPos))
                positionsList.Add(rightPos);

            if (roomGrid.ValidPosition(upPos))
                positionsList.Add(upPos);

            if (roomGrid.ValidPosition(downPos))
                positionsList.Add(downPos);

            //Then we eliminate the actual pos from the list
            positionsList.Remove(actualPos);
        }
    }

    private Vector2 CalculateInstantiatonPosition(RoomGrid.GridPosition gridPosition)
    {
        return new Vector2(
            (gridPosition.GridPosX - principalRoomGridPosX) * offsetPosition.x,
            (gridPosition.GridPosY - principalRoomGridPosY) * offsetPosition.y);
    }
}
