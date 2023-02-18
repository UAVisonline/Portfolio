using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continuity_of_knife : Weapon_Item
{
    private int contiunity = 0;

    public override void reset_skill()
    {
        contiunity = 0;
        base.reset_skill();
    }

    public override void function()
    {
        int value = ret_weapon_damage();
        value += contiunity * 2;

        if (contiunity < 10)
        {
            contiunity += 1;
        }

        DungeonManager.dungeonManager.Damage_to_enemy(attack_type, attacked_type.battle, value);
        Util_Manager.utilManager.play_clip(weapon_sound);
        DungeonManager.dungeonManager.make_particle_enemy_position(particle);
    }
}
