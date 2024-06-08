using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject MoveCharacterToPoint(GameObject character, Vector3Int spawnPoint)
    {
        EnviromentController controller = EnviromentController.Instance;
        bool isWalkable = false;
        List<GameObject> cells = controller.GetGameObjectsAt(spawnPoint);
        foreach (GameObject cell in cells)
        {
            Debug.Log("TRER");
            Debug.Log(cell);
            if (cell != null && cell.GetComponent<EnviromentTile>() != null && cell.GetComponent<EnviromentTile>().isWalkable)
            {
                isWalkable = true;
                break;
            }
        }

        if (!isWalkable)
        {
            return null;
        }

        Vector3 targerPosition = controller.getCellCenter(spawnPoint);
        character.transform.position = targerPosition;
        return character;
    }

    public GameObject MoveCharacter(GameObject character)
    {
        EnviromentController controller = EnviromentController.Instance;

        Vector3Int spawnPoint = controller.getRandomCell();

        while (MoveCharacterToPoint(character, spawnPoint) != null) { }

        Vector3 targerPosition = controller.getCellCenter(spawnPoint);
        character.transform.position = targerPosition;

        return character;
    }

    public GameObject Spawn(GameObject character)
    {
        EnviromentController controller = EnviromentController.Instance;

        bool isWalkable = false;
        Vector3Int spawnPoint = controller.getRandomCell();

        while (!isWalkable)
        {
            List<GameObject> cells = controller.GetGameObjectsAt(spawnPoint);
            foreach (GameObject cell in cells)
            {
                if (cell != null && cell.GetComponent<EnviromentTile>() != null && cell.GetComponent<EnviromentTile>().isWalkable)
                {
                    isWalkable = true;
                    break;
                }
            }

            if (!isWalkable)
            {
                spawnPoint = controller.getRandomCell();
            }
        }

        Vector3 targerPosition = controller.getCellCenter(spawnPoint);
        GameObject go = Instantiate(character);
        go.transform.position = targerPosition;

        return go;
    }

    public GameObject SpawnAtDistance(GameObject character, Vector3Int center, float radius)
    {
        EnviromentController controller = EnviromentController.Instance;

        bool isCorrect = false;
        Vector3Int spawnPoint = controller.getRandomCell();

        int counter = 50;

        while (!isCorrect && counter > 0)
        {
            List<GameObject> cells = controller.GetGameObjectsAt(spawnPoint);

            foreach (GameObject cell in cells)
            {
                if (cell != null && cell.GetComponent<EnviromentTile>() != null && cell.GetComponent<EnviromentTile>().isWalkable)
                {
                    if (Vector3Int.Distance(center, spawnPoint) >= radius)
                    {
                        isCorrect = true;
                        break;
                    }
                }
            }

            if (!isCorrect)
            {
                spawnPoint = controller.getRandomCell();
                counter--;
            }
        }

        if(!isCorrect)
        {
            return null;
        }

        Vector3 targerPosition = controller.getCellCenter(spawnPoint);
        GameObject go = Instantiate(character);
        go.transform.position = targerPosition;

        return go;
    }




}
