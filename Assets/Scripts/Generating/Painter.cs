using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Painter : MonoBehaviour
{
    public const int FLOOR_ID = 1;
    public const int WALL_ID = 2;
    public const int SMALL_WALL_ID = 3;

    public EnviromentController controller;
    public TilemapGameObjectManager tilemapGameObjectManager;
    public TilemapGameObjectManager wallLayer;

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
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {

                paintTile(new Vector2Int(x, y), grid, size);
            }
        }
        tilemapGameObjectManager.Init();

    }

    void paintTile(Vector2Int tile, int[,] grid, Vector2Int size)
    {
        GameObject o = null;
        int curr = grid[tile.x, tile.y];
        switch(curr)
        {
            case FLOOR_ID:
                o = paintFloor();
                break;
            case WALL_ID:
                o = paintWall(0);
                break;
            case SMALL_WALL_ID:
                o = paintWall(1);
                break;

        }

        if (o != null)
        {
            Vector2Int newPos = tile - size / 2;
            o.transform.position = (controller.getCellCenter(new Vector3Int(newPos.x, newPos.y, 0)));
        }
    }

    GameObject paintFloor()
    {
        if (floors.Count > 0)
        {
            return Instantiate(floors[0], tilemapGameObjectManager.transform);
        }
        return null;
    }

    GameObject paintWall(int wall)
    {
        if (walls.Count > 0)
        {
            return Instantiate(walls[wall], wallLayer.transform);
        }
        return null;
    }

    public void Clean()
    {
        foreach (Transform transform in tilemapGameObjectManager.GetComponentInChildren<Transform>())
        {
            GameObject go = transform.gameObject;
            Destroy(go);
        }

        foreach (Transform transform in wallLayer.GetComponentInChildren<Transform>())
        {
            GameObject go = transform.gameObject;
            Destroy(go);
        }
    }
}
