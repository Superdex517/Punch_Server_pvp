using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerManager : MonoBehaviour
{
    public MyHeroInfo MyHeroInfo { get; set; }
    public RoomInfo RoomInfo { get; set; }
    public EGameSceneType SceneType { get; set; }
}