using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static Define;

public class MoveManager
{
    public Dictionary<Vector3, BaseObject> obj = new Dictionary<Vector3, BaseObject>();

    public BaseObject GetObject(Vector3Int cellPos)
    {
        obj.TryGetValue(cellPos, out BaseObject value);
        return value;
    }


    public bool MoveTo(BaseObject obj, Vector3 pos, bool forceMove = false)
    {
        obj.SetPosition(pos, forceMove);
        return true;
    }

    public void ClearObjects()
    {
        obj.Clear();
    }
}