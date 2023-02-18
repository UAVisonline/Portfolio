using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dungeon_additional_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dungeon_detail_text;

    [SerializeField] private TextMeshProUGUI battle_turn_text;
    [SerializeField] private Animator battle_turn_UI_animator;
    [SerializeField] private Color player_color;
    [SerializeField] private Color enemy_color;

    public void visualize_dungeon_detail_text()
    {
        dungeon_detail_text.text = DungeonManager.dungeonManager.current_dungeon_struct.dungeon_name + '\n';
        dungeon_detail_text.text += "Level " + DungeonManager.dungeonManager.current_dungeon_struct.dungeon_level.ToString() + '\n';
        dungeon_detail_text.text += "ROOM    " + (1 + DungeonManager.dungeonManager.return_dungeon_position_player()).ToString();
    }

    public void battle_start_UI()
    {
        battle_turn_UI_animator.SetBool("Battle_var", true);
        battle_turn_text.text = "";
        battle_turn_text.color = Color.white;

        StartCoroutine(change_battle_turn_text("Battle Start", Color.white));
    }

    public void player_turn_UI(string value)
    {
        StartCoroutine(change_battle_turn_text(value, player_color));
    }

    public void enemy_turn_UI(string value)
    {
        StartCoroutine(change_battle_turn_text(value, enemy_color));
    }

    public void battle_end_UI()
    {
        battle_turn_UI_animator.SetBool("Battle_var", false);

        StartCoroutine(change_battle_turn_text("",Color.white));
    }

    IEnumerator change_battle_turn_text(string changed_value, Color value)
    {
        // Debug.Log(changed_value);

        while(battle_turn_text.text.Length>0)
        {
            battle_turn_text.text = battle_turn_text.text.Remove(battle_turn_text.text.Length - 1);
            yield return Util_Manager.utilManager.return_short_time();
        }

        battle_turn_text.color = value;

        for(int i =0;i<changed_value.Length;i++)
        {
            battle_turn_text.text += changed_value[i];
            yield return Util_Manager.utilManager.return_short_time();
        }

        yield break;
    }
}
