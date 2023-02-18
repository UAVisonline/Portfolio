using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Amulet_timing
{
    immediately, start_battle, before_attack, after_attack, before_attacked, after_attacked, turn_end, end_battle, life_end
}

public class BaseAmuletScript : MonoBehaviour
{
    public Amulet_information information;

    public List<Amulet_timing> amulet_timeing = new List<Amulet_timing>();

    protected virtual void OnEnable()
    {
        PlayerManager.playerManager.insert_amulet(this);
        
        for(int i =0;i<amulet_timeing.Count;i++)
        {
            PlayerManager.playerManager.insert_detail_amulet(this, amulet_timeing[i]);
        }
        //Debug.Log("Acquire : " + information.name);
    }

    protected virtual void OnDisable()
    {
        
    }

    public virtual void OnAcquire()
    {
        PlayerManager.playerManager.spec.carried_amulet.Add(information.code);

        // Debug.Log("Acquire Soon: " + information.name);

        PlayerManager.playerManager.save_spec();
        PlayerManager.playerManager.save_schedule_information();
    }

    public virtual void OnDismiss()
    {
        for(int i = 0;i<amulet_timeing.Count;i++)
        {
            PlayerManager.playerManager.dismiss_detail_amulet(this, amulet_timeing[i]);
        }
        // Debug.Log("Dismiss : " + information.name);
    }

    public virtual void OnFunction(Amulet_timing timing)
    {

    }
}
