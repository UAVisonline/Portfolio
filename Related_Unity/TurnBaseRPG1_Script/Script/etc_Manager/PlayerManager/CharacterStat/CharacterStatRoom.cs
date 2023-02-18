using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatRoom : MonoBehaviour
{
    [SerializeField] private Text hp_text;
    [SerializeField] private Text ATK_text;
    [SerializeField] private Text PDEF_text;
    [SerializeField] private Text MDEF_text;
    [SerializeField] private Text STR_text;
    [SerializeField] private Text DEX_text;
    [SerializeField] private Text INT_text;
    [SerializeField] private Text Gold_text;
    [SerializeField] private Text Soul_text;

    private void OnEnable()
    {
        refresh_spec();
    }

    public void refresh_spec()
    {
        if(DungeonManager.dungeonManager.current_dungeon_struct.in_dungeon==false)
        {
            hp_text.text = "Start HP : " + PlayerManager.playerManager.spec.ret_battle_hp_int().ToString();
        }
        else
        {
            hp_text.text = "HP : " + DungeonManager.dungeonManager.return_player_hp_in_dungeon() + " (MAX HP : " + PlayerManager.playerManager.spec.ret_battle_hp_int().ToString() + ")";
        }

        ATK_text.text = PlayerManager.playerManager.spec.ret_current_atk_int().ToString();
        PDEF_text.text = PlayerManager.playerManager.spec.ret_current_pdef_int().ToString();
        MDEF_text.text = PlayerManager.playerManager.spec.ret_current_mdef_int().ToString();
        STR_text.text = PlayerManager.playerManager.spec.ret_current_str_int().ToString();
        DEX_text.text = PlayerManager.playerManager.spec.ret_current_dex_int().ToString();
        INT_text.text = PlayerManager.playerManager.spec.ret_current_int_int().ToString();
        Gold_text.text = "Gold : " + PlayerManager.playerManager.spec.current_gold.ToString();
        Soul_text.text = "Soul : " + PlayerManager.playerManager.spec.currect_soul.ToString();
    }
}
