using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomManager : MonoBehaviour
{
    public bool IsHost { get; set; } = false;
    public string RoomName { get; set; }
    public bool IsPlayerReady { get; set; }

    public int WaitingPlayerCount;

    public int HostId { get; set; }
    public int QuestId { get; set; }

    public bool IsResult { get; set; } = false;

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

    public void GameResult()
    {
        FindObjectOfType<UI_GameScene>().gameResult.gameObject.SetActive(true);
        Debug.Log("END GAEM");
    }
}
