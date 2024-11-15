using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WaitingScene : BaseScene
{


    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        //gameObject.AddComponent<CaptureScreenShot>();
#endif

        Debug.Log("SelectRoom");
        SceneType = EScene.ScelectRoomScene;
    }

    protected override void Start()
    {
        base.Start();


    }

    public override void Clear()
    {

    }

    private void OnApplicationQuit()
    {
        Managers.Network.GameServer.Disconnect();
    }
}
