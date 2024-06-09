
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public CameraController CameraController;
    public Image loadingScreen = null;

    Generator generator = null;
    Spawner spawner= null;
    EnviromentController enviromentController = null;
    TurnController turnController = null;
    EnemyControler enemyControler = null;
    PointsCounter counter = null;

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
        counter = GetComponentInChildren<PointsCounter>();
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
        StartCoroutine(ExitLoading());
    }

    public void Restart()
    {
        EnterLoading();
        TurnController.Instance.ForceEndTurn();
        TurnController.Instance.RestartTurns();        
        generator.Clean();
        update = true;
        BeginRound();
    }

    public void EnterLoading()
    {
        loadingScreen.enabled = true;
    }

    IEnumerator ExitLoading()
    {
        yield return new WaitForSeconds(2);
        loadingScreen.enabled = false;
    }

    public void UpdatePoints(int points)
    {
        counter.UpdatePoints(points);
    }

   
}
