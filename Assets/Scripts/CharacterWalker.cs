using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CharacterWalker : MonoBehaviour
{
    public Tilemap enviroment;
    GameObject[,,] enviroObjects = null;

    // Start is called before the first frame update
    void Start()
    {
        enviroObjects = enviroment.GetArrangedGameObjects();
        BoundsInt bounds = enviroment.GetGameObjectTilemapCellBounds();

        bool picked = false;

        Vector3 startPos = new Vector3();

        while (!picked)
        {
            Vector3Int newStartPos = new Vector3Int(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));

            GameObject obj = enviroObjects[newStartPos.x - bounds.min.x, newStartPos.y - bounds.min.y, newStartPos.z - bounds.min.z];

            if(obj != null && obj.GetComponent<EnviromentTile>() != null ) { 
                EnviromentTile tile = obj.GetComponent<EnviromentTile>();

                if(tile.isWalkable)
                {
                    Debug.Log("GRID START POS"+ newStartPos);
                    startPos = obj.transform.localPosition;
                    Debug.Log("START POS" + startPos);
                    startPos.y = 3; 
                    picked = true;
                }
            }
        }

        transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {

    }







}
