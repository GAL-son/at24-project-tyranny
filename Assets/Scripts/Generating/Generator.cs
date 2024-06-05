using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    [Header("Map scale")]
    public int scaleX = 50;
    public int scaleY = 50;

    [Header("Paths")]
    public int numberOfPaths = 2;
    public int numberOfSubPaths = 4;


    [Header("Loop Safety")]
    public int failLimit = 50;

    List<List<Vector2Int>> paths = new List<List<Vector2Int>>();

    private Painter painter; 
    private int[,] grid;


    private Vector2Int start;
    private Vector2Int end;


    // Start is called before the first frame update
    void Start()
    {
        painter = GetComponent<Painter>();
        grid = new int[scaleX*2, scaleY*2];
        Debug.Log("DO PAINT");
        Generate();
        Debug.Log("DO PAINT");
        painter.paintGrid(grid, new Vector2Int(scaleX*2, scaleY*2));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Generate()
    {
       generatePaths();
       FillPaths();
    }

    private void FillPaths()
    {
        Debug.Log("PATHS" + paths.Count);
        foreach (List<Vector2Int> path in paths)
        {
            Debug.Log("steps" + path.Count);
            foreach (Vector2Int p in path)
            {
                //Debug.Log("MARK PATH" + (p.x + scaleX) + " " + (p.y + scaleY));
                grid[p.x + scaleX, p.y + scaleY] = Painter.FLOOR_ID;
                //Debug.Log("?PATH" + grid[p.x + scaleX, p.y + scaleY]);
            }
        }
    }

    public void FillSpaces()
    {

    }

    private void GenerateDirectPath()
    {
        start = RandomPoint();
        int minDistance = ((scaleX + scaleY) / 2) + ((Math.Abs(start.x) + Math.Abs(start.y)) / 2) - 2;
        end = RandomPointAtDistance(minDistance, start);
        var path = GeneratePath(start, end);
        if (path != null)
        {
            paths.Add(path);
        }
    }

    private void generatePaths()
    {

        for (int i = 0; i < numberOfPaths; i++)
        {
            Vector2Int tempPoint = RandomPointAtDistance(12 ,start);
            var path = GeneratePath(start, tempPoint);
            if (path != null)
            {
                paths.Add(path);
            }

            path = GeneratePath(tempPoint, end);
            if (path != null)
            {
                paths.Add(path);
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

        Debug.Log(paths.Count);
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


        int counter = 0;
        while (currentPoint != end && counter < failLimit)
        {
            currentPoint = path.Last();
            int action = 0;
            Vector2 dir = new Vector2(end.x - currentPoint.x, end.y - currentPoint.y).normalized;

            if(turns > 1)
            {
                dir = (dir + RandomPoint()).normalized;
            }

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) // Move left to right
            {
                if (dir.x > 0) // Go right
                {
                    action = 1;
                }
                else // Go left
                {
                    action = 3;
                }
            }
            else // Move up and down
            {
                if (dir.y > 0) // Go up
                {
                    action = 0;
                }
                else // Go down
                {
                    action = 2;
                }
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

            if (IsInBounds(currentPoint) && !path.Contains(currentPoint))
            {
                path.Add(currentPoint);
            }
            else
            {
                counter++;
            }
        }

        if (path.Last() != end)
        {
            return null;
        }

        Debug.Log(path.Count);

        return path;

    }

    private bool IsInBounds(Vector2Int point)
    {
        return point.x > -scaleX && point.y > -scaleY && point.x < scaleX && point.y < scaleY;
    }
}
