using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Double_Slash : Skill
{
    [SerializeField] private AudioClip skill_sound;
    [SerializeField] private GameObject slash_particle;

    public override void reset_skill_condition()
    {
        base.reset_skill_condition();

        set_turn_condition(0);
    }

    public override void skill_function()
    {
        base.skill_function();

        int value = Mathf.RoundToInt(DungeonManager.dungeonManager.ret_player_standard_damage() * 1.5f);
        DungeonManager.dungeonManager.Damage_to_enemy(attack_type, attacked_type.battle, value);

        Util_Manager.utilManager.play_clip(skill_sound);
        DungeonManager.dungeonManager.make_particle_enemy_position(slash_particle);
        
    }
}
