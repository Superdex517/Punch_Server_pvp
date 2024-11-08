using System;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Define
{
    public const char MAP_TOOL_WALL = '0';
    public const char MAP_TOOL_NONE = '1';

    public enum EScene
    {
        Unknown,
        TitleScene,
        GameScene,
    }

    public enum ESound
    {
        Bgm,
        SubBgm,
        Effect,
        Max,
    }

    public enum ETouchEvent
    {
        PointerUp,
        PointerDown,
        Click,
        Pressed,
        BeginDrag,
        Drag,
        EndDrag,
    }

    public enum ELanguage
    {
        Korean,
        English,
    }

    public enum ELayer
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Dummy1 = 3,
        Water = 4,
        UI = 5,
        Hero = 6,
        Monster = 7,
        Boss = 8,
        //
        Env = 11,
        Obstacle = 12,
        //
        Projectile = 20,
    }

    public static class AnimName
    {
        public const string ATTACK = "attack";
        public const string IDLE = "Idle";
        public const string MOVE = "WalkForword";

        public const string DAMAGED = "hit";
        public const string DEAD = "Dead";
        public const string EVENT_ATTACK_A = "event_attack";
        public const string EVENT_ATTACK_B = "event_attack";
        public const string EVENT_SKILL_A = "event_attack";
        public const string EVENT_SKILL_B = "event_attack";
    }
}