using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour
{
    Button btn;
    public Vector3 upScale;
    public Vector3 originScale;
    //(1.2f, 1.2f, 1);

    private void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Anim);
    }

    private void Anim()
    {
        Vector3 scale = upScale;
        LeanTween.scale(gameObject, scale, 0.1f);
        LeanTween.scale(gameObject, originScale, 0.1f).setDelay(0.1f);
    }
}
