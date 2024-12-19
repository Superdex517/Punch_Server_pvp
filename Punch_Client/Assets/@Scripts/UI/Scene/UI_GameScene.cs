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

    enum GameButtons
    {
        MenuButton,
    }

    public UI_GameResult gameResult;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(GameButtons));

        gameResult = GetObject((int)GameObjects.UI_GameResult).GetComponent<UI_GameResult>();
        gameResult.gameObject.SetActive(false);
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
}
