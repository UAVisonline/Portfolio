using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_057 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.spec.amulet_PDEF += 1;
        PlayerManager.playerManager.spec.amulet_MDEF += 1;

        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.spec.amulet_PDEF -= 1;
        PlayerManager.playerManager.spec.amulet_MDEF -= 1;

        base.OnDismiss();
    }
}
