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
    }

    // Update is called once per frame
    void Update()
    {
        if (turnController.isStageAction())
        {
            if (!isAtTarget && isNext)
            {
                Move();
                if(IsAtTarget())
                {
                    transform.position = nextPoint;
                    isAtTarget = true;
                }

            }

            if(isAtTarget && isNext)
            {
                nextCellIndex++;
                UpdateTarget();
            }

            if(isAtTarget && !isNext)
            {
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
        if (isNext) {
            nextPoint = enviromentController.getCellCenter(path[nextCellIndex]);
            isAtTarget = false;
        }
    }

    private void Move()
    {
        Vector3 direction = (nextPoint - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private bool IsAtTarget()
    {
        return Vector3.Distance(nextPoint, transform.position) < PRECISION;
    }

    private bool IsNextTargtet()
    {
        return nextCellIndex < path.Count;
    }


}
