using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character_Origin
{
    None, Warrior, Assassin, Wizard
}

[System.Serializable]
public class CharacterSpec
{
    public Character_Origin origin;

    public bool gameover_status = false;

    #region Spec
    public int start_hp;
    public int correction_hp = 0; // 보정체력
    public int temp_max_hp = 0; // 일시적 최대 체력
    public int amulet_max_hp = 0; // 부적을 통해 증가한 체력
    public int armor_hp = 0;
    // 체력 부분 ( max hp : start_hp + temp_max_hp + amulet_max_hp )

    public int start_ATK;
    public int correction_ATK = 0;
    public int temp_ATK = 0;
    public int amulet_ATK = 0;
    public int armor_ATK = 0;

    public int start_STR;
    public int correction_STR = 0;
    public int temp_STR = 0;
    public int amulet_STR = 0;
    public int armor_STR = 0;

    public int start_DEX = 0;
    public int correction_DEX = 0;
    public int temp_DEX = 0;
    public int amulet_DEX = 0;
    public int armor_DEX = 0;

    public int start_INT;
    public int correction_INT = 0;
    public int temp_INT = 0;
    public int amulet_INT = 0;
    public int armor_INT = 0;

    public int start_PDEF;
    public int correction_PDEF = 0;
    public int temp_PDEF = 0;
    public int amulet_PDEF = 0;
    public int armor_PDEF = 0;

    public int start_MDEF;
    public int correction_MDEF = 0;
    public int temp_MDEF = 0;
    public int amulet_MDEF = 0;
    public int armor_MDEF = 0;
    // 전투 관련 능력치들

    public int current_gold = 100;
    public int currect_soul = 200;

    public int current_turn = 1;
    public int max_turn = 10;
    public int current_chance = 1;
    public int max_chance = 5;

    public int current_stress = 0;
    public int max_stress = 100;

    public List<int> carried_amulet = new List<int>();
    // 소지 부적 및 스킬 번호

    public float dismiss_percent = 0.5f;
    public List<int> protected_amulet = new List<int>(new int[2]);

    #endregion

    public CharacterSpec(Character_Origin valued_origin)
    {
        origin = valued_origin;

        //current_gold = 99999;
        current_gold = 500;
        //currect_soul = 99999;
        currect_soul = 500;
        current_turn = 1;
        max_turn = 8;
        current_chance = 1;
        max_chance = 5;
        current_stress = 0;
        max_stress = 100;
        dismiss_percent = 0.3f;
        gameover_status = false;

        switch (origin)
        {
            case Character_Origin.Warrior:
                start_hp = 100;
                start_ATK = 15;
                start_STR = 20;
                start_DEX = 12;
                start_INT = 4;
                start_PDEF = 5;
                start_MDEF = 2;
                break;
            case Character_Origin.Assassin:
                start_hp = 70;
                start_ATK = 16;
                start_STR = 10;
                start_DEX = 20;
                start_INT = 8;
                start_PDEF = 4;
                start_MDEF = 3;
                break;
            case Character_Origin.Wizard:
                start_hp = 70;
                start_ATK = 13;
                start_STR = 4;
                start_DEX = 8;
                start_INT = 24;
                start_PDEF = 2;
                start_MDEF = 6;
                break;
            default:
                start_hp = 50;
                start_ATK = 15;
                start_STR = 9;
                start_DEX = 9;
                start_INT = 9;
                start_PDEF = 4;
                start_MDEF = 4;
                gameover_status = true;
                break;
        }

    }

