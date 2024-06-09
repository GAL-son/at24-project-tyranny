using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
public class TargetReached : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTERED TARGET");
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(RestartTimer());            
        }
    }

    IEnumerator RestartTimer()
    {
        yield return new WaitForSeconds(1);
        GameController.Instance.Restart();
    }
}
