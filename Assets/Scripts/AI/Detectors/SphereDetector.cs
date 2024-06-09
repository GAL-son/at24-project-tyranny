using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem.HID;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(SphereCollider))]
public class SphereDetector : MonoBehaviour
{
    Detector detector;
    float coliderLength = 0;

    Vector3 testpos = new();
    // Start is called before the first frame update
    void Start()
    {
        coliderLength = GetComponent<SphereCollider>().radius;
        detector = GetComponentInParent<Detector>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, (testpos - detector.transform.position).normalized * coliderLength, Color.yellow);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && IsInLineOfSight(other.gameObject.transform.position))
        {
            detector.HandleSphereDetection(other.gameObject);
        }
    }

    private bool IsInLineOfSight(Vector3 pos)
    {
        Debug.Log("TEST LINE OF SIGHT");
        RaycastHit hit;
        bool wasPlayerHit = false;
        Vector3 direction = (pos - detector.transform.position).normalized;
        testpos = pos;
        if (Physics.Raycast(detector.transform.position, direction, out hit, 1000))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("HIT SOMETHING: " + hit.collider.tag);
            if (hit.collider.tag == "Player")
            {
                wasPlayerHit = true;
            }
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }

        return wasPlayerHit;
    }
}
