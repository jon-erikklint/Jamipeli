using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorColor {
    public static Color VectorToColor(Vector4 vec)
    {
        return new Color(vec.x, vec.y, vec.z, vec.w);
    }

    public static Vector4 ColorToVector(Color col)
    {
        return new Vector4(col.r, col.g, col.b, col.a);
    }
}
