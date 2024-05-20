using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngineInternal;

[RequireComponent(typeof(Performer))]
[RequireComponent(typeof(ActionVizualizer))]

public class Planer : MonoBehaviour
{
    [Header("Actions Visualizations")]
    public PathVisualization pathPrefab;

    private TurnController turnController = null;
    private EnviromentController enviromentController = null;
    private ActionVizualizer actionVizualizer;

    private int actionPoints = 100;
    private bool triggerActionRerender = false;
    private List<Action> actions = new List<Action>();
    private Action nextAction;
    private Action lastUpdateAction;

    private Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        turnController = TurnController.Instance;
        enviromentController = EnviromentController.Instance;
        actionVizualizer = gameObject.GetComponent<ActionVizualizer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (turnController.isStagePlanning())
        {
            /*GameObject pointed = GetPointedGameObject();
            if (pointed != null)
            {
                lastUpdateAction = nextAction;
                Vector3Int targetCell = enviromentController.worldGrid.WorldToCell(pointed.transform.position);
                // If pointing at waklable enviroment
                EnviromentTile tile = pointed.GetComponentInParent<EnviromentTile>();
                if (tile != null && tile.isWalkable)
                {
                    PlanMove(targetCell);
                }
                else
                {
                    // If Targtet is somewhere far
                    *//*if (targetCell != actions.Last().ActionTarget)
                    {
                        // Move to that cell
                        PlanMove(targetCell);
                        // And then
                    }*//*
                    // Add new Action
                    // If pointing at enemy

                    // if pointing at interactabe

                    // if pointing at item
                }
            }*/

            // Update action visialization
            UpdateActionsVizualization();

        }

        if (turnController.isStageAction())
        {
            if (actions.Count != 0)
            {
                AcceptPlan();
                actions.Clear();
                actionVizualizer.RerenderVisualizations(actions);
            }
        }
    }

 /*   private GameObject GetPointedGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.transform.gameObject;
        }

        return null;
    }

    public void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }*/

    public void SaveAction()
    {
        if (actions.Count == 0 || actions.Last().ActionTarget != nextAction.ActionTarget)
        {
            actions.Add(nextAction);
        }
        else
        {
            actions.Remove(actions.Last());
        }
        triggerActionRerender = true;
    }

    public void AcceptPlan()
    {
        Performer performer = GetComponent<Performer>();
        performer.setActions(actions);
    }

    public void PlanMove(Vector3Int where)
    {
        // Prepare Move Action
        Vector3Int startTile;
        // If has no actions start from current position
        if (actions.Count == 0)
        {
            startTile = enviromentController.worldGrid.WorldToCell(transform.position);
        }
        else // else start from the final position
        {
            startTile = actions.Last().ActionTarget;
        }

        List<Vector3Int> path = enviromentController.FindPath(startTile, where);

        if (path != null && startTile != where)
        {
            nextAction = new MoveAction(path);
        }
        else
        {
            nextAction = new Action(where);
        }
    }

    private void UpdateActionsVizualization()
    {
        actionVizualizer.UpdateCurrentVizualization(nextAction, lastUpdateAction);
        if (triggerActionRerender)
        {
            actionVizualizer.RerenderVisualizations(actions);
            triggerActionRerender = false;
        }
    }

}
