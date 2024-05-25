using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Walker))]

public class Performer : MonoBehaviour
{
    private TurnController turnController;
    private bool isCurrentActionDone = true;
    private int nextActionIndex = 0;
    private bool canUpdateActions = false;

    private List<Action> actionList = new List<Action>();
    // Start is called before the first frame update
    void Start()
    {
        turnController = TurnController.Instance;
        turnController.EndTurnSubscribe(gameObject);
        turnController.OnTurnEnded += ClearActions;        
    }

    // Update is called once per frame
    void Update()
    {
        if(turnController.isStagePlanning())
        {
            canUpdateActions = true;
            canUpdateActions = true;
        }
        if(turnController.isStageAction())
        {
            if(gameObject.tag != "Player")
            {
                // Debug.Log("HasNextAction" + HasNextAction());
            }
            if (isCurrentActionDone && HasNextAction())
            {
                Action action = actionList[nextActionIndex];
                PerformAction(action);
            }

            if(!HasNextAction())
            {
                turnController.EndTurnRequest(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        turnController.EndTurnUnsubscribe(gameObject);
    }

    public void setActions(List<Action> actions)
    {
        if(canUpdateActions)
        {
            actionList.Clear();
            actionList.AddRange(actions);
            nextActionIndex = 0;
            isCurrentActionDone = true;
            canUpdateActions = false;
        }
    }

    private void PerformAction(Action action)
    {
        isCurrentActionDone = false;
        if(action is MoveAction)
        {            
            Walker walker = gameObject.GetComponent<Walker>();
            walker.OnDoneWalking += ActionDone;
            MoveAction moveAction = (MoveAction)action;
            walker.SetPath(moveAction.Path);
        }
        else if (action is InteractAction)
        {

        }
        else if(action is FightAction)
        {

        }
        else if(action is ItemAction)
        {

        }
        else
        {
            ActionDone();
        }
        
    }

    private void ActionDone()
    {
        isCurrentActionDone = true;
        nextActionIndex++;
    }

    private bool HasNextAction()
    {
        return nextActionIndex < actionList.Count;
    }

    public void ClearActions()
    {
        actionList.Clear();
        isCurrentActionDone= false;
        canUpdateActions = true;
        nextActionIndex= 0;
        Debug.Log("CLEAR CAN UPDATE" + HasNextAction());
    }

    public void DoNothing()
    {
        Debug.Log("DO NOTHING");
        ClearActions();
    }


}
