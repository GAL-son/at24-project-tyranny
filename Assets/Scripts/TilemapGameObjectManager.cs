using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]

public class TilemapGameObjectManager : MonoBehaviour
{
    BoundsInt cellBounds;
    Bounds worldBounds;

    Tilemap tilemap;
    GameObject[,,] tiles;

    Vector3Int maxIndex = new Vector3Int();

    // Start is called before the first frame update

    private void Awake()
    {
       

    }
    void Start()
    {
        
    }

    public void Init()
    {
        tilemap = GetComponent<Tilemap>();
        tiles = tilemap.GetArrangedGameObjects();
        cellBounds = tilemap.GetGameObjectTilemapCellBounds();
        maxIndex = cellBounds.size + new Vector3Int(1,1,1);
        worldBounds = tilemap.GetGameObjectTilemapBounds();
    }

    public GameObject GetObjectAtCell(Vector3Int cell)
    {
        Vector3Int cellOffset = OffsetVector(cell);
        if(!offsetInArray(cellOffset))
        {
            return null;
        }

        try
        {
            return tiles[cellOffset.x, cellOffset.y, cellOffset.z];
        } catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    public GameObject GetObjectAtPosition(Vector3 position)
    {
        Vector3Int cellPosition = tilemap.layoutGrid.WorldToCell(position);

        return GetObjectAtCell(cellPosition);
    }

    public GameObject GetObjectAtIndex(Vector3Int index)
    {
        GameObject obj = tiles[index.x, index.y, index.z];
        return obj;
    }

    public bool isIndexCorrect(Vector3Int index)
    {
        return (index.x < maxIndex.x && index.y < maxIndex.y && index.z < maxIndex.z && index.x >= 0 && index.y >= 0 && index.z >= 0);
    }

    public BoundsInt GetCellBounds()
    {
        if(tilemap == null)
        {
            Debug.LogError("NO TILE MAP AT" + name);
            return cellBounds;
        }
        return tilemap.GetGameObjectTilemapCellBounds();
    }

    public Bounds GetWorldBounds()
    {
        return tilemap.GetGameObjectTilemapBounds();
    }

    public Vector3Int GetArrayOffset()
    {
        return new Vector3Int(GetCellBounds().min.x, GetCellBounds().min.y, GetCellBounds().min.z);
    }

    public Vector3Int OffsetVector(Vector3Int vector) { 
        return vector - GetArrayOffset();
    }

    public Vector3Int getArraySize()
    {
        return new Vector3Int(cellBounds.size.x, cellBounds.size.y, cellBounds.size.z);
    }

    public bool offsetInArray(Vector3Int offset)
    {
        Vector3Int size = getArraySize();
        return (offset.x <= size.x) && (offset.y <= size.y) && (offset.z <= size.z);
    }

    
}
