using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
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
        UpdateAI();
    }

    protected override void UpdateMove()
    {
        base.UpdateMove();
        
        //UpdateLerpToPos(MoveSpeed);
    }

    public virtual void SetInfo(int templatedId)
    {
        
    }
}
