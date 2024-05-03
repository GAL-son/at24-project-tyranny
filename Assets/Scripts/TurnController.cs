using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public static TurnController Instance { get; private set; }

    public TurnStage Stage
    {
        get { return _stage; }
        private set { _stage = value; }
    }

    public void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public enum TurnStage
    {
        Planning,
        Action
    }

    private int _turn = 0;
    private TurnStage _stage = TurnStage.Planning;

    public void nextStage()
    {
        if (_stage == TurnStage.Action)
        {
            _turn++;
        }

        if (_stage == TurnStage.Action)
        {
            _stage = TurnStage.Planning;
        } else
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
}
