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
     
        UpdatePos(MoveSpeed, true);
    }

    protected override void UpdateMove()
    {
        base.UpdateMove();
        
    }

    public virtual void SetInfo(int templatedId)
    {
        
    }
}
