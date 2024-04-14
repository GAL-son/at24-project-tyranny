using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class VectorExtensions
{ 
    public static Vector3 ClampToBounds(this Vector3 vector3, Bounds bounds , float offset = 0)
    {
        vector3.x = Mathf.Clamp(vector3.x, bounds.min.x - offset, bounds.max.x + offset);
        vector3.y = Mathf.Clamp(vector3.y, bounds.min.y - offset, bounds.max.y + offset);
        vector3.z = Mathf.Clamp(vector3.z, bounds.min.z - offset, bounds.max.z + offset);

        return vector3;
    }
}

