using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualization : MonoBehaviour
{
    public GameObject pathStraight;
    public GameObject pathLeft;
    public GameObject pathRight;
    public GameObject pathTarget;


    private List<Vector3Int> path = new();
    private bool pathUpdated = false;
    private EnviromentController controller;

    private List<GameObject> pathObjects = new List<GameObject>();

    void Start()
    {
        controller = EnviromentController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pathUpdated)
        {
            clearPathObjects();
            RenderPath();
        }
    }

    void OnDestroy()
    {
        clearPathObjects();
    }

    public void updatePath(List<Vector3Int> path)
    {
        this.path = path;
        pathUpdated = false;
    }

    private void RenderPath()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector3Int current = path[i];
            Vector3Int previous = (i > 0) ? path[i - 1] : path[i];
            Vector3Int next = (i < path.Count - 1) ? path[i + 1] : path[i];

            GameObject instance = InstatiateNextPath(previous, current, next);
            if(instance != null)
            {
                pathObjects.Add(instance);
            }
        }
        pathUpdated = true;
    }

    private void clearPathObjects()
    {
        foreach (var item in pathObjects)
        {
            Destroy(item);
        }
    }

    GameObject InstatiateNextPath(Vector3Int previous, Vector3Int current, Vector3Int next)
    {
        Vector3 worldPosition = controller.getCellCenter(current);
        worldPosition.y = 0;

        float yRotaton = 0;
        if (previous == current)
        {
            return null;
        }
        if (next == current)
        {
            return Instantiate(pathTarget, worldPosition, Quaternion.identity);
        }
        bool isStartght = previous.x == next.x || previous.y == next.y;

        // Straight Path
        if (isStartght)
        {
            if (previous.x < next.x)
            {
                yRotaton = 0;
            }
            if (previous.x > next.x)
            {
                yRotaton = 180;
            }

            if (previous.y > next.y)
            {
                yRotaton = 90;
            }
            if (previous.y < next.y)
            {
                yRotaton = 270;
            }

            Quaternion straightRoration = Quaternion.Euler(new Vector3(0, yRotaton + 270, 0));
            return Instantiate(pathStraight, worldPosition, straightRoration);
        }


        GameObject arrow = null;

        if (previous.x < next.x && previous.y < next.y) // Up Right
        {
            if (current.x == next.x) // Left Arrow
            {
                yRotaton = 90;
                arrow = pathLeft;

            }
            else // Right Arrow
            {
                yRotaton = 0;
                arrow = pathRight;
            }
        }
        else if (previous.x < next.x && previous.y > next.y) // Down Right
        {
            if (current.x == next.x) // Right Arrow
            {
                yRotaton = 90;
                arrow = pathRight;
            }
            else // Left Arrow
            {
                yRotaton = 180;
                arrow = pathLeft;

            }
        }
        else if (previous.x > next.x && previous.y < next.y) // Up Left
        {
            if (current.x == next.x) // Right Arrow
            {
                yRotaton = 270;
                arrow = pathRight;

            }
            else // Left Arrow
            {
                yRotaton = 0;
                arrow = pathLeft;

            }
        }
        else if (previous.x > next.x && previous.y > next.y) // Down Left
        {
            if (current.x == next.x) // Left Arrow
            {
                yRotaton = 270;
                arrow = pathLeft;

            }
            else // Left Arrow
            {
                yRotaton = 180;
                arrow = pathRight;
            }
        }

        if (arrow == null)
        {

            return Instantiate(pathStraight, worldPosition, Quaternion.identity);
        }

        Quaternion rotation = Quaternion.Euler(new Vector3(0, yRotaton + 270, 0));

        return Instantiate(arrow, worldPosition, rotation);
    }
}
