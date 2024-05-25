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

    // Start is called before the first frame update

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tiles = tilemap.GetArrangedGameObjects();
        cellBounds = tilemap.GetGameObjectTilemapCellBounds();
        worldBounds = tilemap.GetGameObjectTilemapBounds();

    }
    void Start()
    {
        
    }

    public GameObject GetObjectAtCell(Vector3Int cell)
    {
        Vector3Int cellOffset = OffsetVector(cell);
        if(!offsetInArray(cellOffset))
        {
            return null;
        }

        return tiles[cellOffset.x, cellOffset.y, cellOffset.z]; ;
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

    public BoundsInt GetCellBounds()
    {
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
