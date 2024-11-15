using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public int RoomId { get; set; }


    public int MaxPlayer { get; set; }

    private void Awake()
    {
        MaxPlayer = 1;
    }
}
