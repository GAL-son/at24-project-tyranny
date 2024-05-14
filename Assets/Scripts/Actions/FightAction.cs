using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightAction : Action
{
    const int FIGHT_COST = 3;


    // TODO: Change to fightable enemy
    private GameObject enemyToFight;
    public FightAction(GameObject enemy, Vector3Int fightLocation) : this(enemy, FIGHT_COST, fightLocation) {}
    public FightAction(GameObject enemy, int fightCost, Vector3Int fightLocation) : base(fightCost, fightLocation)
    { 
        enemyToFight = enemy;
    }
}
