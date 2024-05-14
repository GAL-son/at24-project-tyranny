using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Walker))]

public class Performer : MonoBehaviour
{
    private TurnController turnController;
    private bool isCurrentActionDone = true;
    private int nextActionIndex = 0;

    private List<Action> actionList = new List<Action>();
    // Start is called before the first frame update
    void Start()
    {
        turnController = TurnController.Instance;
        turnController.EndTurnSubscribe(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(turnController.isStageAction())
        {
            if(isCurrentActionDone && HasNextAction())
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
        actionList.Clear();
        actionList.AddRange(actions);
        nextActionIndex = 0;
        isCurrentActionDone = true;
    }

    private void PerformAction(Action action)
    {
        isCurrentActionDone = false;
        if(action is MoveAction)
        {
            Walker walker = GetComponent<Walker>();
            walker.OnDoneWalking += ActionDone;

            MoveAction moveAction = (MoveAction)action;
            walker.SetPath(moveAction.Path);
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
        return nextActionIndex >= actionList.Count;
    }


}
