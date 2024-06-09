using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
    public Text turnNumber;
    public Text turnStage;

    private const string TURN_ACTION = "Action";
    private const string TURN_PLAN = "Planning";

    public void setTurnNumber(int turnNumber)
    {
        this.turnNumber.text = turnNumber.ToString();
    }

    public void setTurnStage(TurnController.TurnStage stage) {
        if(stage == TurnController.TurnStage.Action)
        {
            turnStage.text = TURN_ACTION;
        } 
        else
        {
            turnStage.text = TURN_PLAN;
        }
     
    }
}
