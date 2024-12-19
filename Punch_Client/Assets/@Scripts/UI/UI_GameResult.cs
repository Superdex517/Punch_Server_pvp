using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using Object = UnityEngine.Object;


public class UI_GameResult : UI_GameScene
{
    enum GameTexts
    {
        ResultText,
    }

    enum GameButtons
    {
        LeaveButton,
    }

    public enum GameResult
    {
        None,
        Win,
        Lose
    }

    GameResult _result = GameResult.None;
    public GameResult Result
    {
        get { return _result; }
        set
        {
            _result = value;
            switch (value)
            {
                case GameResult.None:
                    break;
                case GameResult.Win:
                    GetText((int)GameTexts.ResultText).text = $"Win!";
                    break;
                case GameResult.Lose:
                    GetText((int)GameTexts.ResultText).text = $"Lose!";
                    break;
            }
        }
    }

    protected override void Awake()
    {
        //base.Awake();

        BindButtons(typeof(GameButtons));
        BindTexts(typeof(GameTexts));

        GetButton((int)GameButtons.LeaveButton).onClick.AddListener(() =>
        {
            OnClickLobby();
            C_DestroyWaitingRoom destroyWaitingRoomPacket = new C_DestroyWaitingRoom();
            destroyWaitingRoomPacket.RoomInfo = Managers.MyPlayer.RoomInfo;
            Managers.Network.Send(destroyWaitingRoomPacket);
        });
    }

    private void OnEnable()
    {

    }
}
