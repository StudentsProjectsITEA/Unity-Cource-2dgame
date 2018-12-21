using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string Name;
    public int Coins;
    public int MaxScore;
    public bool IsMagnetAvailable, IsPowerJumpAvailable;
}

[Serializable]
public class ArrayWraper
{
    public PlayerData[] DataArray;
}
