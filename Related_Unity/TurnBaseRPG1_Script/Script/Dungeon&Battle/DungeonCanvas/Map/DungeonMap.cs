using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonMap : MonoBehaviour
{
    [SerializeField] private Transform content_transform;

    [SerializeField] private List<GameObject> tile_list;

    [SerializeField] private GameObject nothing_tile;
    [SerializeField] private GameObject enemy_tile;
    [SerializeField] private GameObject boss_tile;
    [SerializeField] private GameObject event_tile;
    [SerializeField] private GameObject rest_tile;
    private bool map_init = false;
    private int pre_position = -1;

    [SerializeField] private TextMeshProUGUI dungeon_text;

    // Start is called before the first frame update
    void Start()
    {
        List<int> dungeon_struct = DungeonManager.dungeonManager.current_dungeon_struct.dungeon_content;
        // 0 : Nothing, 1 : Enemy, 2 : Positive Event, 3 : Negative Event, 4 : Chaos Event (Start is always 0), 5 : Dungeon Boss

        for (int i = 0;i<dungeon_struct.Count;i++)
        {
            if(dungeon_struct[i]==0)
            {
                tile_list.Add(Instantiate(nothing_tile, content_transform));
            }
            else if(dungeon_struct[i]==1)
            {
                tile_list.Add(Instantiate(enemy_tile, content_transform));
            }
            else if(dungeon_struct[i]==5)
            {
                tile_list.Add(Instantiate(boss_tile, content_transform));
            }
            else
            {
                tile_list.Add(Instantiate(event_tile, content_transform));
            }
        }

        map_init = true;
        dungeon_text.text = DungeonManager.dungeonManager.current_dungeon_struct.dungeon_name;
        player_position_visualize(DungeonManager.dungeonManager.current_dungeon_struct.current_dungeon_position);
    }

    private void OnEnable()
    {
        if(map_init==true)
        {
            player_position_visualize(DungeonManager.dungeonManager.current_dungeon_struct.current_dungeon_position);
        }
    }

    private void player_position_visualize(int player_pos)
    {
        if(pre_position != -1)
        {
            tile_list[pre_position].transform.GetChild(0).gameObject.SetActive(false);
        }

        for(int i =0;i<tile_list.Count;i++)
        {
            if(i<player_pos)
            {
                tile_list[i].GetComponent<Image>().color = new Color(0.33f, 0.33f, 0.33f);
            }
            else
            {
                tile_list[i].GetComponent<Image>().color = new Color(1.0f, 0.64f, 0.0f);
                tile_list[i].transform.GetChild(0).gameObject.SetActive(true);
                pre_position = i;
                break;
            }
        }
    }
}
