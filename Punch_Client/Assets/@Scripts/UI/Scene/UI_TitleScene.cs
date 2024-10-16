using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UI_TitleScene : MonoBehaviour
{
    private void Start()
    {
        NetworkConnect();   
    }

    private void NetworkConnect()
    {
        Debug.Log("Connecting...");

        IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(iPAddress, 7777);
        Managers.Network.GameServer.Connect(endPoint, ConnectionSuccess, ConnectionFailed);
    }

    private void ConnectionSuccess()
    {
        Debug.Log("Connect to Server");
        
        StartCoroutine(CoPacketTest());
    }

    private void ConnectionFailed()
    {
        Debug.Log("Failed to Connect Server");

    }

    IEnumerator CoPacketTest()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            C_Test pkt = new C_Test();
            pkt.Temp = 1;
            Managers.Network.Send(pkt);
        }
    }
}
