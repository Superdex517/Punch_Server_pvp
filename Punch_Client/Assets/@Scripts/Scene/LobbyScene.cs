using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LobbyScene : BaseScene
{


    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        //gameObject.AddComponent<CaptureScreenShot>();
#endif


    }

    protected override void Start()
    {
        base.Start();

        Debug.Log("SelectRoom");
        SceneType = EScene.ScelectRoomScene;

        C_EnterLobby enterLobby = new C_EnterLobby();
        Managers.Network.Send(enterLobby);
    }

    public override void Clear()
    {

    }

    private void OnApplicationQuit()
    {
        Managers.Network.GameServer.Disconnect();
    }
}
