using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        
    }

    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];
        _objects.Add(typeof(T), objects);

        for(int i = 0; i < names.Length; i++)
        {

        }
    }

}