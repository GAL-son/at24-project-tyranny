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
        }
        if(turnController.isStageAction())
        {
            Debug.Log("STAGE ACTION");
            Debug.Log("isCurrentActionDone " + isCurrentActionDone);
            Debug.Log("HasNextAction " + HasNextAction());
            if (isCurrentActionDone && HasNextAction())
            {
                Debug.Log("THERE IS NEW ACTION");
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
        actionList.Clear();
        actionList.AddRange(actions);
        nextActionIndex = 0;
        isCurrentActionDone = true;
        Debug.Log("ACTIONS TO DO: " + actionList.Count);
        canUpdateActions = false;
    }

    private void PerformAction(Action action)
    {
        isCurrentActionDone = false;
        if(action is MoveAction)
        {            
            Walker walker = gameObject.GetComponent<Walker>();
            walker.OnDoneWalking += ActionDone;

            Debug.Log("WALKER: " + walker);

            MoveAction moveAction = (MoveAction)action;
            walker.SetPath(moveAction.Path);
            Debug.Log("MOVE TO: " + moveAction.ActionTarget);
        }
        if (action is InteractAction)
        {

        }
        if (action is FightAction)
        {

        }
        if(action is ItemAction)
        {

        }
    }

    private void ActionDone()
    {
        isCurrentActionDone = true;
        nextActionIndex++;
    }

    private bool HasNextAction()
    {
        Debug.Log("nextActionIndex" + nextActionIndex);
        Debug.Log("actionList.Coun" + actionList.Count);
        return nextActionIndex < actionList.Count;
    }

    public void ClearActions()
    {
        actionList.Clear();
        isCurrentActionDone= false;
        nextActionIndex= 0;
    }


}
