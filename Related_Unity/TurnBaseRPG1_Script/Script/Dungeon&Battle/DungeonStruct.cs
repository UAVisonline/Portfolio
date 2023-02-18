using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonStruct
{
    public string dungeon_name;
    public int current_dungeon_position;
    public List<int> dungeon_content = new List<int>(); // 0 : Nothing, 1 : Enemy, 2 : Positive Event, 3 : Negative Event, 4 : Chaos Event (Start is always 0), 5 : Dungeon Boss
    public List<int> dungeon_monster_code = new List<int>();
    public List<int> dungeon_event_code = new List<int>();

    public bool in_dungeon;

    public int saved_player_hp;

    public int dungeon_level;

    public DungeonStruct()
    {
        dungeon_name = "None Dungeon";
        current_dungeon_position = 0;

        dungeon_content = new List<int>();
        dungeon_monster_code = new List<int>();
        dungeon_event_code = new List<int>();

        in_dungeon = false;

        saved_player_hp = 0;

        dungeon_level = 1;
    }

    public DungeonStruct(DungeonSelect value)
    {
        dungeon_name = value.ret_name();

        int dungeon_length = value.ret_dungeon_size();
        current_dungeon_position = 0;

        dungeon_content.Clear();
        dungeon_monster_code.Clear();
        dungeon_event_code.Clear();

        dungeon_content.Add(0);
        dungeon_monster_code.Add(0);
        dungeon_event_code.Add(0);
        for(int i = 0 ; i < dungeon_length ; i++)
        {
            dungeon_content.Add(-1);
            dungeon_monster_code.Add(0);
            dungeon_event_code.Add(0);
        }

        event_distribution positive = value.ret_positive_distribution();
        event_distribution negative = value.ret_negative_distribution();
        event_distribution chaos = value.ret_chaos_distribution();
        event_distribution nothing = value.ret_nothing_distribution();
        event_distribution last = value.ret_last_distritubion();


        for (int i = 0 ; i < positive.number_of_event; i++)
        {
            int random_pos = Random.Range(positive.min_position, positive.max_position + 1);
            int event_code = positive.event_list[Random.Range(0, positive.event_list.Count)]; // event code 불러오기

            dungeon_content[random_pos] = 2;
            dungeon_event_code[random_pos] = event_code;
        }

        for (int i = 0; i < negative.number_of_event; i++)
        {
            int random_pos = Random.Range(negative.min_position, negative.max_position + 1);
            int event_code = negative.event_list[Random.Range(0, negative.event_list.Count)]; // event code 불러오기

            dungeon_content[random_pos] = 3;
            dungeon_event_code[random_pos] = event_code;
        }

        for (int i = 0; i < chaos.number_of_event; i++)
        {
            int random_pos = Random.Range(chaos.min_position, chaos.max_position + 1);
            int event_code = chaos.event_list[Random.Range(0, chaos.event_list.Count)]; // event code 불러오기

            dungeon_content[random_pos] = 4;
            dungeon_event_code[random_pos] = event_code;
        }
 
        for (int i = 0; i < nothing.number_of_event; i++)
        {
            int random_pos = Random.Range(nothing.min_position, nothing.max_position + 1);

            dungeon_content[random_pos] = 0;
            dungeon_event_code[random_pos] = 0;
        }

        for (int i = 0; i < last.number_of_event; i++)
        {
            int pos = dungeon_content.Count - 2;
            if(last.event_list.Count==0)
            {
                dungeon_content[pos] = -1;
                dungeon_event_code[pos] = 0;
            }
            else
            {
                int last_code = last.event_list[0];

                dungeon_content[pos] = 2;
                dungeon_event_code[pos] = last_code;
            }

        }
        
        for (int i =1;i<dungeon_content.Count;i++)
        {
            if(i < dungeon_content.Count-1)
            {
                if(dungeon_content[i]==-1)
                {
                    dungeon_content[i] = 1;

                    int monster_code = value.ret_random_enemy_code();
                    dungeon_monster_code[i] = monster_code;
                }
            }
            else
            {
                dungeon_content[i] = 5;

                int boss_code = value.ret_random_boss_code();
                dungeon_monster_code[i] = boss_code;
            }
        }
        
        saved_player_hp = PlayerManager.playerManager.spec.ret_battle_hp_int();
        in_dungeon = true;
        dungeon_level = value.ret_dungeon_level();
    }
}
