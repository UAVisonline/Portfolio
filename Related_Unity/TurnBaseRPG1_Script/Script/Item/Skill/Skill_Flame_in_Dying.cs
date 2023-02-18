using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Flame_in_Dying : Skill
{
    [SerializeField] private AudioClip skill_sound;
    [SerializeField] private GameObject skill_particle;

    public override void skill_function()
    {
        base.skill_function();

        int value = Mathf.RoundToInt(DungeonManager.dungeonManager.ret_player_standard_damage() * 1.5f);
        DungeonManager.dungeonManager.Damage_to_enemy(attack_type, attacked_type.battle, value);

        Util_Manager.utilManager.play_clip(skill_sound);
        DungeonManager.dungeonManager.make_particle_enemy_position(skill_particle);
    }
}
