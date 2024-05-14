using System;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public static class TilemapExtension
{
    public static Bounds GetGameObjectTilemapBounds(this Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.GetGameObjectTilemapCellBounds();
        Bounds localBounds = new Bounds();
        Vector3 cellSize = tilemap.layoutGrid.cellSize;        
        localBounds.SetMinMax(new Vector3(bounds.min.x * cellSize.x, bounds.min.y * cellSize.y, bounds.min.z * cellSize.z)  - (cellSize / 2), new Vector3(bounds.max.x * cellSize.x, bounds.max.y * cellSize.y, bounds.max.z * cellSize.z) + (cellSize / 2));

        return localBounds;

    }

    public static BoundsInt GetGameObjectTilemapCellBounds(this Tilemap tilemap)
    {
        BoundsInt bounds = new BoundsInt();
        bool firstUpdate = true;
        Vector3Int min;
        Vector3Int max;
        Grid parentGrid = tilemap.layoutGrid;

        foreach (Transform child in tilemap.GetComponent<Transform>())
        {
            Vector3Int childCellPostion = parentGrid.WorldToCell(child.localPosition);
            if (!firstUpdate)
            {
                min = new Vector3Int(Math.Min(childCellPostion.x, bounds.min.x), Math.Min(childCellPostion.y, bounds.min.y), Math.Min(childCellPostion.z, bounds.min.z));
                max = new Vector3Int(Math.Max(childCellPostion.x, bounds.max.x), Math.Max(childCellPostion.y, bounds.max.y), Math.Max(childCellPostion.z, bounds.max.z));
            }
            else
            {
                min = childCellPostion;
                max = childCellPostion;
                firstUpdate = false;

            }

            bounds.SetMinMax(min, max);

        }

        return bounds;
    }

    public static GameObject[,,] GetArrangedGameObjects(this Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.GetGameObjectTilemapCellBounds();
        Vector3Int size = bounds.size;
        Grid parentGrid = tilemap.layoutGrid;

        GameObject[,,] objects = new GameObject [size.x+1, size.y+1, size.z+1];

        foreach (Transform child in tilemap.GetComponent<Transform>())
        {
            Vector3Int cellPosition = parentGrid.WorldToCell(child.localPosition);
            objects[cellPosition.x - bounds.min.x, cellPosition.y - bounds.min.y, cellPosition.z - bounds.min.z] = child.gameObject;
        }

        return objects;
    }

    
}
