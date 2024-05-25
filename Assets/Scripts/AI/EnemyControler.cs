using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public static EnemyControler Instance { get; private set; }
    public List<EnemyPlaner> enemyPlaners;

    private TurnController turnController;

    public int searchTimeoutTurns = 2;
    public int searchRadiusTiles = 4;

    private int currentSearchTurn = 0;   

    private EnviromentController enviromentController;
    Vector3Int pastPosition;
    Vector3Int playerPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        enviromentController = EnviromentController.Instance;
        turnController = TurnController.Instance;
        turnController.OnTurnEnded += UpdateSearch;
    }

    private void Update()
    {

    }

    public void UpdateSearch()
    {
        if (pastPosition != playerPosition)
        {
            playerPosition = pastPosition;
            RaiseAlert();
        }
        else
        {
            currentSearchTurn++;
            if (currentSearchTurn >= searchTimeoutTurns)
            {
                DropAlert();
            }
            else
            {
                SearchArea();
            }
        }
    }

    

    public void AssignToControler(EnemyPlaner planer)
    {
        if(!enemyPlaners.Contains(planer))
        {
            enemyPlaners.Add(planer);
        }
    }

    public void RaiseAlert()
    {
        turnController.setActionType(TurnController.ActionStageType.Atack);
        currentSearchTurn = 0;
        foreach (EnemyPlaner planer in enemyPlaners)
        {
            planer.EnterChase();
            planer.UpdateChaseTargtet(playerPosition);    
        }
    }

    public void DropAlert()
    {
        turnController.setActionType(TurnController.ActionStageType.Regular);
        Debug.Log("DROP ALERT");
        foreach (EnemyPlaner planer in enemyPlaners)
        {
            planer.ExitChase();
        }
    }

    public void EnemyDetected(Vector3 playerPosition)
    {
        pastPosition = enviromentController.worldGrid.WorldToCell(playerPosition);
    }

    public Vector3Int GetLastPlayerPosition()
    {
        return this.playerPosition;
    }

    public void SearchArea()
    {
        foreach (EnemyPlaner planer in enemyPlaners)
        {
            Vector3 playerPos = enviromentController.getCellCenter(playerPosition);
            Vector2 SearchPoint = Random.insideUnitSphere * (searchRadiusTiles / enviromentController.worldGrid.cellSize.x);
            SearchPoint.x += playerPos.x;
            SearchPoint.y += playerPos.z;

            Vector3Int searchTile = enviromentController.worldGrid.WorldToCell(new Vector3(SearchPoint.x, 0.5f, SearchPoint.y));

            Vector3Int planerPos = enviromentController.worldGrid.WorldToCell(planer.transform.position);

            List<Vector3Int> path = enviromentController.FindPath(planerPos, searchTile);

            if(path.Count != 0)
            {
                planer.UpdateChaseTargtet(searchTile);
            }
        }
    }
}
