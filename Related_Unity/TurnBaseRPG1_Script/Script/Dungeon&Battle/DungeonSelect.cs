using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class event_distribution
{
    public int number_of_event;
    public int min_position;
    public int max_position;

    public List<int> event_list;
}

[System.Serializable]
public class DungeonSelect
{
    [SerializeField] private string dungeon_name;

    [SerializeField] private int dungeon_size;
    [SerializeField] private int dungeon_level;

    [SerializeField] private event_distribution distribution_positive_event;
    [SerializeField] private event_distribution distribution_negative_event;
    [SerializeField] private event_distribution distribution_chaos_event;
    [SerializeField] private event_distribution distribution_nothing_event;

    [SerializeField] private event_distribution distribution_last_event;

    [SerializeField] public List<EnemyScriptableObject> enemy_list;
    [SerializeField] public List<EnemyScriptableObject> boss_list;

    public string ret_name()
    {
        return dungeon_name;
    }

    public int ret_dungeon_size()
    {
        return dungeon_size;
    }

    public int ret_dungeon_level()
    {
        return dungeon_level;
    }

    public event_distribution ret_positive_distribution()
    {
        return distribution_positive_event;
    }

    public event_distribution ret_negative_distribution()
    {
        return distribution_negative_event;
    }

    public event_distribution ret_chaos_distribution()
    {
        return distribution_chaos_event;
    }

    public event_distribution ret_nothing_distribution()
    {
        return distribution_nothing_event;
    }

    public event_distribution ret_last_distritubion()
    {
        return distribution_last_event;
    }

    public int ret_random_enemy_code()
    {
        return enemy_list[Random.Range(0, enemy_list.Count)].ret_code();
    }

    public int ret_random_boss_code()
    {
        return boss_list[Random.Range(0, boss_list.Count)].ret_code();
    }
}
