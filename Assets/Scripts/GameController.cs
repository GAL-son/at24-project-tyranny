using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public CameraController CameraController;

    Generator generator = null;
    Spawner spawner= null;
    EnviromentController enviromentController = null;
    TurnController turnController = null;
    EnemyControler enemyControler = null;

    bool update = false;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponentInChildren<Generator>();
        enviromentController = GetComponentInChildren<EnviromentController>();
        spawner = GetComponentInChildren<Spawner>();
        enemyControler = GetComponentInChildren<EnemyControler>();
        turnController = GetComponentInChildren<TurnController>();
        BeginRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BeginRound()
    {
        generator.Generate();
        Vector2Int start = generator.getStart();
        spawner.setStart(new Vector3Int(start.x, start.y));

        if(!update)
        {
            spawner.Spawn();
        } else
        {
            spawner.Respawn();
        }

    }

    public void Restart()
    {
        TurnController.Instance.ForceEndTurn();
        TurnController.Instance.RestartTurns();        
        generator.Clean();
        update = true;
        BeginRound();
    }
}
