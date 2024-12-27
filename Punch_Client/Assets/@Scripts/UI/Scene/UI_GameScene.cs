using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_GameScene : UI_Scene
{
    enum GameObjects
    {
        UI_GameResult,
    }

    enum GameTexts
    {
        ResultText,
    }

    enum GameButtons
    {
        MenuButton,
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
                    GetText((int)GameTexts.ResultText).text = $"WL";
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
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(GameButtons));
        BindTexts(typeof(GameTexts));

        Result = GameResult.Win;
        GetObject((int)GameObjects.UI_GameResult).SetActive(false);

        GetButton((int)GameButtons.LeaveButton).onClick.AddListener(() =>
        {
            OnClickLobby();
            C_DestroyWaitingRoom destroyWaitingRoomPacket = new C_DestroyWaitingRoom();
            destroyWaitingRoomPacket.RoomInfo = Managers.MyPlayer.RoomInfo;
            Managers.Network.Send(destroyWaitingRoomPacket);
        });
    }

    protected override void Start()
    {
        base.Start();

    }

    protected void OnClickLobby()
    {
        Managers.Room.IsHost = false;
        C_LeaveGame leaveGamePacket = new C_LeaveGame();
        Managers.Network.Send(leaveGamePacket);
        Managers.Scene.LoadScene(EScene.ScelectRoomScene);
        Managers.Room.IsResult = false;
    }

    public void ActiveGameResult()
    {
        GetObject((int)GameObjects.UI_GameResult).SetActive(true);
    }
}
