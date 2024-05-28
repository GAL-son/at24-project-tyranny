using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAnimation : MonoBehaviour
{
    public GameObject targetAnimation;

    private void Awake()
    {
        Instantiate(targetAnimation);
    }

    private void OnDestroy()
    {
        Destroy(targetAnimation);
    }
}
