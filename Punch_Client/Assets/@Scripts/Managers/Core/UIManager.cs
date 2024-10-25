using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;

public class UIManager
{

    private UI_Scene _sceneUI = null;
    public UI_Scene SceneUI
    {
        get { return _sceneUI; }
        set { _sceneUI = value; }
    }

    private int _pupupOrder = 100;

    public Canvas SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);

        return canvas;
    }

    public T GetSceneUI<T>() where T : UI_Base
    {
        return _sceneUI as T;
    }
}