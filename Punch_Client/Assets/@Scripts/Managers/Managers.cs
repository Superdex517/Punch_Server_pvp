using System;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static bool Initialized { get; set; }

    public static Managers s_instance;
    public static Managers Instance { get { Init(); return s_instance; } }

    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private NetworkManager _network = new NetworkManager();

    public static PoolManager Pool { get { return Instance?._pool; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static NetworkManager Network { get { return Instance?._network; } }

    public static void Init()
    {
        if(s_instance == null && Initialized == false)
        {
            Initialized = true;

            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            //s_instance._sound.Init();
        }
    }

    public void Update()
    {
        _network?.Update();
    }

    public static void Clear()
    {
        //Sound.Clear();
        //Scene.Clear();
        //UI.Clear();
        //Pool.Clear();
    }
}