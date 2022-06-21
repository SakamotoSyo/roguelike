using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : SingletonBehaviour<GameManager>
{
    public enum TurnManager
    {
        Player,
        Enemy,
    }

    public TurnManager TurnType;

    //ƒvƒŒƒCƒ„[‚ÌêŠ
    public int PlayerX;
    public int PlayerY;
 
}
