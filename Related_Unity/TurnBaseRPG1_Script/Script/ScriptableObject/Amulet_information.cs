using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum amulet_rank
{
    A, B, C, N
}

[CreateAssetMenu(menuName ="ScriptableObject/Amulet")]
public class Amulet_information : ScriptableObject
{
    public int code;

    public string name;
    public string information;

    public amulet_rank rank;
    public Sprite amulet_jacket;

    public int duplicate_limit; // 1 : only one, 2 : only two, 0 : infinite
}
