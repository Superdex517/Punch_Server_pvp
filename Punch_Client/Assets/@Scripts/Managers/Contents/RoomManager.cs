using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool IsHost { get; set; }
    public bool IsPlayerReady { get; set; }

    public int WaitingPlayerCount;

    public int HostId { get; set; }
    public int QuestId { get; set; }

    public void CountPlayer(string count)
    {
        FindObjectOfType<UI_WaitingRoom>().GetComponent<UI_WaitingRoom>().CountPlayer(count);
    }

    public void SetHostId()
    {

    }

    public void SetQuestId()
    {

    }
}
