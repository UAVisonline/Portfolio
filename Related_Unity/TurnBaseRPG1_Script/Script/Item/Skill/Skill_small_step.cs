using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_small_step : Skill
{
    [SerializeField] private AudioClip skill_sound;
    [SerializeField] private GameObject particle;

    public override void skill_function()
    {
        base.skill_function();

        int value = DungeonManager.dungeonManager.ret_player_standard_damage();
        value = Mathf.RoundToInt(value * 0.5f);
        DungeonManager.dungeonManager.Damage_to_enemy(this.attack_type, attacked_type.battle, value);

        Util_Manager.utilManager.play_clip(skill_sound);
        DungeonManager.dungeonManager.make_particle_enemy_position(particle);
    }
}
