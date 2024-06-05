using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class AnimationControllerScript : MonoBehaviour
{

    bool isWalking = false;
    bool isRunning = false;
    bool isIdling = false;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        if(isWalking == false)
        {
            isWalking = true;
            isRunning = false;
            isIdling = false;
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
    }

    public void Run()
    {
        if(isRunning == false)
        {
            isWalking = false;
            isRunning = true;
            isIdling = false;
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
    }

    public void Idle()
    {
        isWalking = false;
        isRunning = false;
        isIdling = true;
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
    }
}
