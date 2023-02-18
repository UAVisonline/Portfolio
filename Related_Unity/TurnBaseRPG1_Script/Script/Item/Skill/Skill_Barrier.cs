using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Barrier : Skill
{
    [SerializeField] private GameObject barrier_particle;
    [SerializeField] private AudioClip barrier_sound;

    public override void reset_skill_condition()
    {
        base.reset_skill_condition();

        set_turn_condition(0);
    }

    public override void skill_function()
    {
        base.skill_function();

        DungeonManager.dungeonManager.set_player_barrier(true);

        Util_Manager.utilManager.play_clip(barrier_sound);
        DungeonManager.dungeonManager.make_particle_player_position(barrier_particle);
    }
}
