using UnityEngine;
using System.Collections;


public class Node : TwoDObj
{
    public Node(GameObject prefab, int x, int y) : base(prefab, x, y)
    {
    }

    public override string ToString()
    {
        return "(X:" + x + "Y:" + y + ")";
    }

    public override Vector3 get3dLocation()
    {
        Vector3 ret = base.get3dLocation();
        ret.y = 1f;
        return ret;
    }
}