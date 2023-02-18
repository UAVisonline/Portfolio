using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_second_chance : Skill
{
    [SerializeField] private AudioClip heal_sound;
    [SerializeField] private AudioClip attack_sound;

    [SerializeField] private GameObject heal_particle;
    [SerializeField] private GameObject attack_particle;
    [SerializeField] private GameObject barrier_particle;

    public override void skill_function()
    {
        base.skill_function();

        if(DungeonManager.dungeonManager.ret_player_hp_ratio()>0.6f)
        {
            int value = Random.Range(0, 101) % 2;

            if(value==0)
            {
                int damage = Random.Range(5, 15) + 5;

                DungeonManager.dungeonManager.Damage_to_enemy(attack_type, attacked_type.battle, damage);
                Util_Manager.utilManager.play_clip(attack_sound);
                DungeonManager.dungeonManager.make_particle_player_position(attack_particle);
            }
            else
            {
                int heal_value = Random.Range(0, 10) + 3;

                DungeonManager.dungeonManager.heal_player_fixed(heal_value);
                Util_Manager.utilManager.play_clip(heal_sound);
                DungeonManager.dungeonManager.make_particle_player_position(heal_particle);
            }
        }
        else if(DungeonManager.dungeonManager.ret_player_hp_ratio()<0.3f)
        {
            int value = Random.Range(0, 101) % 2;

            if (value == 0)
            {
                DungeonManager.dungeonManager.set_player_barrier(true);
                Util_Manager.utilManager.play_clip(heal_sound);
                DungeonManager.dungeonManager.make_particle_player_position(barrier_particle);
            }
            else
            {
                int heal_value = Random.Range(10, 25);

                DungeonManager.dungeonManager.heal_player_fixed(heal_value);
                Util_Manager.utilManager.play_clip(heal_sound);
                DungeonManager.dungeonManager.make_particle_player_position(heal_particle);
            }
        }
        else
        {
            int value = Random.Range(0, 101) % 2;

            if (value == 0)
            {
                int damage = Random.Range(10, 25) + 5;

                DungeonManager.dungeonManager.Damage_to_enemy(attack_type, attacked_type.battle, damage);
                Util_Manager.utilManager.play_clip(attack_sound);
                DungeonManager.dungeonManager.make_particle_player_position(attack_particle);
            }
            else
            {
                int heal_value = Random.Range(5, 16);

                DungeonManager.dungeonManager.heal_player_fixed(heal_value);
                Util_Manager.utilManager.play_clip(heal_sound);
                DungeonManager.dungeonManager.make_particle_player_position(heal_particle);
            }
        }
    }
}
