using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomCard : UI_Base
{
    public RoomInfo RoomInfo { get; set; }
    public int MaxPlayer { get; set; } = 1;
    private UI_WaitingRoom _waitingRoom;

    [SerializeField]
    private EGameUIType _uiType = EGameUIType.Room;

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
            EnterRoom();
        });
    }

    protected override void Start()
    {
        base.Start();

    }

    public void UpdateRoomTitle(string id)
    {
        GetText((int)GameTexts.Room_NumberText).text = "�� ��ȣ: \n" + id;

    }

    public void EnterRoom()
    {
        _waitingRoom.ShowPopup();

        C_EnterWaitingRoom enterWaitingRoom = new C_EnterWaitingRoom();
        enterWaitingRoom.RoomInfo = RoomInfo;
        Managers.Network.Send(enterWaitingRoom);

        //���⼭ �� �뿡 �ִ� �ο� �����ְ�
        //���濡�� �� �����ְ� �ϸ� ��
        if (Managers.Room.IsHost)
        {
            GetText((int)GameTexts.HostId).text = "Host Id ��������";
        }
        else
        {
            GetText((int)GameTexts.QuestId).text = "Quest Id ��������";
        }
    }
}
