using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TargetSpawner))]
[RequireComponent (typeof(CharacterSpawner))]

public class Spawner : MonoBehaviour
{
    [Header("Charachters")]
    public GameObject playerObject;
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [Header("Spawn properties")]
    public float enemiesDistanceFromPlayer = 2;
    public int numberOfEnemies = 5;

    private List<GameObject> enemies = new();
    private GameObject targetInstance = null;

    Vector3Int startPoint;
    Vector3Int endPoint;

    private void Start()
    {
    }

    public void setStart(Vector3Int startPoint)
    {
        this.startPoint = startPoint;
    }

    public void setEnd(Vector3Int endPoint)
    {
        this.endPoint = endPoint;
    }

    public void Spawn()
    {      
        EnviromentController controller = EnviromentController.Instance;
        CharacterSpawner charSpawner = GetComponent<CharacterSpawner>();
        charSpawner.MoveCharacterToPoint(playerObject, startPoint);
        Vector3Int charPosition = controller.worldGrid.WorldToCell(playerObject.transform.position);

        

        TargetSpawner targetSpawner = GetComponent<TargetSpawner>();
        targetInstance = targetSpawner.Spawn(endPoint);

        
        for(int i = 0; i < numberOfEnemies; i++)
        {
            int enemyIndex = Random.Range(0, enemyPrefabs.Count - 1);
            GameObject enemy = charSpawner.SpawnAtDistance(enemyPrefabs[enemyIndex], charPosition, enemiesDistanceFromPlayer);

            if(enemy != null)
            {
                enemies.Add(enemy);
            }
        }
       
    }

    public void Respawn()
    {
        ClearSpawns();
        Spawn();
    }

    public void ClearSpawns()
    {
        targetInstance.SetActive(false);
        Destroy(targetInstance);
        targetInstance = null;
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
            Destroy(enemy);
        }
        enemies.Clear();

        Planer playerPlanner = playerObject.GetComponent<Planer>();
        playerPlanner.Register();
    }
}
