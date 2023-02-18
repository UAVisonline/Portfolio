using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumption_Item : Item
{
    public override bool function_condition()
    {
        /*if(DungeonManager.dungeonManager.current_dungeon_struct.in_dungeon==true)
        {
            return true;
        }*/
        return false;
    }

    public override void function()
    {
        // DungeonManager.dungeonManager.Damage_to_Player(attack_type.physical, attacked_type.status, 10);
    }
}
