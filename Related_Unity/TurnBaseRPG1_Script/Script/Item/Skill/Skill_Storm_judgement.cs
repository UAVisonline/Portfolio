using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Storm_judgement : Skill
{
    [SerializeField] private AudioClip skill_sound;
    [SerializeField] private GameObject particle;

    public override void skill_function()
    {
        base.skill_function();

        int value = 5;
        Tiny_Storm temp = this.GetComponentInParent<Tiny_Storm>();
        if(temp!=null)
        {
            int count = temp.ret_storm_count();
            value += count * 10;
        }
        DungeonManager.dungeonManager.Damage_to_enemy(attack_type, attacked_type.battle, value);
        Util_Manager.utilManager.play_clip(skill_sound);
        DungeonManager.dungeonManager.make_particle_enemy_position(particle);
    }

}
