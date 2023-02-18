using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Item_information information;

    public int required_energy;

    public virtual bool function_condition()
    {
        return true;
    }

    public virtual void function()
    {

    }

    public virtual string ret_item_information_text()
    {
        return information.information;
    }

    public virtual string ret_item_name_text()
    {
        return information.name;
    }

    public virtual int ret_required_energy()
    {
        return required_energy;
    }
}
