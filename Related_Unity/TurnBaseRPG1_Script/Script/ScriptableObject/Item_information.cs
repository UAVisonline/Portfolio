using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum kind_of_Item
{
    Weapon, Armor, Accessories, Potion
}

public enum kind_of_weapon
{
    sword, dagger, staff, none
}

[CreateAssetMenu(menuName = "ScriptableObject/Item")]
public class Item_information : ScriptableObject
{
    public int code;

    public string name;
    public string information;

    public kind_of_Item kind;
    public Sprite jacket;

    public int gold;
}
