using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack_man : Enemy
{
    private int attack_mode;

    [SerializeField] private GameObject gathering_particle;
    [SerializeField] private GameObject attack_particle_physical;
    [SerializeField] private GameObject attack_particle_magic;

    [SerializeField] private AudioClip gathering_clip;
    [SerializeField] private AudioClip physical_clip;
    [SerializeField] private AudioClip magic_clip;

    protected override void Start()
    {
        base.Start();
        attack_mode = Random.Range(0, 101) % 2;
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
                attack_type temp = attack_type.physical;
                if(attack_mode == 1)
                {
                    temp = attack_type.magic;
                }

                DungeonManager.dungeonManager.Damage_to_Player(temp, attacked_type.battle, ret_enemy_damage());
                if(temp==attack_type.physical)
                {
                    Util_Manager.utilManager.play_clip(physical_clip);
                    DungeonManager.dungeonManager.make_particle_player_position(attack_particle_physical);
                }
                else if(temp==attack_type.magic)
                {
                    Util_Manager.utilManager.play_clip(magic_clip);
                    DungeonManager.dungeonManager.make_particle_player_position(attack_particle_magic);
                }

                attack_mode = Random.Range(0, 101) % 2;
                enemy_function_pos = Random.Range(0, 101) % 2;
                break;
        }
    }
}
