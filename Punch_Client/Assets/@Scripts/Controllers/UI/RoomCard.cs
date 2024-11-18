using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCard : UI_Base
{
    public int RoomId { get; set; }

    public int MaxPlayer { get; set; } = 1;

    [SerializeField]
    private EGameUIType _uiType = EGameUIType.Room;

    private enum GameTexts
    {
        RoomTitle,
        Room_NumberText,
    }

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(GameTexts));

    }

    protected override void Start()
    {
        base.Start();

        GetText((int)GameTexts.Room_NumberText).text = "¹æ ¹øÈ£: \n" + RoomId;
    }
}
