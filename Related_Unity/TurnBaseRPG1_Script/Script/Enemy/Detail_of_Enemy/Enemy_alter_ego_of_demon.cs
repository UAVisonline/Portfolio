using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_alter_ego_of_demon : Enemy
{
    [SerializeField] private AudioClip gather_power_clip;
    [SerializeField] private AudioClip power_up_clip;
    [SerializeField] private AudioClip debuff_clip;
    [SerializeField] private AudioClip physical_clip;
    [SerializeField] private AudioClip magic_clip;
    [SerializeField] private AudioClip ultimate_clip;

    [SerializeField] private GameObject gather_power_particle;
    [SerializeField] private GameObject power_particle;
    [SerializeField] private GameObject debuff_particle;
    [SerializeField] private GameObject physical_particle;
    [SerializeField] private GameObject magic_particle;
    [SerializeField] private GameObject ultimate_particle;

    private int ultimate_var = 0;

    protected override void Start()
    {
        base.Start();

        enemy_function_pos = 0;
        ultimate_var = 0;
    }

    public override void enemy_function()
    {

        switch(enemy_function_pos)
        {
            case 0:
                enemy_spec.temp_ATK += 1 * PlayerManager.playerManager.ret_total_amulet_count();
                if(enemy_spec.temp_ATK < 10)
                {
                    enemy_spec.temp_ATK = 10;
                }
                DungeonManager.dungeonManager.make_particle_enemy_position(power_particle);
                Util_Manager.utilManager.play_clip(power_up_clip);
                enemy_function_pos = Random.Range(1, 3);
                break;

            case 1:
                DungeonManager.dungeonManager.Damage_to_Player(attack_type.physical, attacked_type.battle, ret_enemy_damage() + PlayerManager.playerManager.spec.ret_amulet_pdef_int());
                Util_Manager.utilManager.play_clip(physical_clip);
                DungeonManager.dungeonManager.make_particle_player_position(physical_particle);
                enemy_function_pos = 3;
                break;

            case 2:
                DungeonManager.dungeonManager.Damage_to_Player(attack_type.magic, attacked_type.battle, ret_enemy_damage() + PlayerManager.playerManager.spec.ret_amulet_pdef_int());
                Util_Manager.utilManager.play_clip(magic_clip);
                DungeonManager.dungeonManager.make_particle_player_position(magic_particle);
                enemy_function_pos = 3;
                break;

            case 3:
                if(DungeonManager.dungeonManager.ret_player_barrier()==true)
                {
                    DungeonManager.dungeonManager.set_player_barrier(false);
                    Util_Manager.utilManager.play_clip(debuff_clip);
                    DungeonManager.dungeonManager.make_particle_player_position(debuff_particle);
                }
                else
                {
                    enemy_spec.temp_ATK += 1 * PlayerManager.playerManager.ret_total_amulet_count();
                    DungeonManager.dungeonManager.make_particle_enemy_position(power_particle);
                    Util_Manager.utilManager.play_clip(power_up_clip);

                    if (enemy_spec.temp_ATK < 10)
                    {
                        enemy_spec.temp_ATK = 10;
                    }
                }

                enemy_function_pos = Random.Range(1, 3);
                ultimate_var += 1;
                if(ultimate_var>=2)
                {
                    enemy_function_pos = 4;
                }
                break;

            case 4:
                Util_Manager.utilManager.play_clip(gather_power_clip);
                DungeonManager.dungeonManager.make_particle_enemy_position(gather_power_particle);
                enemy_function_pos = 5;
                break;

            case 5:
                int value = 99999;
                if(DungeonManager.dungeonManager.ret_player_guard()==true)
                {
                    value = ret_enemy_damage();
                }

                DungeonManager.dungeonManager.Damage_to_Player(attack_type.none, attacked_type.battle, value);
                Util_Manager.utilManager.play_clip(ultimate_clip);
                DungeonManager.dungeonManager.make_particle_player_position(ultimate_particle);

                ultimate_var = 0;
                enemy_function_pos = Random.Range(1, 3);
                break;

        }

        if(solution_var >= 1)
        {
            solution_var = 0;
            enemy_spec.temp_ATK = 0;
        }

        Debug.Log(enemy_function_pos);
    }

    public override int Hurt(int value, attack_type attack_type_value)
    {
        if (attack_type_value == attack_type.physical)
        {
            value -= enemy_spec.ret_current_pdef_int();
        }
        else if (attack_type_value == attack_type.magic)
        {
            value -= enemy_spec.ret_current_mdef_int();
        }

        if (value <= 0)
        {
            value = 1;
        }
        else if(value > 30)
        {
            value = 30;
        }

        current_hp -= value;
        if (enemy_UI != null)
        {
            enemy_UI.visualize(current_hp, enemy_spec.ret_battle_hp_int());
        }
        check_dead();

        return value;
    }
}
