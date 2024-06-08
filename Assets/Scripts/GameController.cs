
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
        Vector2Int end = generator.getEnd();
        spawner.setStart(new Vector3Int(start.x, start.y));
        spawner.setEnd(new Vector3Int(end.x, end.y));
        enviromentController.InitAll();

        if (!update)
        {
            Debug.Log("Spawn");
            spawner.Spawn();
        } else
        {
            Debug.Log("RESPAWN");
            spawner.Respawn();
        }

        CameraController.moveTo(new Vector3Int(start.x, 0, start.y));
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
