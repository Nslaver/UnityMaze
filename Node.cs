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
}