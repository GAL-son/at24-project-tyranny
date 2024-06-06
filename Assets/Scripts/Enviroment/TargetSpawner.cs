using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetObject;   

    public GameObject Spawn(Vector3Int position)
    {
        EnviromentController controller = EnviromentController.Instance;
        GameObject go = Instantiate(targetObject);
        Vector3 finalPos = controller.getCellCenter(position);
        finalPos.y = 0.1f;
        go.transform.position = finalPos;
        return go;       
    }
}
