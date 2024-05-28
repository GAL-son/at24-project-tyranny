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

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        EnviromentController controller = EnviromentController.Instance;
        CharacterSpawner charSpawner = GetComponent<CharacterSpawner>();
        charSpawner.MoveCharacter(playerObject);
        Vector3Int charPosition = controller.worldGrid.WorldToCell(playerObject.transform.position);

        foreach (GameObject enemy in enemyPrefabs)
        {
            GameObject enemyInstance = charSpawner.SpawnAtDistance(enemy, charPosition, enemiesDistanceFromPlayer);
            enemies.Add(enemyInstance);
        }

        TargetSpawner targetSpawner = GetComponent<TargetSpawner>();

        targetInstance = targetSpawner.Spawn(charPosition);
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
