using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_wizard : Enemy
{
    [SerializeField] private AudioClip heal_sound;
    [SerializeField] private AudioClip magic_sound;

    [SerializeField] private GameObject heal_particle;
    [SerializeField] private GameObject magic_particle;

    private int heal_number;

    protected override void Start()
    {
        base.Start();
        enemy_function_pos = 0;
        heal_number = 1;
    }

    public override void enemy_function()
    {
        if(solution_var>=1)
        {
            DungeonManager.dungeonManager.Damage_to_Player(attack_type.magic, attacked_type.battle, ret_enemy_damage());
            Util_Manager.utilManager.play_clip(magic_sound);
            DungeonManager.dungeonManager.make_particle_player_position(magic_particle);
        }
        else
        {
            if(ret_enemy_ratio()<=0.5f && heal_number>0)
            {
                heal_number -= 1;
                heal(0.3f);

                Util_Manager.utilManager.play_clip(heal_sound);
                DungeonManager.dungeonManager.make_particle_enemy_position(heal_particle);
            }
            else
            {
                DungeonManager.dungeonManager.Damage_to_Player(attack_type.magic, attacked_type.battle, ret_enemy_damage());
                Util_Manager.utilManager.play_clip(magic_sound);
                DungeonManager.dungeonManager.make_particle_player_position(magic_particle);
            }
        }
    }
}
