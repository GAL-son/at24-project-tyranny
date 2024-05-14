using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterWalker))]

public class EnemyAI : MonoBehaviour
{
    private CharacterWalker characterWalker;

    public EnviromentController enviromentController;
    public TurnController turnController;

    private Vector3Int currentTargtet;
    private bool hasTargtet = false;

    private void Awake()
    {
            
    }

    // Start is called before the first frame update
    void Start()
    {
        enviromentController = EnviromentController.Instance;
        turnController = TurnController.Instance;
        characterWalker = gameObject.GetComponent<CharacterWalker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnController.Instance.isStagePlanning() && (!hasTargtet))
        {
            currentTargtet = getRandomWaklableCell();
            List<Vector3Int> path = GenerateWalkerPath(currentTargtet);      

            if (path.Count > 0)
            {
                hasTargtet = true;
                characterWalker.updateGridPath(path);
            }
            Debug.Log("?");
        }
        else if (TurnController.Instance.isStageAction())
        {
            hasTargtet = false;
        }
            
    }

    private List<Vector3Int> GenerateWalkerPath(Vector3Int target)
    {
        Vector3Int curretCell = EnviromentController.Instance.worldGrid.WorldToCell(transform.position);
        Vector3Int targterCell = enviromentController.worldGrid.WorldToCell(target);
        List<Vector3Int> path = enviromentController.FindPath(curretCell, target); // EnviromentController.Instance.FindPath(curretCell, target);
        Debug.Log("AI PATH" + path.Count);

        return path;
    }

    private Vector3Int getRandomWaklableCell()
    {
        Vector3Int target = EnviromentController.Instance.worldGrid.WorldToCell(transform.position);
        Vector3Int newTarget = EnviromentController.Instance.getRandomCell();

        foreach (GameObject go in EnviromentController.Instance.GetGameObjectsAt(newTarget))
        {
            if (go.GetComponent<EnviromentTile>() != null && go.GetComponent<EnviromentTile>().isWalkable)
            {
                target = newTarget;
                break;
            }
        }

        Debug.Log("RAMDOM CELL" + target);
        return target;
    }




}
