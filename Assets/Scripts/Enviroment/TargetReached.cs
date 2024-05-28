using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
public class TargetReached : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTERED TARGET");
        if(other.gameObject.tag == "Player")
        {
            TurnController.Instance.ForceEndTurn();
            TurnController.Instance.RestartTurns();

            Spawner spawner = FindAnyObjectByType<Spawner>();
            spawner.Respawn();            
        }
    }
}
