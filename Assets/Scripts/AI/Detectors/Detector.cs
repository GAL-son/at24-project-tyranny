using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Painter))]
public class Detector : MonoBehaviour
{
    public delegate void SphereDetection(GameObject player);
    public event SphereDetection OnSphereDetection;

    public delegate void BoxDetection(GameObject player);
    public event BoxDetection OnBoxDetection;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleBoxDetection(GameObject player)
    {
        if (OnBoxDetection != null)
        {
            OnBoxDetection(player);
        }
    }
}
