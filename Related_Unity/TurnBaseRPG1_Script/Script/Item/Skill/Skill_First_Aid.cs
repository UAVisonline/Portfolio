using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_First_Aid : Skill
{
    [SerializeField] private int min_value;
    [SerializeField] private int max_value;

    [SerializeField] private AudioClip skill_sound;
    [SerializeField] private GameObject particle;

    public override void skill_function()
    {
        base.skill_function();

        int value = Random.Range(min_value, max_value + 1);
        DungeonManager.dungeonManager.heal_player_fixed(value);

        Util_Manager.utilManager.play_clip(skill_sound);
        DungeonManager.dungeonManager.make_particle_player_position(particle);
    }
}
