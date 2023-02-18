using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] SkillScriptableObject skill_information;

    private int need_energy;
    private int turn_condition;
    private int number_of_skill_possible;

    [SerializeField] protected attack_type attack_type;

    public virtual void reset_skill_condition()
    {
        need_energy = skill_information.ret_need_energy();
        turn_condition = skill_information.ret_turn_condtion();
        number_of_skill_possible = skill_information.ret_possible_number();
    }

    public bool ret_skill_condition()
    {
        if(DungeonManager.dungeonManager.ret_energy() >= need_energy)
        {
            if(skill_information.ret_use_turn_condtion() == true && DungeonManager.dungeonManager.return_dungeon_turn() < turn_condition)
            {
                return false;
            }

            if(skill_information.ret_use_possible_number() == true && number_of_skill_possible <= 0)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public virtual void skill_function() // pre function
    {
        if(skill_information.type == skill_type.attack)
        {
            DungeonManager.dungeonManager.set_act_type(player_act_type.skill_attack);
        }
        else
        {
            DungeonManager.dungeonManager.set_act_type(player_act_type.skill_stab);
        }

        if(skill_information.ret_use_turn_condtion()==true)
        {
            turn_condition = DungeonManager.dungeonManager.return_dungeon_turn() + skill_information.ret_turn_condtion();
        }

        if(skill_information.ret_use_possible_number()==true)
        {
            number_of_skill_possible--;
        }

        // Debug.Log(skill_information.name);
    }

    public SkillScriptableObject ret_skill_information()
    {
        return skill_information;
    }

    public int ret_need_energy()
    {
        return need_energy;
    }

    public int ret_possible_number()
    {
        return number_of_skill_possible;
    }

    public int ret_turn_condtion()
    {
        return turn_condition;
    }

    public void set_turn_condition(int value)
    {
        turn_condition = value;
    }
}
