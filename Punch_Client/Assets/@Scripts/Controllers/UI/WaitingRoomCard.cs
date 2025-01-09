using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomCard : UI_Base
{
    [SerializeField]
    public RoomInfo RoomInfo { get; set; }
    public int MaxPlayer { get; set; } = 1;
    private UI_WaitingRoom _waitingRoom;

    [SerializeField]
    private EGameUIType _uiType = EGameUIType.Waitingroom;

    private enum GameTexts
    {
        RoomTitle,
        Room_NumberText,
        HostId,
        QuestId,
    }

    private enum GameButton
    {
        UI_RoomCard,
    }

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(GameTexts));
        BindButtons(typeof(GameButton));

        _waitingRoom = FindObjectOfType<UI_WaitingRoom>();

        this.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            EnterRoom(RoomInfo);
        });
    }

    protected override void Start()
    {
        base.Start();

        Debug.Log($"{RoomInfo.RoomId}, {RoomInfo.RoomName}"); 
    }

    public void UpdateRoomTitle(string id)
    {
        GetText((int)GameTexts.Room_NumberText).text = "Title: \n" + id;

    }

    public void EnterRoom(RoomInfo info)
    {
        _waitingRoom.ShowPopup();

        C_EnterWaitingRoom enterWaitingRoom = new C_EnterWaitingRoom();
        
        enterWaitingRoom.RoomInfo = info;
        
        Managers.MyPlayer.RoomInfo = info;
        
        Managers.Network.Send(enterWaitingRoom);
    }
}
