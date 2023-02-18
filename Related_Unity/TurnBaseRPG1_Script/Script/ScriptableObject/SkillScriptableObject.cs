using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skill_type
{
    attack, change
}

[CreateAssetMenu(menuName = "ScriptableObject/Skill")]
public class SkillScriptableObject : ScriptableObject
{
    [SerializeField] private bool use_turn_condtion;
    [SerializeField] private bool use_possible_number;

    [SerializeField] private int need_energy = 1;
    [SerializeField] private int turn_condition;
    [SerializeField] private int possible_number;

    public skill_type type;
    
    public string head;

    [TextArea]
    public string body;

    public bool ret_use_turn_condtion()
    {
        return use_turn_condtion;
    }

    public bool ret_use_possible_number()
    {
        return use_possible_number;
    }

    public int ret_need_energy()
    {
        return need_energy;
    }

    public int ret_turn_condtion()
    {
        return turn_condition;
    }

    public int ret_possible_number()
    {
        return possible_number;
    }
}
