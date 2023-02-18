using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier_Potion : Consumption_Item
{
    [SerializeField] private AudioClip potion_sound;
    [SerializeField] private GameObject potion_particle;

    public override bool function_condition()
    {
        if (DungeonManager.dungeonManager.ret_current_situation() == dungeon_situation.battle)
        {
            return true;
        }
        return false;
    }

    public override void function()
    {
        DungeonManager.dungeonManager.set_player_barrier(true);
        Util_Manager.utilManager.play_clip(potion_sound);
        DungeonManager.dungeonManager.make_particle_player_position(potion_particle);
    }
}
