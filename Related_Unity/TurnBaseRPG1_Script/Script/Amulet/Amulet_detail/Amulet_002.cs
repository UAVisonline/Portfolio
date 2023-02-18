using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_002 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.spec.amulet_max_hp += 25;

        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.spec.amulet_max_hp -= 25;

        base.OnDismiss();
    }
}
