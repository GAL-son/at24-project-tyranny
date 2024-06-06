using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public delegate void TurnEnded();
    public event TurnEnded OnTurnEnded;

    public delegate void PlaningEnded();
    public event PlaningEnded OnPlaningEnded;

    private const int RegularActionPoints = 50;
    private const int AtackActionPoints = 20;

    public enum TurnStage
    {
        Planning,
        Action
    }

    public enum ActionStageType
    {
        Regular,
        Atack
    }

    public int GetActionPoints()
    {
        if (actionStage == ActionStageType.Atack)
        {
            return AtackActionPoints;
        }
        else
        {
            return RegularActionPoints;
        }
    }

    public void setActionType(ActionStageType type)
    {
        this.actionStage = type;
    }

    public static TurnController Instance { get; private set; }

    private int _turn = 0;
    private TurnStage _stage = TurnStage.Planning;
    private ActionStageType actionStage = ActionStageType.Regular;
    private Dictionary<GameObject, bool> endTurnSubscribers = new();

    public TurnStage Stage
    {
        get { return _stage; }
        private set { _stage = value; }
    }

    public void EndTurnSubscribe(GameObject subscriber)
    {
        if (!endTurnSubscribers.ContainsKey(subscriber))
        {
            //Debug.Log("SUBSCIRBED" + endTurnSubscribers.Count);
            endTurnSubscribers.Add(subscriber, false);
            bool subscirberBool;
            
            //Debug.Log("SUBSCIRBED SUCCESFULL??" + endTurnSubscribers.TryGetValue(subscriber, out subscirberBool));
        }
        
    }

    public void EndTurnUnsubscribe(GameObject subscriber)
    {
        if (endTurnSubscribers.ContainsKey(subscriber))
        {
            endTurnSubscribers.Remove(subscriber);
        }
    }

    public void EndTurnRequest(GameObject subscriber)
    {
        //Debug.Log("REQUEST FROM" + subscriber);
        bool subscirberBool;
        if (endTurnSubscribers.TryGetValue(subscriber, out subscirberBool))
        {
            //Debug.Log("SUBSCRIBER FOUND");
            if (!subscirberBool)
            {
                //Debug.Log("REQUEST ACCEPTED");
                endTurnSubscribers[subscriber] = true;
            }
        }
    }

    public void ForceEndTurn()
    {
        nextStage();
    }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Update()
    {
        if (isStageAction())
        {
            bool canEndTurn = true;
            foreach (var subscriber in endTurnSubscribers.Values)
            {
                ;
                canEndTurn &= subscriber;
            }

            if (canEndTurn)
            {
                nextStage();
            }
        }
    }

    public void nextStage()
    {
        if (isStageAction())
        {
            if (OnTurnEnded != null)
            {
                OnTurnEnded();
            }
            _turn++;
            _stage = TurnStage.Planning;
            ClearEndTrurnRequests();

        }
        else
        {
            if (OnPlaningEnded != null)
            {
                OnPlaningEnded();
            }
            _stage = TurnStage.Action;
        }
    }

    public bool isStagePlanning()
    {
        return _stage == TurnStage.Planning;
    }

    public bool isStageAction()
    {
        return _stage == TurnStage.Action;
    }

    private void ClearEndTrurnRequests()
    {
        Dictionary<GameObject, bool> newEndTurnSubscribers = new();
        foreach (var subscriber in endTurnSubscribers.Keys)
        {
            newEndTurnSubscribers.Add(subscriber, false);
        }
        endTurnSubscribers = new(newEndTurnSubscribers);
    }

    public void endPlaning()
    {
        if (isStagePlanning())
        {
            nextStage();
        }
    }
    public void RestartTurns()
    {
        _turn = 0;
        _stage = TurnStage.Planning;
        actionStage = ActionStageType.Regular;
        OnPlaningEnded = null;
        OnTurnEnded = null;
    }
}
