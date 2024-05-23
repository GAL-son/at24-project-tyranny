using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxDetector : MonoBehaviour
{
    Detector detector;
    // Start is called before the first frame update
    void Start()
    {
        detector = GetComponentInParent<Detector>();
        Debug.Log(detector);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLISION ENTER");
        if(other.tag == "Player")
        {
            detector.HandleBoxDetection(other.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }
}
