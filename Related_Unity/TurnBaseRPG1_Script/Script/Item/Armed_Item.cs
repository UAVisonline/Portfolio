using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armed_Item : Item
{
    // [SerializeField] private Skill skill_in_item;

    [SerializeField] private int armor_ATK = 0;
    [SerializeField] private int armor_PDEF = 0;
    [SerializeField] private int armor_MDEF = 0;
    [SerializeField] private int armor_STR = 0;
    [SerializeField] private int armor_DEX = 0;
    [SerializeField] private int armor_INT = 0;
    [SerializeField] private int armor_HP = 0;

    [SerializeField] protected Skill ref_skill;

    private void OnEnable()
    {
        ref_skill = this.GetComponentInChildren<Skill>();

        if(ref_skill!=null)
        {
            ref_skill.reset_skill_condition();
        }
    }

    public override bool function_condition()
    {
        return true;
    }

    public override void function()
    {
        // Damage??? (갑옷, 반지는 딱히 여기에 무엇인가 있을 필요가 없을듯)
    }

    public virtual void equip()
    {
        if(armor_ATK!=0)
        {
            PlayerManager.playerManager.spec.armor_ATK += armor_ATK;
        }
        if(armor_PDEF!=0)
        {
            PlayerManager.playerManager.spec.armor_PDEF += armor_PDEF;
        }
        if (armor_MDEF != 0)
        {
            PlayerManager.playerManager.spec.armor_MDEF += armor_MDEF;
        }
        if (armor_STR != 0)
        {
            PlayerManager.playerManager.spec.armor_STR += armor_STR;
        }
        if (armor_DEX != 0)
        {
            PlayerManager.playerManager.spec.armor_DEX += armor_DEX;
        }
        if (armor_INT != 0)
        {
            PlayerManager.playerManager.spec.armor_INT += armor_INT;
        }
        if(armor_HP != 0)
        {
            PlayerManager.playerManager.spec.armor_hp += armor_HP;
            DungeonManager.dungeonManager.visualize_player_hp();
        }
    }

    public virtual void unequip()
    {
        if (armor_ATK != 0)
        {
            PlayerManager.playerManager.spec.armor_ATK -= armor_ATK;
        }
        if (armor_PDEF != 0)
        {
            PlayerManager.playerManager.spec.armor_PDEF -= armor_PDEF;
        }
        if (armor_MDEF != 0)
        {
            PlayerManager.playerManager.spec.armor_MDEF -= armor_MDEF;
        }
        if (armor_STR != 0)
        {
            PlayerManager.playerManager.spec.armor_STR -= armor_STR;
        }
        if (armor_DEX != 0)
        {
            PlayerManager.playerManager.spec.armor_DEX -= armor_DEX;
        }
        if (armor_INT != 0)
        {
            PlayerManager.playerManager.spec.armor_INT -= armor_INT;
        }
        if (armor_HP != 0)
        {
            PlayerManager.playerManager.spec.armor_hp -= armor_HP;
            DungeonManager.dungeonManager.visualize_player_hp();
        }
    }

    public bool ret_had_skill()
    {
        if(ref_skill!=null)
        {
            return true;
        }
        return false;
    }

    public bool ret_skill_condition() // skill의 사용가능여부 반환
    {
        if(ref_skill!=null)
        {
            return ref_skill.ret_skill_condition();
        }
        return false;
    }

    public Skill ret_ref_skill()
    {
        return ref_skill;
    }

    public void use_skill()
    {
        if(ref_skill != null)
        {
            ref_skill.skill_function();
        }
    }

    public virtual void reset_skill()
    {
        if(ref_skill != null)
        {
            ref_skill.reset_skill_condition();
        }
    }
}
