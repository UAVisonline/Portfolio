using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_001 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.spec.amulet_max_hp += 10;

        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.spec.amulet_max_hp -= 10;

        base.OnDismiss();
    }
}
