using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_042 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.schedule_information.soul_sale_percent -= 0.1f;
        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.schedule_information.soul_sale_percent += 0.1f;
        base.OnDismiss();
    }
}
