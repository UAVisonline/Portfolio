using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_003 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.spec.amulet_STR += 15;

        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.spec.amulet_STR -= 15;

        base.OnDismiss();
    }
}
