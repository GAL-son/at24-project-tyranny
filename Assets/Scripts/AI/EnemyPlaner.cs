using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaner : MonoBehaviour
{
    public int patrolPointsCount = 4;
    private TurnController turnController = null;
    private EnviromentController enviromentController = null;
    private Planer planer = null;

    private List<Vector3Int> patrolPoints = new();
    private int currentPatrolPointIndex = 0;
    bool isChasing = false;
    bool canPlan = true;

    // Start is called before the first frame update
    void Start()
    {
        turnController = TurnController.Instance;
        turnController.OnTurnEnded += onTurnEnd;

        enviromentController = EnviromentController.Instance;
        planer = gameObject.GetComponent<Planer>();
        GeneratePatrolRoute();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnController.isStagePlanning() && canPlan)
        {
            if (isChasing)
            {

            }
            else
            {
                updatePatrolLoop();
            }
            canPlan = false;
        }
    }

    public void onTurnEnd()
    {
        canPlan = true;
    }

    private void GeneratePatrolRoute()
    {
        Vector3Int patrolPoint = enviromentController.worldGrid.WorldToCell(transform.position);
        patrolPoints.Add(patrolPoint);

        for(int i = 1; i < patrolPointsCount;) 
        {
            patrolPoint = enviromentController.getRandomCell();
            EnviromentTile tile = enviromentController.GetComponentAtCell<EnviromentTile>(patrolPoint);

            if (tile != null)
            {
                List<Vector3Int> path = enviromentController.FindPath(patrolPoint, patrolPoints[patrolPoints.Count - 1]);

                if (path.Count > 0)
                {
                    patrolPoints.Add(patrolPoint);
                    i++;
                }
            }            
        }
    }

    private void updatePatrolLoop()
    {

        Vector3Int currentLocation = enviromentController.worldGrid.WorldToCell(transform.position);

        if (currentLocation == CurrentPathTarget())
        {
            nextPatrolPoint();
        }

        UpdatePatrolPath();
    }

    private void nextPatrolPoint()
    {
        currentPatrolPointIndex++;
        if(currentPatrolPointIndex >= patrolPoints.Count)
        {
            currentPatrolPointIndex = 0;
        }
    }

    private void UpdatePatrolPath()
    {
        Debug.Log("UPDATE PATROL PATH");
        planer.PlanMove(CurrentPathTarget());

        if(planer.isCurrentActionValid())
        {
            planer.SaveAction();
        }
    }

    private Vector3Int CurrentPathTarget()
    {
        return patrolPoints[currentPatrolPointIndex];
    }
}
