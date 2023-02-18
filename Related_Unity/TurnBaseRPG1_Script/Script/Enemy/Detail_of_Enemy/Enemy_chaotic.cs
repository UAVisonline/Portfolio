using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_chaotic : Enemy
{
    [SerializeField] private AudioClip change_clip;
    [SerializeField] private AudioClip stat_up_clip;
    [SerializeField] private AudioClip attack_magic_clip;
    [SerializeField] private AudioClip attack_physical_clip;

    [SerializeField] private GameObject attack_up_particle;
    [SerializeField] private GameObject guard_up_particle;
    [SerializeField] private GameObject chaos_particle;
    [SerializeField] private GameObject magic_attack_particle;
    [SerializeField] private GameObject physical_attack_particle;

    [SerializeField] private GameObject ready_attack_particle;
    [SerializeField] private GameObject ready_stat_particle;

    protected override void Start()
    {
        base.Start();
        enemy_function_pos = 0;
    }

    public override void enemy_function()
    {
        if(solution_var>=1)
        {
            solution_var -= 1;
            enemy_spec.temp_ATK = 0;
            enemy_spec.temp_MDEF = 0;
            enemy_spec.temp_PDEF = 0;

            enemy_function_pos = 0;
        }

        switch(enemy_function_pos)
        {
            case 0:
                break;
            /*
            case 1:
                int temp = enemy_spec.start_PDEF;
                enemy_spec.start_PDEF = enemy_spec.start_MDEF;
                enemy_spec.start_MDEF = temp;
                change_enemy_function_pos(false);

                Util_Manager.utilManager.play_clip(change_clip);
                DungeonManager.dungeonManager.make_particle_enemy_position(chaos_particle);
                break;
            */
            case 1:
                DungeonManager.dungeonManager.Damage_to_Player(attack_type.physical, attacked_type.battle, ret_enemy_damage());
                change_enemy_function_pos(false);

                Util_Manager.utilManager.play_clip(attack_physical_clip);
                DungeonManager.dungeonManager.make_particle_player_position(physical_attack_particle);
                break;

            case 2:
                enemy_spec.temp_PDEF += 3;
                enemy_spec.temp_MDEF += 3;
                change_enemy_function_pos(false);

                if(enemy_spec.temp_PDEF>=15)
                {
                    enemy_spec.temp_PDEF = 15;
                }

                if(enemy_spec.temp_MDEF>=15)
                {
                    enemy_spec.temp_MDEF = 15;
                }

                Util_Manager.utilManager.play_clip(stat_up_clip);
                DungeonManager.dungeonManager.make_particle_enemy_position(guard_up_particle);
                break;

            case 3:
                enemy_spec.temp_ATK += 6;
                change_enemy_function_pos(false);

                if(enemy_spec.temp_ATK>=30)
                {
                    enemy_spec.temp_ATK = 30;
                }

                Util_Manager.utilManager.play_clip(stat_up_clip);
                DungeonManager.dungeonManager.make_particle_enemy_position(attack_up_particle);
                break;

            case 4:
                DungeonManager.dungeonManager.Damage_to_Player(attack_type.magic, attacked_type.battle, ret_enemy_damage());
                change_enemy_function_pos(false);

                Util_Manager.utilManager.play_clip(attack_magic_clip);
                DungeonManager.dungeonManager.make_particle_player_position(magic_attack_particle);
                break;
        }
    }

    protected override void check_dead()
    {
        change_enemy_function_pos(true);
        base.check_dead();
    }

    private void change_enemy_function_pos(bool value)
    {
        int prev = enemy_function_pos;
        enemy_function_pos = (Random.Range(0, 101) % 4) + 1;
        while (prev == enemy_function_pos)
        {
            enemy_function_pos = (Random.Range(0, 101) % 4) + 1;
        }

        if(value==true)
        {
            if (enemy_function_pos == 2 || enemy_function_pos == 5)
            {
                Util_Manager.utilManager.play_clip(stat_up_clip);
                DungeonManager.dungeonManager.make_particle_enemy_position(ready_attack_particle);
            }
            else
            {
                Util_Manager.utilManager.play_clip(stat_up_clip);
                DungeonManager.dungeonManager.make_particle_enemy_position(ready_stat_particle);
            }
        }
    }

}
