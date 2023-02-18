using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Call : MonoBehaviour
{
    public void playerManager_save()
    {
        PlayerManager.playerManager.save_spec();
    }

    public void playerManager_death()
    {
        PlayerManager.playerManager.Player_death();
    }

    public void scenemove_call(string name)
    {
        SceneManagerCode.sceneManagerCode.Scene_move(name);
    }

    public void stat_canvas_visual()
    {
        PlayerManager.playerManager.set_active_stat_canvas(true);
        PlayerManager.playerManager.set_active_amuletcanvas(false);
        InventoryManager.inventoryManager.set_active_inventorycanvas(false);
    }

    public void all_canvas_unvisual()
    {
        PlayerManager.playerManager.set_active_stat_canvas(false);
        PlayerManager.playerManager.set_active_amuletcanvas(false);
        InventoryManager.inventoryManager.set_active_inventorycanvas(false);
    }

    public void amulet_canvas_visual()
    {
        PlayerManager.playerManager.set_active_amuletcanvas(true);
        InventoryManager.inventoryManager.set_active_inventorycanvas(false);
    }

    public void inventory_canvas_visual()
    {
        PlayerManager.playerManager.set_active_amuletcanvas(false);
        InventoryManager.inventoryManager.set_active_inventorycanvas(true);
    }

    public void inventory_inshop(bool value)
    {
        InventoryManager.inventoryManager.set_inshop(value);
    }

    public void game_exit()
    {
        Application.Quit();
    }

    public void clear_room_in_dungeon()
    {
        DungeonManager.dungeonManager.room_clear_dungeon();
    }

    public void next_room_move()
    {
        DungeonManager.dungeonManager.next_room();
    }

    public void battle_test_start()
    {
        DungeonManager.dungeonManager.start_battle();
    }

    public void button_click_play()
    {
        Util_Manager.utilManager.button_click_sound_play();
    }

    public void play_sfx(AudioClip value)
    {
        Util_Manager.utilManager.play_clip(value);
    }
}
