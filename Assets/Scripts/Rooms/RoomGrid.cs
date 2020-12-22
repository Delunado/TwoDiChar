using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGrid
{
    public struct GridPosition
    {
        int gridPosX, gridPosY;
        public int GridPosX { get => gridPosX; }
        public int GridPosY { get => gridPosY; }

        public GridPosition(int gridPosX, int gridPosY)
        {
            this.gridPosX = gridPosX;
            this.gridPosY = gridPosY;
        }
    }

    RoomManager[,] grid;
    private int height, width;

    public RoomGrid(int height, int width)
    {
        this.height = height;
        this.width = width;

        grid = new RoomManager[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = null;
            }
        }
    }

    public RoomManager GetRoom(GridPosition gridPosition)
    {
        return grid[gridPosition.GridPosX, gridPosition.GridPosY];
    }

    /// <summary>
    /// Adds a room in the grid, in the indicated GridPosition 
    /// </summary>
    /// <param name="roomManager"></param>
    /// <param name="gridPosition"></param>
    public void AddRoom(RoomManager roomManager, GridPosition gridPosition)
    {
        roomManager.GridPosition = gridPosition;
        grid[gridPosition.GridPosX, gridPosition.GridPosY] = roomManager;
    }

    public void ConfigRooms()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[i, j] == null)
                    continue;

                if (ValidPosition(i - 1, j))
                {
                    if (grid[i - 1, j] != null)
                    {
                        grid[i, j].EntriesManager.OpenEntry(EntriesManager.EntriesPosition.RIGHT);
                    }
                }

                if (ValidPosition(i + 1, j))
                {
                    if (grid[i + 1, j] != null)
                    {
                        grid[i, j].EntriesManager.OpenEntry(EntriesManager.EntriesPosition.LEFT);
                    }
                }

                if (ValidPosition(i, j + 1))
                {
                    if (grid[i, j + 1] != null)
                    {
                        grid[i, j].EntriesManager.OpenEntry(EntriesManager.EntriesPosition.TOP);
                    }
                }

                if (ValidPosition(i, j - 1))
                {
                    if (grid[i, j - 1] != null)
                    {
                        grid[i, j].EntriesManager.OpenEntry(EntriesManager.EntriesPosition.BOTTOM);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns true if the GridPosition is a valid position in the grid.
    /// </summary>
    /// <returns></returns>
    public bool ValidPosition(GridPosition gridPosition)
    {
        return ValidPosition(gridPosition.GridPosX, gridPosition.GridPosY);
    }

    /// <summary>
    /// Returns true if the posX & posY is a valid position in the grid.
    /// </summary>
    /// <returns></returns>
    private bool ValidPosition(int posX, int posY)
    {
        return posX < width && posX >= 0 && posY < height && posY >= 0;
    }
}
