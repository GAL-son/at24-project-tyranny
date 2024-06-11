using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{

    private delegate void pathsDone();
    private event pathsDone OnPathsDone;

    [Header("Map scale")]
    public int scaleX = 50;
    public int scaleY = 50;

    [Header("Paths")]
    public int numberOfPaths = 2;
    public int numberOfSubPaths = 4;
    [Range(0.1f, 1.0f)]
    public float turnTreshold = 0.5f;
    [Range(0.0f, 1.0f)]
    public float pathVariance = 0.5f;
    public int auxiliaryPathCount = 3;
    public int pathSubPoints = 2;

    [Header("Rooms")]
    public int numberOfRooms = 5;
    public int MinRoomSize = 5;
    public int maxRoomSize = 10;
    [Range(0.0f, 1.0f)]
    public float roomSizeVariance = 0.5f;
    [Range(0.0f, 0.7f)]
    public float obstacleChance = 0.2f;


    [Header("Loop Safety")]
    public int failLimit = 50;

    List<List<Vector2Int>> paths = new List<List<Vector2Int>>();

    private Painter painter;
    private int[,] grid;

    List<Vector2Int> walls = new();


    private Vector2Int start;
    private Vector2Int end;


    // Start is called before the first frame update
    void Start()
    {
        painter = GetComponent<Painter>();
        grid = new int[scaleX * 2, scaleY * 2];
    }

    public void Clean()
    {
        grid = new int[scaleX * 2, scaleY * 2];
        paths = new List<List<Vector2Int>>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2Int getStart()
    {
        return start;
    }

    public Vector2Int getEnd()
    {
        return end;
    }

    public void Generate()
    {
        generatePaths();
        FillPaths();
        GenerateRooms();

        painter.paintGrid(grid, new Vector2Int(scaleX * 2, scaleY * 2));
    }

    private void FillPaths()
    {

        // markAround(new Vector2Int(scaleX-1, scaleY-1), Painter.WALL_ID);
        foreach (List<Vector2Int> path in paths)
        {
            foreach (Vector2Int p in path)
            {
                markAround(p, Painter.WALL_ID);
                // Set Floor
                grid[p.x + scaleX, p.y + scaleY] = Painter.FLOOR_ID;
                

            }
        }
    }

    public void FillSpaces()
    {

    }



    private void GenerateDirectPath()
    {
        start = RandomPoint();
        int minDistance = ((scaleX + scaleY) / 2) + ((Math.Abs(start.x) + Math.Abs(start.y)) / 2) - 10;
        end = RandomPointAtDistance(minDistance, start);
        var path = GeneratePath(start, end);
        int counter = 0;
        while (path.Last() != end && counter < failLimit)
        {
            path = GeneratePath(start, end);
            counter++;
        }
        if (counter == failLimit)
        {
            end = path.Last();
        }

        paths.Add(path);

        // Debug.Log("Direct Path: " + path.Count + " spaces, Raw distance" + Vector2Int.Distance(start, end));
    }

    private void generatePaths()
    {
        GenerateDirectPath();
        GenerateRandomPath();
    }

    private void GenerateRandomPath()
    {
        for (int i = 0; i < numberOfPaths; i++)
        {
            Vector2Int tempPoint = RandomPointAtDistance((int)((scaleX + scaleY) / 2 * pathVariance), start);
            List<Vector2Int> path = GeneratePath(start, tempPoint, 15);
            paths.Add(path);

            for (int j = 0; j < pathSubPoints; j++)
            {
                path = GeneratePath(tempPoint, end);
                paths.Add(path);
                tempPoint = RandomPointAtDistance((int)((scaleX + scaleY) / 2 * pathVariance), tempPoint);
            }
        }

        List<List<Vector2Int>> subPaths = new List<List<Vector2Int>>();

        for (int i = 0; i < numberOfSubPaths; i++)
        {
            if (paths.Count <= 1)
            {
                break;
            }

            int firstPathIndex = Random.Range(0, paths.Count - 1);
            int secondPathIndex = Random.Range(0, paths.Count - 1);
            if (firstPathIndex == secondPathIndex)
            {
                continue;
            }

            List<Vector2Int> firstPath = paths[firstPathIndex];
            List<Vector2Int> secondPath = paths[secondPathIndex];
            Vector2Int subStart = firstPath[Random.Range(0, firstPath.Count - 1)];
            Vector2Int subEnd = secondPath[Random.Range(0, secondPath.Count - 1)];

            List<Vector2Int> subPath = GeneratePath(subStart, subEnd);

            if (subPath != null)
            {
                subPaths.Add(subPath);
            }

        }

        paths.AddRange(subPaths);

        //Debug.Log(paths.Count);
    }

    private Vector2Int RandomPoint()
    {
        return new Vector2Int(Random.Range(-scaleX, scaleX), Random.Range(-scaleY, scaleY));
    }

    private Vector2Int RandomPointAtDistance(int distance, Vector2Int from)
    {
        Vector2Int point;
        int counter = -1;
        do
        {
            point = RandomPoint();
            counter++;

        } while (Vector2Int.Distance(point, from) < distance && counter < failLimit);
        return point;
    }

    private List<Vector2Int> GeneratePath(Vector2Int start, Vector2Int end, int turns = 1)
    {
        if (turns == 0)
        {
            turns = 1;
        }
        else
        {
            turns = Math.Abs(turns);
        }

        List<Vector2Int> path = new List<Vector2Int>() { start };
        Vector2Int currentPoint = path.Last();


        int action = 0;
        int counter = 0;
        while (currentPoint != end && counter < failLimit)
        {
            Vector2 dir = new Vector2(end.x - currentPoint.x, end.y - currentPoint.y).normalized;


            if (turns > 1)
            {
                Vector2Int random = RandomPoint();
                dir = (dir + (new Vector2(random.x, random.y) * pathVariance)).normalized;
                turns--;
            }

            if (Mathf.Abs(dir.x) > turnTreshold || Mathf.Abs(dir.y) > turnTreshold)
            {
                action = calculateAction(dir);
            }

            switch (action)
            {
                case 0: // Go UP
                    currentPoint.y += 1;
                    break;
                case 1: // Go RIGHT
                    currentPoint.x += 1;
                    break;
                case 2: // Go DOWN
                    currentPoint.y -= 1;
                    break;
                case 3: // go LEFT
                    currentPoint.x -= 1;
                    break;
            }

            if (IsInBounds(currentPoint, 1) && !path.Contains(currentPoint))
            {
                path.Add(currentPoint);
            }
            else
            {
                counter++;
            }
        }

        //Debug.Log(path.Count);

        return path;

    }

    private bool IsInBounds(Vector2Int point, int offset = 0)
    {
        return point.x > -scaleX + offset && point.y > -scaleY + offset && point.x < scaleX - offset && point.y < scaleY - offset;
    }

    private int calculateAction(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) // Move left to right
        {
            if (dir.x > 0) // Go right
            {
                return 1;
            }
            else // Go left
            {
                return 3;
            }
        }
        else // Move up and down
        {
            if (dir.y > 0) // Go up
            {
                return 0;
            }
            else // Go down
            {
                return 2;
            }
        }
    }

    private void markAround(Vector2Int p, int mark)
    {
        Vector2Int index = new Vector2Int(p.x + scaleX, p.y + scaleY);
        Vector2Int limitX = new Vector2Int(Math.Max(0, index.x - 1), Math.Min(index.x + 1, 2 * scaleX - 1));
        Vector2Int limitY = new Vector2Int(Math.Max(0, index.y - 1), Math.Min(index.y + 1, 2 * scaleY - 1));


        for (int x = limitX.x; x <= limitX.y; x++)
        {
            for (int y = limitY.x; y <= limitY.y; y++)
            {
                if (grid[x, y] == 0)
                {
                    grid[x, y] = mark;
                }
            }
        }
    }

    private void GenerateRooms()
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            List<Vector2Int> originPath = paths[Random.Range(0, paths.Count - 1)];
            Vector2Int origin = originPath[Random.Range(0, originPath.Count - 1)];

            Vector2Int roomSize = new Vector2Int(
                (int)(Random.Range(3, maxRoomSize) * (1 + roomSizeVariance)),
                (int)(Random.Range(3, maxRoomSize) * (1 + roomSizeVariance))
            );

            Vector2Int roomOriginOffset = new Vector2Int(
                Random.Range(0, roomSize.x / 2),
                Random.Range(0, roomSize.y / 2)
            );

            Vector2Int roomXRange = new Vector2Int(
                Math.Max(0, origin.x - roomOriginOffset.x + scaleX),
                Math.Min(origin.x + roomSize.x - roomOriginOffset.x + scaleX, 2 * scaleX - 1)
            );

            Vector2Int roomYRange = new Vector2Int(
                Math.Max(0, origin.y - roomOriginOffset.y + scaleY),
                Math.Min(origin.y + roomSize.y - roomOriginOffset.y + scaleY, 2 * scaleY - 1)
            );

            for (int x = roomXRange.x; x <= roomXRange.y; x++)
            {
                for (int y = roomYRange.x; y <= roomYRange.y; y++)
                {
                    if (y > roomYRange.x && y < roomYRange.y && x > roomXRange.x && x < roomXRange.y)
                    {
                        float randomObstacleChance = Random.Range(0.0f, 1.0f);
                        if (randomObstacleChance < obstacleChance && y > roomYRange.x + 1 && y < roomYRange.y - 1 && x > roomXRange.x + 1 && x < roomXRange.y - 1)
                        {
                            grid[x, y] = Painter.SMALL_WALL_ID;
                        }
                        else
                        {
                            grid[x, y] = Painter.FLOOR_ID;
                        }



                    }
                    else if (grid[x, y] == 0)
                    {
                        grid[x, y] = Painter.WALL_ID;
                    }
                }
            }

        }
    }
}
