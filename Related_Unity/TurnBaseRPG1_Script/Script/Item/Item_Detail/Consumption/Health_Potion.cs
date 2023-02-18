using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Potion : Consumption_Item
{
    [SerializeField] private float percent_heal;
    [SerializeField] private int fixed_heal;

    [SerializeField] private AudioClip potion_sound;
    [SerializeField] private GameObject potion_particle;

    public override bool function_condition()
    {
        if(DungeonManager.dungeonManager.current_dungeon_struct.in_dungeon==true)
        {
            dungeon_situation temp = DungeonManager.dungeonManager.ret_current_situation();

            switch(temp)
            {
                case dungeon_situation.battle:
                    return true;
                case dungeon_situation.dungeon_event:
                    return true;
                case dungeon_situation.wait:
                    return true;
            }
        }
        return false;
    }

    public override void function()
    {
        DungeonManager.dungeonManager.heal_player(percent_heal, fixed_heal);
        Util_Manager.utilManager.play_clip(potion_sound);
        DungeonManager.dungeonManager.make_particle_player_position(potion_particle);
    }
}
