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
            GameController.Instance.Restart();
        }
    }
}
