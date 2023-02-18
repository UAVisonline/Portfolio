using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Power_man : Enemy
{
    [SerializeField] private AudioClip power_up_clip;
    [SerializeField] private AudioClip magic_clip;
    [SerializeField] private AudioClip physical_clip;

    [SerializeField] private GameObject power_up_particle;
    [SerializeField] private GameObject magic_partice;
    [SerializeField] private GameObject physical_particle;


    protected override void Start()
    {
        base.Start();
        enemy_function_pos = 0;
    }

    public override void enemy_function()
    {
        switch(enemy_function_pos)
        {
            case 0:
                enemy_spec.temp_ATK += 5;
                Util_Manager.utilManager.play_clip(power_up_clip);
                DungeonManager.dungeonManager.make_particle_enemy_position(power_up_particle);
                enemy_function_pos = 1;
                break;
            case 1:
                if(PlayerManager.playerManager.spec.ret_current_pdef_int() >= PlayerManager.playerManager.spec.ret_current_mdef_int())
                {
                    DungeonManager.dungeonManager.Damage_to_Player(attack_type.magic, attacked_type.battle, ret_enemy_damage());
                    Util_Manager.utilManager.play_clip(magic_clip);
                    DungeonManager.dungeonManager.make_particle_player_position(magic_partice);
                }
                else
                {
                    DungeonManager.dungeonManager.Damage_to_Player(attack_type.physical, attacked_type.battle, ret_enemy_damage());
                    Util_Manager.utilManager.play_clip(physical_clip);
                    DungeonManager.dungeonManager.make_particle_player_position(physical_particle);
                }

                if(enemy_spec.temp_ATK<10)
                {
                    enemy_function_pos = 0;
                }
                break;
        }
    }
}
