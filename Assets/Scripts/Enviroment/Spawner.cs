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
        Debug.Log("SPAWN");
        // charSpawner.MoveCharacterToPoint(playerObject, startPoint);
        // Vector3Int charPosition = controller.worldGrid.WorldToCell(playerObject.transform.position);

        // TargetSpawner targetSpawner = GetComponent<TargetSpawner>();

        // targetInstance = targetSpawner.Spawn(charPosition);

        return;

       /* foreach (GameObject enemy in enemyPrefabs)
        {
            GameObject enemyInstance = charSpawner.SpawnAtDistance(enemy, charPosition, enemiesDistanceFromPlayer);
            enemies.Add(enemyInstance);
        }*/
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

        Debug.Log(enemies.Count);
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
