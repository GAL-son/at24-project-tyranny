using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class CharacterWalker : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    Vector3 targetLocation;
    bool isAtTargetLocation = true;
    int gridTarget;
    private EnviromentController enviromentController;
    private TurnController turnController;

    List<Vector3Int> gridPath = new List<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        enviromentController = EnviromentController.Instance;
        turnController = TurnController.Instance;
        turnController.EndTurnSubscribe(gameObject);

        transform.position = enviromentController.getCellCenter(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (turnController.isStageAction() && !isAtTargetLocation)
        {
            Vector3 dir = (targetLocation - transform.position).normalized;

            transform.position += dir * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(targetLocation, transform.position) < 0.05f)
            {
                if (gridTarget == gridPath.Count - 1)
                {
                    isAtTargetLocation = true;
                    gridPath.Clear();
                    turnController.EndTurnRequest(gameObject);
                }
                else
                {
                    gridTarget++;
                    UpdateTargetLocation();
                }
            }
        }
    }

    private void UpdateTargetLocation()
    {
        if(gridTarget >= gridPath.Count)
        {
            return;
        }

        targetLocation = enviromentController.worldGrid.CellToWorld(gridPath[gridTarget]) + EnviromentController.Instance.worldGrid.cellSize /2 ;
    }

    public Vector3Int getCurrentCell()
    {
        return enviromentController.worldGrid.WorldToCell(transform.position);
    }

    public void updateGridPath(List<Vector3Int> newGridPath)
    {
        gridPath = new List<Vector3Int>(newGridPath);
        gridTarget = 0;
        UpdateTargetLocation();
        isAtTargetLocation=false;
    }








}
