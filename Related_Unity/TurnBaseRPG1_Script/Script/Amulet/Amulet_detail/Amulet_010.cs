using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_010 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.spec.amulet_PDEF += 3;

        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.spec.amulet_PDEF -= 3;

        base.OnDismiss();
    }
}
