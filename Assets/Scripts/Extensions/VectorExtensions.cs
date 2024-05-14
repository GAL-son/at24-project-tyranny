using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 ClampToBounds(this Vector3 vector3, Bounds bounds, Vector3 offset = new Vector3())
    {
        vector3.x = Mathf.Clamp(vector3.x, bounds.min.x - offset.x, bounds.max.x + offset.x);
        vector3.y = Mathf.Clamp(vector3.y, bounds.min.y - offset.y, bounds.max.y + offset.y);
        vector3.z = Mathf.Clamp(vector3.z, bounds.min.z - offset.z, bounds.max.z + offset.z);

        return vector3;
    }
}    

