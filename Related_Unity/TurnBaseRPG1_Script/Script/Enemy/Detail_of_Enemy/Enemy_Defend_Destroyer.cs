using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Defend_Destroyer : Enemy
{
    [SerializeField] private AudioClip gathering_clip;
    [SerializeField] private AudioClip critical_hammer_clip;
    [SerializeField] private AudioClip weak_hammer_clip;

    [SerializeField] private GameObject gathering_particle;
    [SerializeField] private GameObject critical_hammer_particle;
    [SerializeField] private GameObject weak_hammer_particle;

    // Start is called before the first frame update
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
                enemy_function_pos = 1;
                Util_Manager.utilManager.play_clip(gathering_clip);
                DungeonManager.dungeonManager.make_particle_enemy_position(gathering_particle);
                break;
            case 1:
                enemy_function_pos = 2;
                Util_Manager.utilManager.play_clip(gathering_clip);
                DungeonManager.dungeonManager.make_particle_enemy_position(gathering_particle);
                break;
            case 2:

                if(solution_var>=1)
                {
                    int value = Mathf.RoundToInt(ret_enemy_damage() * 0.5f);
                    DungeonManager.dungeonManager.Damage_to_Player(attack_type.physical, attacked_type.battle, value);
                    Util_Manager.utilManager.play_clip(weak_hammer_clip);
                    DungeonManager.dungeonManager.make_particle_player_position(weak_hammer_particle);
                }
                else
                {
                    if(DungeonManager.dungeonManager.ret_player_guard()==true)
                    {
                        int value = Mathf.RoundToInt(ret_enemy_damage() * 0.5f);
                        DungeonManager.dungeonManager.Damage_to_Player(attack_type.physical, attacked_type.battle, value);
                        Util_Manager.utilManager.play_clip(weak_hammer_clip);
                        DungeonManager.dungeonManager.make_particle_player_position(weak_hammer_particle);
                    }
                    else
                    {
                        int value = Mathf.RoundToInt(ret_enemy_damage() * 1.5f);
                        DungeonManager.dungeonManager.Damage_to_Player(attack_type.physical, attacked_type.battle, value);
                        Util_Manager.utilManager.play_clip(critical_hammer_clip);
                        DungeonManager.dungeonManager.make_particle_player_position(critical_hammer_particle);
                    }
                }
                enemy_function_pos = 1;
                break;
        }
    }
}
