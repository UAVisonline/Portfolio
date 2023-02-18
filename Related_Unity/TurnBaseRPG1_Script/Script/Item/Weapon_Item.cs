using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Item : Armed_Item
{
    public kind_of_weapon weapon_kind;

    [SerializeField] private float ATK_correlation;
    [SerializeField] private float STR_correlation;
    [SerializeField] private float DEX_correlation;
    [SerializeField] private float INT_correlation;

    [SerializeField] protected attack_type attack_type;

    [SerializeField] protected AudioClip weapon_sound;
    [SerializeField] protected GameObject particle;

    public override bool function_condition()
    {
        // 무기에 따라 바꾸어야 하는데 개발하는동안 안 쓸듯

        return base.function_condition();
    }

    public override void function()
    {
        DungeonManager.dungeonManager.Damage_to_enemy(attack_type, attacked_type.battle, ret_weapon_damage());
        Util_Manager.utilManager.play_clip(weapon_sound);
        DungeonManager.dungeonManager.make_particle_enemy_position(particle);

        base.function();
    }

    /*
    public override bool ret_skill_condition()
    {
        return base.ret_skill_condition();
    }
    */

    public kind_of_weapon ret_weapon_kind()
    {
        return weapon_kind;
    }

    public int ret_weapon_damage()
    {
        float ret = PlayerManager.playerManager.spec.ret_current_atk_int() * ATK_correlation;
        ret += PlayerManager.playerManager.spec.ret_current_str_int() * STR_correlation;
        ret += PlayerManager.playerManager.spec.ret_current_dex_int() * DEX_correlation;
        ret += PlayerManager.playerManager.spec.ret_current_int_int() * INT_correlation;

        return Mathf.RoundToInt(ret);
    }
}
