using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool IsRoomManager { get; set; }
    public bool IsPlayerReady { get; set; }

    public int WaitingPlayerCount;

    public void CountPlayer(string count)
    {
        FindObjectOfType<UI_WaitingRoom>().GetComponent<UI_WaitingRoom>().CountPlayer(count);
    }
}
