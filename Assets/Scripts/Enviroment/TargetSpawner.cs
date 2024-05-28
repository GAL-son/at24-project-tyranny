using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetObject;   

    public GameObject Spawn(Vector3Int playerPosition)
    {
        EnviromentController controller = EnviromentController.Instance;
        if(playerPosition != null)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            while(path.Count <= 0) {
                Vector3Int endPos = controller.getRandomCell();
                path = controller.FindPath(playerPosition, endPos);

                if(path.Count > 0)
                {
                    GameObject go = Instantiate(targetObject);
                    Vector3 finalPos = controller.getCellCenter(endPos);
                    finalPos.y = 0.1f;
                    go.transform.position = finalPos;
                    return go;
                }
            }
        }

        return null;
    }
}
