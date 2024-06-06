using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngineInternal;

[RequireComponent(typeof(Performer))]
[RequireComponent(typeof(ActionVizualizer))]

public class Planer : MonoBehaviour
{
    public bool displayCurrentAction = true;
    public int planCost = 0;
    private TurnController turnController = null;
    private EnviromentController enviromentController = null;
    private ActionVizualizer actionVizualizer;
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
        Register();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnController.isStagePlanning())
        {
            UpdateActionsVizualization();
        }

        if (turnController.isStageAction())
        {
        }
    }

    public void SaveAction()
    {   
        if(gameObject.tag == "Player")
        {
            Debug.Log("PLAYER CREATES ACTION");
        }
        if(nextAction == null) {
            return;
        }

        //Debug.Log(nextAction.ToString());

        if (actions.Count == 0 || actions.Last().ActionTarget != nextAction.ActionTarget)
        {
            if(planCost + nextAction.Cost <= turnController.GetActionPoints())
            {
                actions.Add(nextAction);
            }
        }
        else
        {
            actions.Remove(actions.Last());
        }
        triggerActionRerender = true;
        RecalculateTotalCost();
    }

    public void SendPlan()
    {
        Performer performer = GetComponent<Performer>();
        if(actions.Count == 0)
        {
            performer.DoNothing();
        }
        else
        {
            performer.setActions(actions);
        }
    }

    private void OnDestroy()
    {
        if(gameObject.tag == "Player")
        {
            Debug.Log("Player planer destroyed");
        }
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

       

        if (path.Count != 0 && startTile != where)
        {
            MoveAction newAction = new MoveAction(path);
            int newCost = newAction.Cost;

            if (planCost + newCost > turnController.GetActionPoints())
            {
                int costDiference = (planCost + newCost) - turnController.GetActionPoints();
                int costPerCell = newCost / path.Count;
                int movesOver = costDiference / costPerCell;

                path.RemoveRange(Math.Max(0, path.Count - movesOver), Math.Min(movesOver, path.Count));

                if (path.Count != 0)
                {
                    newAction = new MoveAction(path);
                }

            }
            nextAction = newAction;
        }
        else
        {
            nextAction = new Action(where);
        }
    }

    private void UpdateActionsVizualization()
    {
        if(displayCurrentAction)
        {
            actionVizualizer.UpdateCurrentVizualization(nextAction);
        }
        if (triggerActionRerender)
        {
            actionVizualizer.RerenderVisualizations(actions);
            triggerActionRerender = false;
        }
    }

    public void ClearPlanedAction()
    {
        nextAction = null;
    }

    private void RecalculateTotalCost()
    {
        int sum = 0;

        foreach (var action in actions)
        {
            sum += action.Cost;
        }

        planCost = sum;
    }

    public void ResetOnTurnEnd()
    {
        Debug.Log("RESET ON END");
        triggerActionRerender = true;
        ClearPlanedAction();
        actions.Clear();
        RecalculateTotalCost();
        UpdateActionsVizualization();
        Debug.Log(actions.Count);
    }

    public void AcceptPlan()
    {
        SendPlan();
        actionVizualizer.RerenderVisualizations(actions);
        actions.Clear();
    }

    public bool isCurrentActionValid()
    {
        return nextAction != null;
    }

    public void Register()
    {
        turnController.OnTurnEnded += ResetOnTurnEnd;
        turnController.OnPlaningEnded += AcceptPlan;
    }
}
