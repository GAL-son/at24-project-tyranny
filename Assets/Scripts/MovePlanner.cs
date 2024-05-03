using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Linq;

[RequireComponent(typeof(CharacterWalker))]

public class MovePlanner : MonoBehaviour
{
    private TurnController turnController = null;
    private EnviromentController enviromentController = null;
    private CharacterWalker characterWalker = null;

    public GameObject pathPrefab;
    public GameObject pathTargetPrefab;

    private Vector2 mousePos;
    private Vector3Int pointingCell;

    private List<Vector3Int> pointsGridPostion = new List<Vector3Int>();
    private List<Vector3Int> pathTargets = new List<Vector3Int>();
    private List<GameObject> pathPoints = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        enviromentController = EnviromentController.Instance;
        turnController = TurnController.Instance;
        characterWalker = GetComponent<CharacterWalker>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (turnController.isStagePlanning())
        {
            updatePointingCell();
            List<Vector3Int> path = enviromentController.FindPath(characterWalker.getCurrentCell(), pointingCell);

            Debug.Log("PATH LENGTH"+ path.Count);

            if (path.Count == 0)
            {
                return;
            }

            pointsGridPostion.Clear();
            foreach (GameObject point in pathPoints)
            {
                Destroy(point);
            }
            pathPoints.Clear();

            foreach (Vector3Int point in path)
            {
                pointsGridPostion.Add(point);
                Vector3 worldPos = enviromentController.worldGrid.CellToWorld(point) + enviromentController.worldGrid.cellSize / 2;
                worldPos.y -= enviromentController.worldGrid.cellSize.z / 2;
                GameObject newInstance = Instantiate((point.Equals(path.Last<Vector3Int>()) || pathTargets.Contains(point)) ? pathTargetPrefab : pathPrefab, worldPos, new Quaternion());
                pathPoints.Add(newInstance);
            }
        }
        else
        {
            foreach (GameObject point in pathPoints)
            {
                Destroy(point);
            }
            if(pathTargets.Count > 0)
            {
                clearTargters();
            }
            if(pathPoints.Count > 0)
            {
                pathPoints.Clear();
            }
        }
    }

    public void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
        
    }

    public void updatePointingCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            Vector3 newWorldPosition = hit.point;

            Vector3Int gridPostion = enviromentController.worldGrid.WorldToCell(newWorldPosition);

            foreach (GameObject go in enviromentController.GetGameObjectsAt(gridPostion))
            {
                if (go.GetComponent<EnviromentTile>() != null && go.GetComponent<EnviromentTile>().isWalkable)
                {
                    pointingCell = gridPostion;
                }
            }

        }
    }

    public void PressedLMB(InputAction.CallbackContext context)
    {
        if (turnController.isStagePlanning())
        {
            updatePointingCell();
            if (pathTargets.Contains(pointingCell))
            {
                pathTargets.Remove(pointingCell);
            }
            else
            {
                pathTargets.Add(pointingCell);
            }
            characterWalker.updateGridPath(pointsGridPostion);
        }
    }

    public void clearTargters()
    {
        pathTargets.Clear();
    }
}