    public CharacterSpec(CharacterSpec value)
    {
        origin = value.origin;
        gameover_status = value.gameover_status;

        start_hp = value.start_hp;
        correction_hp = value.correction_hp;
        temp_max_hp = value.temp_max_hp;
        amulet_max_hp = value.amulet_max_hp;

        start_ATK = value.start_ATK;
        correction_ATK = value.correction_ATK;
        temp_ATK = value.temp_ATK;
        amulet_ATK = value.amulet_ATK;
        armor_ATK = value.armor_ATK;

        start_STR = value.start_STR;
        correction_STR = value.correction_STR;
        temp_STR = value.temp_STR;
        amulet_STR = value.amulet_STR;
        armor_STR = value.armor_STR;

        start_DEX = value.start_DEX;
        correction_DEX = value.correction_DEX;
        temp_DEX = value.temp_DEX;
        amulet_DEX = value.amulet_DEX;
        armor_DEX = value.armor_DEX;

        start_INT = value.start_INT;
        correction_INT = value.correction_INT;
        temp_INT = value.temp_INT;
        amulet_INT = value.amulet_INT;
        armor_INT = value.armor_INT;

        start_PDEF = value.start_PDEF;
        correction_PDEF = value.correction_PDEF;
        temp_PDEF = value.temp_PDEF;
        amulet_PDEF = value.amulet_PDEF;
        armor_PDEF = value.armor_PDEF;

        start_MDEF = value.start_MDEF;
        correction_MDEF = value.correction_MDEF;
        temp_MDEF = value.temp_MDEF;
        amulet_MDEF = value.amulet_MDEF;
        armor_MDEF = value.armor_MDEF;

        current_gold = value.current_gold;
        currect_soul = value.currect_soul;

        current_turn = value.current_turn;
        max_turn = value.max_turn;
        current_chance = value.current_chance;
        max_chance = value.max_chance;

        current_stress = value.current_stress;
        max_stress = value.max_stress;

        carried_amulet = value.carried_amulet;

        dismiss_percent = value.dismiss_percent;
        protected_amulet = value.protected_amulet;
}

    #region return_Function

    public int ret_current_atk_int()
    {
        int ret = 0;
        ret = amulet_ATK + start_ATK + temp_ATK + armor_ATK;
        return ret;
    }

    public int ret_current_pdef_int()
    {
        int ret = 0;
        ret = amulet_PDEF + start_PDEF + temp_PDEF + armor_PDEF;
        return ret;
    }

    public int ret_current_mdef_int()
    {
        int ret = 0;
        ret = amulet_MDEF + start_MDEF + temp_MDEF + armor_MDEF;
        return ret;
    }

    public int ret_current_str_int()
    {
        int ret = 0;
        ret = amulet_STR + start_STR + temp_STR + armor_STR;
        return ret;
    }

    public int ret_current_dex_int()
    {
        int ret = 0;
        ret = amulet_DEX + start_DEX + temp_DEX + armor_DEX;
        return ret;
    }

    public int ret_current_int_int()
    {
        int ret = 0;
        ret = amulet_INT + start_INT + temp_INT + armor_INT;
        return ret;
    }

    public int ret_battle_hp_int()
    {
        int ret = 0;
        ret = amulet_max_hp + start_hp + temp_max_hp + armor_hp;
        return ret;
    }

    public int ret_amulet_pdef_int()
    {
        return amulet_PDEF;
    }

    public int ret_amulet_mdef_int()
    {
        return amulet_MDEF;
    }
    #endregion

    public void cal_gold(int value)
    {
        current_gold += value;
        if(current_gold < 0)
        {
            current_gold = 0;
        }
    }

    public void cal_soul(int value)
    {
        currect_soul += value;
        if(currect_soul < 0)
        {
            currect_soul = 0;
        }
    }

    public void cal_stress(int value)
    {
        current_stress += value;
        if(current_stress>max_stress)
        {
            current_stress = max_stress;
        }
        else if(current_stress<0)
        {
            current_stress = 0;
        }
    }

    public void death_set_hp()
    {
        start_hp += correction_hp;
        correction_hp = 0;
        temp_max_hp = 0;
    }

    public void death_set_atk()
    {
        start_ATK += correction_ATK;
        correction_ATK = 0;
        temp_ATK = 0;
    }

    public void death_set_pdef()
    {
        start_PDEF += correction_PDEF;
        correction_PDEF = 0;
        temp_PDEF = 0;
    }

    public void death_set_mdef()
    {
        start_MDEF += correction_MDEF;
        correction_MDEF = 0;
        temp_MDEF = 0;
    }

    public void death_set_str()
    {
        start_STR += correction_STR;
        correction_STR = 0;
        temp_STR = 0;
    }

    public void death_set_dex()
    {
        start_DEX += correction_DEX;
        correction_DEX = 0;
        temp_DEX = 0;
    }

    public void death_set_int()
    {
        start_INT += correction_INT;
        correction_INT = 0;
        temp_INT = 0;
    }
}
