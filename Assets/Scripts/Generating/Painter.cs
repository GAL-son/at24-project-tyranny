using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Painter : MonoBehaviour
{
    public const int FLOOR_ID = 1;
    public const int WALL_ID = 2;

    public EnviromentController controller;
    public TilemapGameObjectManager tilemapGameObjectManager;

    [Header("Paintable Objects")]
    public List<GameObject> walls;
    public List<GameObject> floors;
    public List<GameObject> decors;

    public int getNumberOfWalls()
    {
        return walls.Count;
    }

    public int getNumberOfFloors()
    {
        return floors.Count;
    }

    public void paintGrid(int[,] grid, Vector2Int size)
    {

        for(int y = 0; y < size.y; y++)
        {
            for(int x = 0; x < size.x; x++)
            {
                
                paintTile(new Vector2Int(x, y), grid, size);
            }
        }

        // tilemapGameObjectManager.Init();
    }

    void paintTile(Vector2Int tile, int[,] grid, Vector2Int size)
    {
        GameObject o = null;
        if (grid[tile.x, tile.y] == FLOOR_ID) {

            o = paintFloor();
        }

        if(o != null)
        {
            Vector2Int newPos = tile - size;
            Debug.Log(newPos);
            o.transform.position = (controller.worldGrid.CellToWorld(new Vector3Int(newPos.x, newPos.y, 0)));
        }
    }

    GameObject paintFloor()
    {
        if(floors.Count > 0)
        {
            return Instantiate(floors[0], tilemapGameObjectManager.transform);
        }
        return null;
    }
}
