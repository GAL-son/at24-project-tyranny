using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public delegate void DoneWalking();
    public event DoneWalking OnDoneWalking;

    public float moveSpeed = 0.5f;

    private TurnController turnController;
    private EnviromentController enviromentController;
    private AnimationControllerScript acs;

    const float PRECISION = 0.05f;

    private List<Vector3Int> path;
    private int nextCellIndex;
    private Vector3 nextPoint;
    private bool isAtTarget;
    private bool isNext;

    void Start()
    {
        turnController = TurnController.Instance;
        enviromentController = EnviromentController.Instance;
        turnController.OnTurnEnded += AlignToCurrent;
        acs = GetComponentInChildren<AnimationControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnController.isStageAction())
        {
            if (!isAtTarget && isNext)
            {
                Move();
                if (IsAtTarget())
                {
                    AlignToCurrent();
                    isAtTarget = true;
                }

            }

            if (isAtTarget && isNext)
            {
                Move();
                nextCellIndex++;
                UpdateTarget();
            }

            if (isAtTarget && !isNext)
            {
                acs.Idle();

                if (OnDoneWalking != null)
                {
                    OnDoneWalking();
                    OnDoneWalking = null;
                }
            }
        }
    }

    public void SetPath(List<Vector3Int> path)
    {
        this.path = path;
        nextCellIndex = 0;
        isAtTarget = false;
        isNext = true;
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        isNext = IsNextTargtet();
        if (isNext)
        {
            nextPoint = enviromentController.getCellCenter(path[nextCellIndex]);
            isAtTarget = false;
        }
    }

    private void Move()
    {
        acs.Walk();

        if(nextPoint != transform.position)
        {
            Vector3 direction = (nextPoint - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 30f);
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

      
    }


    private bool IsAtTarget()
    {
        return Vector3.Distance(nextPoint, transform.position) < PRECISION;
    }

    private bool IsNextTargtet()
    {
        return nextCellIndex < path.Count;
    }
    private void AlignToCurrent()
    {
        acs.Idle();
        transform.position = nextPoint;
    }


}
