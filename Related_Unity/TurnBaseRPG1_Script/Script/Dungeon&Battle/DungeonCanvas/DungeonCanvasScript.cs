using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DungeonCanvasScript : MonoBehaviour
{
    [SerializeField] private Dungeon_additional_UI additional_ui;
    [SerializeField] private GameObject room_clear_object;
    [SerializeField] private GameObject battle_object;
    [SerializeField] private GameObject event_object;
    [SerializeField] private Player_Death_in_Dungeon_UI player_death_object;
    [SerializeField] private Battle_Result_script battle_result;

    [SerializeField] private PlayerVisualScript player_visual_script;
    [SerializeField] private Player_Battle_UI player_battle_UI_script;

    [SerializeField] private Transform enemy_spawn_battle_position;
    [SerializeField] private Transform player_effected_position;
    [SerializeField] private Transform enemy_effected_position;
    private void OnEnable()
    {
        visualize_room_clear_obj();
        additional_ui.visualize_dungeon_detail_text();
        player_battle_UI_script.set_player_hp_UI(DungeonManager.dungeonManager.return_player_hp_in_dungeon(), PlayerManager.playerManager.spec.ret_battle_hp_int());
    }

    private void Start()
    {
        DungeonManager.dungeonManager.set_dungeon_canvas_script(this);
    }

    private void OnDestroy()
    {
        DungeonManager.dungeonManager.set_dungeon_canvas_script(null);
    }

    public void NextRoom_Button_Btn()
    {
        DungeonManager.dungeonManager.next_room();
    }

    public void change_player_visual_weapon(kind_of_weapon value)
    {
        player_visual_script.change_visual_related_equip(value);
    }

    public void visualize_room_clear_obj()
    {
        room_clear_object.SetActive(true);
        battle_object.SetActive(false);
        event_object.SetActive(false);
    }

    public void visualize_battle_obj()
    {
        room_clear_object.SetActive(false);
        battle_object.SetActive(true);
        event_object.SetActive(false);
    }

    public void visualize_event_obj()
    {
        room_clear_object.SetActive(false);
        battle_object.SetActive(false);
        event_object.SetActive(true);
    }

    public void unvisualize_all_obj()
    {
        room_clear_object.SetActive(false);
        battle_object.SetActive(false);
        event_object.SetActive(false);
    }

    public void set_enemy_effected_position(Transform value)
    {
        enemy_effected_position = value;
    }

    public bool ret_battle_obj_status()
    {
        return battle_object.activeSelf;
    }

    public Dungeon_additional_UI ret_additional_UI()
    {
        return additional_ui;
    }

    public Player_Battle_UI ret_player_Battle_UI()
    {
        return player_battle_UI_script;
    }

    public Vector3 ret_enemy_position()
    {
        return enemy_spawn_battle_position.position;
    }

    public Vector3 ret_player_effected_position()
    {
        return player_effected_position.position;
    }

    public Vector3 ret_enemy_effected_position()
    {
        return enemy_effected_position.position;
    }

    public Battle_Result_script ret_battle_result_script()
    {
        return battle_result;
    }

    public void death_Player_UI()
    {
        player_death_object.gameObject.SetActive(true);
    }

}
