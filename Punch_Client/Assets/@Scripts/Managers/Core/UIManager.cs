using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;

public class UIManager
{
    private int _pupupOrder = 100;

    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private UI_Scene _sceneUI = null;
    public UI_Scene SceneUI
    {
        get { return _sceneUI; }
        set { _sceneUI = value; }
    }


    private Dictionary<string, UI_Popup> _popups = new Dictionary<string, UI_Popup>();
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public Canvas SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);

        return canvas;
    }

    public T GetSceneUI<T>() where T : UI_Base
    {
        return _sceneUI as T;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        popup.gameObject.SetActive(false);
        _pupupOrder--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public int GetPopupCount()
    {
        return _popupStack.Count;
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _popups.Clear();
        Root.gameObject.DestroyChildren();
        Time.timeScale = 1;
        _sceneUI = null;
    }
}