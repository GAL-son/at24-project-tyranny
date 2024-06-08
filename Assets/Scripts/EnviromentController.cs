using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class EnviromentController : MonoBehaviour
{
    public static EnviromentController Instance { get; private set; }

    public Grid worldGrid;
    public Bounds worldBounds;
    private List<TilemapGameObjectManager> enviromentTileMaps;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        enviromentTileMaps = new List<TilemapGameObjectManager>();

        foreach (Transform tilemap in worldGrid.GetComponent<Transform>())
        {
            TilemapGameObjectManager tm = tilemap.GetComponent<TilemapGameObjectManager>();

            if (tm != null)
            {
                enviromentTileMaps.Add(tm);
            }
        };
    }

    private void Start()
    {
       
    }


    public List<GameObject> GetGameObjectsAt(Vector3Int cell)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (TilemapGameObjectManager tm in enviromentTileMaps)
        {
            GameObject gameObject = tm.GetObjectAtCell(cell);

            if (gameObject != null)
            {
                list.Add(gameObject);
            }
        }

        return list;
    }

    public BoundsInt GetTotalCellBounds()
    {
        if (enviromentTileMaps == null || enviromentTileMaps.Count == 0)
        {
            return new BoundsInt();
        }

        BoundsInt cellBounds = enviromentTileMaps[0].GetCellBounds();


        foreach (TilemapGameObjectManager tm in enviromentTileMaps)
        {
            BoundsInt tmCellBounds = tm.GetCellBounds();

            Vector3Int newMin = new Vector3Int(Math.Min(cellBounds.min.x, tmCellBounds.min.x), Math.Min(cellBounds.min.y, tmCellBounds.min.y), Math.Min(cellBounds.min.z, tmCellBounds.min.z));
            Vector3Int newMax = new Vector3Int(Math.Min(cellBounds.max.x, tmCellBounds.max.x), Math.Min(cellBounds.max.y, tmCellBounds.max.y), Math.Min(cellBounds.max.z, tmCellBounds.max.z));

            cellBounds.SetMinMax(newMin, newMax);
        }

        return cellBounds;
    }

    public Bounds GetTotalWorldBounds()
    {
        if (enviromentTileMaps.Count == 0)
        {
            return new Bounds();
        }

        Bounds worldBounds = enviromentTileMaps[0].GetWorldBounds();

        foreach (TilemapGameObjectManager tm in enviromentTileMaps)
        {
            Bounds tmWorldBounds = tm.GetWorldBounds();

            Vector3 newMin = new Vector3(Mathf.Min(worldBounds.min.x, tmWorldBounds.min.x), Mathf.Min(worldBounds.min.y, tmWorldBounds.min.y), Mathf.Min(worldBounds.min.z, tmWorldBounds.min.z));
            Vector3 newMax = new Vector3(Mathf.Min(worldBounds.max.x, tmWorldBounds.max.x), Mathf.Min(worldBounds.max.y, tmWorldBounds.max.y), Mathf.Min(worldBounds.max.z, tmWorldBounds.max.z));

            worldBounds.SetMinMax(newMin, newMax);
        }

        return worldBounds;
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        if(start == target)
        {
            return path;
        }

        TilemapGameObjectManager walkables = null;
        // Select waklable Tilemap
        foreach (TilemapGameObjectManager tilemap in enviromentTileMaps)
        {
            if (tilemap.gameObject.tag == "Walkables")
            {
                walkables = tilemap;
                break;
            }
        }

        if (walkables == null)
        {
            return path;
        }

        Vector3Int arraySize = walkables.getArraySize();
        
        int[,,] costs = new int[arraySize.x + 1, arraySize.y + 1, arraySize.z + 1];

        Vector3Int startCellOffset = walkables.OffsetVector(start);
        Vector3Int targetCellOffset = walkables.OffsetVector(target);

        if (!walkables.offsetInArray(startCellOffset) || !walkables.offsetInArray(targetCellOffset))
        {
            return path;
        }

        costs = ExploreTilemap(walkables, costs, startCellOffset, targetCellOffset);

        if (costs[targetCellOffset.x, targetCellOffset.y, targetCellOffset.z] == 0)
        {
            return path;
        }

        path = convertCosts(costs, startCellOffset, targetCellOffset, walkables.GetArrayOffset());
        path.Add(start);
        path.Reverse();

        return path;
    }

    private int[,,] ExploreTilemap(TilemapGameObjectManager tilemap, int[,,] cost, Vector3Int startTile, Vector3Int targetTile)
    {
        List<Vector3Int> openList = new List<Vector3Int>() {
           startTile
        };
        List<Vector3Int> closedList = new();

        cost[startTile.x, startTile.y, startTile.z] = Math.Abs(startTile.x - targetTile.x) + Math.Abs(startTile.y - targetTile.y);

        while (openList.Count > 0)
        {
            // Get node with smalest cost
            Vector3Int current = openList[0];
            
            foreach (var tile in openList)
            {
                int tileCost = cost[tile.x, tile.y, tile.z];
                int newlow = cost[current.x, current.y, current.z];

                if (tileCost < newlow)
                {
                    current = tile;
                }
            }


            if (current == targetTile)
            {
                break;
            }

            openList.Remove(current);
            closedList.Add(current);

            List<Vector3Int> neighbours = new List<Vector3Int>() {
                new Vector3Int(current.x + 1, current.y, current.z),
                new Vector3Int(current.x - 1, current.y, current.z),
                new Vector3Int(current.x, current.y+1, current.z),
                new Vector3Int(current.x, current.y-1, current.z)
            };
            int currentCost = cost[current.x, current.y, current.z];
            foreach (var tile in neighbours)
            {
                if (
                    closedList.Contains(tile) ||
                    !isTileWaklable(tile, tilemap)
                )
                {
                    continue;
                }

                int newcost = currentCost + 1 + Math.Abs(tile.x - targetTile.x) + Math.Abs(tile.y - targetTile.y); ;
                int currentTileCost = cost[tile.x, tile.y, tile.z];

                if(currentTileCost == 0 || currentTileCost > newcost)
                {
                    cost[tile.x, tile.y, tile.z] = newcost;
                    if (!openList.Contains(tile))
                    {
                        openList.Add(tile);
                    }
                }
            }            
        }

        return cost;
    }

    private bool isTileWaklable(Vector3Int index, TilemapGameObjectManager manager)
    {
        if(!manager.isIndexCorrect(index))
        {
            return false;
        }
        var go = manager.GetObjectAtIndex(index);

        return go != null && go.GetComponent<EnviromentTile>().isWalkable;
    }

    public List<Vector3Int> convertCosts(int[,,] cost, Vector3Int startOffset, Vector3Int targetOffset, Vector3Int offset)
    {
        List<Vector3Int> path = new();

        // Start from end of subpath
        Vector3Int current = targetOffset;
        // Calculate path from Costs
        while (true)
        {
            // It means that we walked to start and path is complete
            if (current.Equals(startOffset))
            {
                // Debug.Log("RETURNED TO START");
                break;
            }

            float currentCost = cost[current.x, current.y, current.z];

            List<Vector3Int> neighbourCells = new List<Vector3Int>
                {
                    new Vector3Int(current.x + 1, current.y, current.z),
                    new Vector3Int(current.x - 1, current.y, current.z),
                    new Vector3Int(current.x, current.y+1, current.z),
                    new Vector3Int(current.x, current.y-1, current.z)
                };

            float mincost = currentCost;
            Vector3Int nextCell = new Vector3Int();
            bool isNext = false;

            foreach (Vector3Int neighbourCell in neighbourCells)
            {
                float cellCost;
                try
                {
                    cellCost = cost[neighbourCell.x, neighbourCell.y, neighbourCell.z];
                } catch (Exception e)
                {
                    continue;
                }

                if (cellCost != 0 && cellCost < mincost)
                {
                    mincost = cellCost;
                    isNext = true;
                    nextCell = neighbourCell;
                }
            }

            if (isNext)
            {
                path.Add(current + offset);
                current = nextCell;
            }
            else
            {
                path = new List<Vector3Int>();
                return path;
            }
        }

        return path;
    }


    public Vector3Int getRandomCell()
    {
        BoundsInt walkableBounds = GetTotalCellBounds();

        Vector3Int randomCell = new Vector3Int(
                UnityEngine.Random.Range(walkableBounds.min.x, walkableBounds.max.x),
                UnityEngine.Random.Range(walkableBounds.min.y, walkableBounds.max.y),
                UnityEngine.Random.Range(walkableBounds.min.z, walkableBounds.max.z)
            );

        return randomCell;
    }

    private TilemapGameObjectManager getWalkablesTMGOM()
    {
        TilemapGameObjectManager walkables = null;
        foreach (TilemapGameObjectManager tilemap in enviromentTileMaps)
        {
            if (tilemap.gameObject.tag == "Walkables")
            {
                walkables = tilemap;
                break;
            }
        }

        return walkables;
    }

    public bool IsInEnviroment(Vector3 point)
    {
        return worldBounds.Contains(point);
    }

    public bool IsCellInBounds(Vector3Int cell)
    {
        return worldBounds.Contains(worldGrid.CellToWorld(cell));
    }

    public Vector3 getCurrentCellCenter(Vector3 point)
    {
        var cell = worldGrid.WorldToCell(point);

        return worldGrid.CellToWorld(cell) + worldGrid.cellSize / 2;
    }

    public Vector3 getCellCenter(Vector3Int cell)
    {
        return worldGrid.CellToWorld(cell) + worldGrid.cellSize / 2;        
    }

    public C GetComponentAtCell<C>(Vector3Int cell) where C: Component
    {
        C component = null;

        List<GameObject> objects = GetGameObjectsAt(cell);

        foreach (GameObject obj in objects)
        {
            component = obj.GetComponent<C>();
            if(component != null)
            {
                break;
            }
        }

        return component;
    }

    public void InitAll()
    {
        foreach(TilemapGameObjectManager tilemapGameObjectManager in enviromentTileMaps)
        {
            tilemapGameObjectManager.Init();
        }
    }


}


