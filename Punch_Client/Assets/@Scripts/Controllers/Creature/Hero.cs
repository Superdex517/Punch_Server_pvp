using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Creature
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {

    }

    protected override void UpdateMove()
    {
        base.UpdateMove();

        UpdatePos(MoveSpeed);
    }

    public virtual void SetInfo(int templatedId)
    {

    }
}
