using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiny_Storm : Weapon_Item
{
    private int storm_count = 0;

    public override void reset_skill()
    {
        storm_count = 0;
        base.reset_skill();
    }

    public override void function()
    {
        storm_count += Random.Range(0, 3) + 1;
        if(storm_count > 10)
        {
            storm_count = 0;
        }

        base.function();
    }

    public int ret_storm_count()
    {
        int temp = storm_count;
        storm_count = 0;

        return temp;
    }
}
