using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Vampire : Enemy
{
    [SerializeField] private AudioClip vampire_buff_sound;
    [SerializeField] private AudioClip vampire_heal_sound;
    [SerializeField] private AudioClip bite_sound;
    [SerializeField] private AudioClip magic_sound;

    [SerializeField] private GameObject vampire_heal_particle;
    [SerializeField] private GameObject vampire_stat_up_particle;
    [SerializeField] private GameObject vampire_physical_particle;
    [SerializeField] private GameObject vampire_power_magic_ready;
    [SerializeField] private GameObject vampire_magic_particle;


    private int vampire_power;
    private int heal_count;

    protected override void Start()
    {
        base.Start();
        enemy_function_pos = 0;
        vampire_power = 0;
        heal_count = 3;
    }

    public override void enemy_function()
    {
        int value = 0;
        switch(enemy_function_pos)
        {
            case 0:
                vampire_power += 15;

                heal(0.2f);
                enemy_function_pos = 1;
                Util_Manager.utilManager.play_clip(vampire_buff_sound);
                DungeonManager.dungeonManager.make_particle_enemy_position(vampire_stat_up_particle);

                break;
            case 1:
                DungeonManager.dungeonManager.Damage_to_Player(attack_type.physical, attacked_type.battle, ret_enemy_damage());
                value = DungeonManager.dungeonManager.ret_current_damage();
                if(value<5)
                {
                    value = 5;
                }
                vampire_power += value;

                enemy_function_pos = 2;
                Util_Manager.utilManager.play_clip(bite_sound);
                DungeonManager.dungeonManager.make_particle_player_position(vampire_physical_particle);

                break;
            case 2:
                if(ret_enemy_ratio()<0.6f && heal_count>0)
                {
                    if(solution_var>=1)
                    {
                        heal(0.15f);
                        
                    }
                    else
                    {
                        heal(0.3f);
                    }
                    heal_count -= 1;

                    Util_Manager.utilManager.play_clip(vampire_heal_sound);
                    DungeonManager.dungeonManager.make_particle_enemy_position(vampire_heal_particle);
                    enemy_function_pos = 1;
                }
                else
                {
                    DungeonManager.dungeonManager.Damage_to_Player(attack_type.physical, attacked_type.battle, ret_enemy_damage());
                    value = DungeonManager.dungeonManager.ret_current_damage();
                    if (value < 5)
                    {
                        value = 5;
                    }
                    vampire_power += value;

                    Util_Manager.utilManager.play_clip(bite_sound);
                    DungeonManager.dungeonManager.make_particle_player_position(vampire_physical_particle);
                    enemy_function_pos = 0;
                }
                break;
            case 5:
                value = Mathf.RoundToInt(ret_enemy_damage() * 1.5f) + Mathf.RoundToInt(vampire_power*0.2f);
                vampire_power = 0;
                if(solution_var>=1)
                {
                    solution_var -= 1;
                    value = Mathf.RoundToInt(ret_enemy_damage() * 1.2f);
                }
                DungeonManager.dungeonManager.Damage_to_Player(attack_type.magic, attacked_type.battle, value);

                DungeonManager.dungeonManager.make_particle_enemy_position(vampire_power_magic_ready);
                DungeonManager.dungeonManager.make_particle_player_position(vampire_magic_particle);
                Util_Manager.utilManager.play_clip(magic_sound);
                enemy_function_pos = 6;
                break;
            case 6:
                enemy_function_pos = 2;
                break;
        }

        if(vampire_power >= 35)
        {
            enemy_function_pos = 5;
        }
    }
}
