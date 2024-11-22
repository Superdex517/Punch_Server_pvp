using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomCard : UI_Base
{
    public int RoomId { get; set; }
    public string RoomName { get; set; }
    public int MaxPlayer { get; set; } = 1;

    [SerializeField]
    private EGameUIType _uiType = EGameUIType.Room;

    private enum GameTexts
    {
        RoomTitle,
        Room_NumberText,
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

        GetButton((int)GameButton.UI_RoomCard).onClick.AddListener(() =>
        {
            EnterRoom();
        });
    }

    protected override void Start()
    {
        base.Start();

    }

    public void UpdateRoomTitle(string id)
    {
        GetText((int)GameTexts.Room_NumberText).text = "¹æ ¹øÈ£: \n" + id;

    }

    public void EnterRoom()
    {
        C_EnterWaitingRoom enterWaitingRoom = new C_EnterWaitingRoom();

        Managers.Network.Send(enterWaitingRoom);
    }
}
