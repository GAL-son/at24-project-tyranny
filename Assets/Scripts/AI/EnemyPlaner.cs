using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaner : MonoBehaviour
{

    private TurnController turnController = null;
    private EnviromentController enviromentController = null;
    private Planer planer = null;

    private List<Vector3Int> patrolPoints;
    private int currentPatrolPointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        turnController = TurnController.Instance;
        enviromentController = EnviromentController.Instance;
        planer = gameObject.GetComponent<Planer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GeneratePatrolRoute()
    {

    }
}
