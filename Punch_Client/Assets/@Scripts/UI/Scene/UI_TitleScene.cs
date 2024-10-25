using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using Object = UnityEngine.Object;

public class UI_TitleScene : UI_Scene
{
    private enum GameObjects
    {
        StartButton,
    }

    private enum Texts 
    {
        StatusText,
    }
    private enum TitleSceneState
    {
        None,
        AssetLoading,
        AssetLoaded,
        ConnectingToServer,
        ConnectedToServer,
        FailedToConnectToServer,
    }

    TitleSceneState _state = TitleSceneState.None;
    TitleSceneState State
    {
        get { return _state; }
        set
        {
            _state = value;
            switch (value)
            {
                case TitleSceneState.None:
                    break;
                case TitleSceneState.AssetLoading:
                    GetText((int)Texts.StatusText).text = $"로딩중";
                    break;
                case TitleSceneState.AssetLoaded:
                    GetText((int)Texts.StatusText).text = "로딩 완료";
                    break;
                case TitleSceneState.ConnectingToServer:
                    GetText((int)Texts.StatusText).text = "서버 접속중";
                    break;
                case TitleSceneState.ConnectedToServer:
                    GetText((int)Texts.StatusText).text = "서버 접속 성공";
                    break;
                case TitleSceneState.FailedToConnectToServer:
                    GetText((int)Texts.StatusText).text = "서버 접속 실패";
                    break;
            }
        }
    }


    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));

        GetObject((int)GameObjects.StartButton).BindEvent((evt) =>
        {
            Debug.Log("OnClick");
            Managers.Scene.LoadScene(EScene.GameScene);
        });

        GetObject((int)GameObjects.StartButton).gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();

        State = TitleSceneState.AssetLoading;

        Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            GetText((int)Texts.StatusText).text = $"용사 불러오는 중:{key} {count}/{totalCount}";

            if(count == totalCount)
            {
                NetworkConnect();   
            }
        });
    }

    private void NetworkConnect()
    {
        State = TitleSceneState.AssetLoaded;

        Debug.Log("Connecting...");
        State = TitleSceneState.ConnectedToServer;

        IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(iPAddress, 7777);
        Managers.Network.GameServer.Connect(endPoint, ConnectionSuccess, ConnectionFailed);
    }

    private void ConnectionSuccess()
    {
        Debug.Log("Connect to Server");
        State = TitleSceneState.ConnectedToServer;

        GetObject((int)GameObjects.StartButton).gameObject.SetActive(true);
        
    }

    private void ConnectionFailed()
    {
        Debug.Log("Failed to Connect Server");
        State = TitleSceneState.FailedToConnectToServer;
    }
}
