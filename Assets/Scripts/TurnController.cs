using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public enum TurnStage
    {
        Planning,
        Action
    }

    public static TurnController Instance { get; private set; }

    private int _turn = 0;
    private TurnStage _stage = TurnStage.Planning;
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
            endTurnSubscribers.Add(subscriber, false);
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
        bool subscirberBool;
        if (endTurnSubscribers.TryGetValue(subscriber, out subscirberBool))
        {
            if(!subscirberBool)
            {
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
            foreach (var subscriber in endTurnSubscribers.Keys)
            {
                bool canSubscriberEnd = endTurnSubscribers[subscriber];

                canEndTurn &= canSubscriberEnd;
            }

            if(canEndTurn)
            {
                nextStage();
                // ClearEndTrurnRequests();
            }
        }
    }

    public void nextStage()
    {
        if (_stage == TurnStage.Action)
        {
            _turn++;
            _stage = TurnStage.Planning;
            ClearEndTrurnRequests();
        }
        else
        {
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
            newEndTurnSubscribers.Add(subscriber, false );
        }
        endTurnSubscribers = new(newEndTurnSubscribers);
    }
}
