using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dungeon_situation
{
    battle, wait, dungeon_event
}

public enum battle_phase
{
    battle_start, player_turn, player_turn_end, enemy_turn, enemy_turn_end, battle_end, player_turn_start, enemy_turn_start
}

public enum attacked_type
{
    battle, trap, status
}

public enum attack_type
{
    physical, magic, none
}

public enum player_act_type
{
    normal_attack, defense, skill_attack, skill_stab, skill, using_item, passing
}

public class DungeonManager : MonoBehaviour // related Dungeon & Battle
{
    private static DungeonManager _dungeonManager;

    public static DungeonManager dungeonManager
    {
        get
        {
            if(_dungeonManager==null)
            {
                _dungeonManager = FindObjectOfType<DungeonManager>();
                if(_dungeonManager==null)
                {
                    Debug.LogError("Can't Find Dungeon(Battle) Manager");
                }
            }
            return _dungeonManager;
        }
    }

    public DungeonStruct current_dungeon_struct;
    [SerializeField] private string filename_dungeon;
    private DungeonSelect dungeon_select;

    [SerializeField] private int current_player_hp;
    [SerializeField] private Enemy enemy;

    public int damage;
    private int energy = 1;
    private int max_energy = 2;
    private int require_energy;
    private bool player_guard = false;
    private bool player_barrier = false;
    [SerializeField] private int dungeon_pos_in_game = 0;

    private attacked_type player_attacked_type;
    private attack_type player_attack_type;
    private attacked_type enemy_attacked_type;
    private attack_type enemy_attack_type;
    private player_act_type act_type;
    private int player_act_pos; // using Skill, Item
    public bool avoidance;

    private int dungeon_turn;
    private int player_act_number;

    private dungeon_situation situation = dungeon_situation.wait;
    private battle_phase battle_Phase = battle_phase.battle_end;

    private float command_time;

    [SerializeField] private List<GameObject> enemy_resources;
    private Dictionary<int, GameObject> enemy_dictionary = new Dictionary<int, GameObject>();

    [SerializeField] private List<GameObject> dungeon_event_resources;
    private Dictionary<int, GameObject> dungeon_event_dictionary = new Dictionary<int, GameObject>();

    [SerializeField] private DungeonCanvasScript canvas_Script = null;

    [SerializeField] private AudioClip fist_sound;
    [SerializeField] private GameObject fist_particle;

    private void Awake()
    {
        if(_dungeonManager==null)
        {
            _dungeonManager = this.GetComponent<DungeonManager>();
            DontDestroyOnLoad(this.gameObject);

            // Object Load - Enemy & Event
            for (int i = 0; i < enemy_resources.Count; i++)
            {
                EnemyScriptableObject temp = enemy_resources[i].GetComponent<Enemy>().ret_enemy_information();
                // Debug.Log(temp.name);
                enemy_dictionary.Add(temp.ret_code(), enemy_resources[i]);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if(SaveManager.saveManager.ExistFile(filename_dungeon) == true)
        {
            current_dungeon_struct = SaveManager.saveManager.LoadJsonFile<DungeonStruct>(filename_dungeon);
            current_player_hp = current_dungeon_struct.saved_player_hp;
            dungeon_pos_in_game = current_dungeon_struct.current_dungeon_position;
            Debug.Log("Find Dungeon struct");
        }
        else
        {
            init_dungeon_struct();
            Debug.Log("None Dungeon struct");
        }
    }

    private void Update()
    {
        if(situation == dungeon_situation.battle && canvas_Script != null)
        {
            battle_update();
        }
    }

    private void battle_update()
    {
        if(command_time > 0.0f)
        {
            command_time -= Time.deltaTime;
            return;
        }

        switch(battle_Phase)
        {
            case battle_phase.battle_start:
                battle_Phase = battle_phase.player_turn_start;
                command_time = 1.0f;
                break;
            case battle_phase.player_turn_start:
                battle_Phase = battle_phase.player_turn;
                canvas_Script.ret_additional_UI().player_turn_UI("Player Turn " + dungeon_turn.ToString());
                command_time = 0.2f;
                break;
            case battle_phase.player_turn:
                break;
            case battle_phase.player_turn_end:
                PlayerManager.playerManager.function_amulet(Amulet_timing.turn_end);
                command_time = 0.4f;
                battle_Phase = battle_phase.enemy_turn_start;
                break;
            case battle_phase.enemy_turn_start:
                command_time = 0.2f;
                canvas_Script.ret_additional_UI().enemy_turn_UI("Enemy Turn " + dungeon_turn.ToString());
                battle_Phase = battle_phase.enemy_turn;
                break;
            case battle_phase.enemy_turn:
                command_time = 0.6f;
                enemy.enemy_function();

                if(current_player_hp>0)
                {
                    battle_Phase = battle_phase.enemy_turn_end;
                }
                else
                {
                    battle_Phase = battle_phase.battle_end;
                }
                break;
            case battle_phase.enemy_turn_end:
                dungeon_turn += 1;
                energy += 1;
                if(energy > max_energy)
                {
                    energy = max_energy;
                }
                set_player_guard(false);
                battle_Phase = battle_phase.player_turn_start;
                command_time = 0.4f;
                break;
            case battle_phase.battle_end:
                situation = dungeon_situation.wait;
                canvas_Script.ret_additional_UI().battle_end_UI();
                set_player_barrier(false);
                set_player_guard(false);

                // 전투결과창 생성
                // room_clear_dungeon();
                return;
        }

        if (canvas_Script.ret_battle_obj_status() == false && command_time <= 0.0f)
        {
            canvas_Script.visualize_battle_obj();
        }
    }

    public void start_battle()
    {
        damage = 0;
        avoidance = false;
        set_player_guard(false);
        set_player_barrier(false);
        canvas_Script.unvisualize_all_obj();

        dungeon_turn = 1;
        player_act_number = 1;
        energy = 1;
        max_energy = 2;

        situation = dungeon_situation.battle;
        battle_Phase = battle_phase.battle_start;
        canvas_Script.ret_additional_UI().battle_start_UI();
        command_time = 0.25f;

        InventoryManager.inventoryManager.reset_skill();

        PlayerManager.playerManager.function_amulet(Amulet_timing.start_battle);
    }

    public void Player_act()
    {
        switch(act_type)
        {
            case player_act_type.normal_attack:
                if(InventoryManager.inventoryManager.player_weapon==null)
                {
                    Damage_to_enemy(attack_type.physical, attacked_type.battle, ret_unequiped_attack_damage());
                    Util_Manager.utilManager.play_clip(fist_sound);
                    make_particle_enemy_position(fist_particle);
                }
                else if(InventoryManager.inventoryManager.player_weapon.function_condition() == true)
                {
                    InventoryManager.inventoryManager.player_weapon.function();
                }
                break;
            case player_act_type.skill_attack:
                break;
            case player_act_type.skill_stab:
                break;
            case player_act_type.skill:
                InventoryManager.inventoryManager.use_skill(player_act_pos); // 기본적으로 Skill 처리에서 skill_attack, skill_stab 변환도 진행
                break;
            case player_act_type.defense:
                set_player_guard(true);
                battle_Phase = battle_phase.player_turn_end;
                break;
            case player_act_type.using_item:
                InventoryManager.inventoryManager.use_item(inventory_frame_enum.player_consumption, player_act_pos);
                break;
            case player_act_type.passing:
                battle_Phase = battle_phase.player_turn_end;
                break;
        }

        energy -= require_energy;
        player_act_number++;

        command_time = 1.0f;
        canvas_Script.unvisualize_all_obj();
    }

    public void set_act_type(player_act_type value)
    {
        act_type = value;
    }

    public void set_act_pos(int value)
    {
        player_act_pos = value;
    }

    public void Damage_to_Player(attack_type attack_type_value, attacked_type attacked_type_value, int damage_value) // Player Hurt by Monster
    {
        if(current_player_hp<= 0)
        {
            return;
        }

        damage = damage_value;
        avoidance = false;
        enemy_attack_type = attack_type_value;
        player_attacked_type = attacked_type_value;

        PlayerManager.playerManager.function_amulet(Amulet_timing.before_attacked);

        int pre_hp = current_player_hp;

        if(avoidance==false)
        {
            if(player_barrier==true)
            {
                set_player_barrier(false);
                damage = 0;
            }
            else 
            {
                if (player_guard == true)
                {
                    player_guard = false;
                    damage = Mathf.RoundToInt(damage * 0.5f);
                    /*
                    if (attack_type_value != attack_type.none)
                    {
                        damage = Mathf.RoundToInt(damage*0.5f);
                    }
                    */
                }

                if (attack_type_value == attack_type.physical)
                {
                    damage -= PlayerManager.playerManager.spec.ret_current_pdef_int();
                }
                else if (attack_type_value == attack_type.magic)
                {
                    damage -= PlayerManager.playerManager.spec.ret_current_mdef_int();
                }

                if (damage <= 0)
                {
                    damage = 1;
                }
                current_player_hp -= damage;
            }
            visualize_player_hp();
        }

        PlayerManager.playerManager.function_amulet(Amulet_timing.after_attacked);

        if(pre_hp >0 && current_player_hp <= 0)
        {
            Death_in_dungeon();
        }
    }

    public void Damage_to_enemy(attack_type attack_type_value, attacked_type attacked_type_value, int damage_value) // Player Attack Monster
    {
        if(enemy != null)
        {
            damage = damage_value;
            avoidance = false;
            player_attack_type = attack_type_value;
            enemy_attacked_type = attacked_type_value;

            PlayerManager.playerManager.function_amulet(Amulet_timing.before_attack);

            if (avoidance == false)
            {
                damage = enemy.Hurt(damage, player_attack_type);
            }
            //Debug.Log(damage_value + " " + damage);

            PlayerManager.playerManager.function_amulet(Amulet_timing.after_attack);
        }
    }

    public void heal_player(float percent_value,int fixed_value)
    {
        int max_hp = PlayerManager.playerManager.spec.ret_battle_hp_int();
        int pre_hp = current_player_hp;

        int heal_value = Mathf.RoundToInt(max_hp * percent_value) + fixed_value;

        current_player_hp += heal_value;

        if(current_player_hp>max_hp)
        {
            current_player_hp = max_hp;
        }
        visualize_player_hp();

        if (pre_hp > 0 && current_player_hp <= 0)
        {
            PlayerManager.playerManager.set_active_stat_canvas(false);
            PlayerManager.playerManager.set_active_amuletcanvas(false);
            InventoryManager.inventoryManager.set_active_inventorycanvas(false);

            Death_in_dungeon();
        }
    }

    public void heal_player_fixed(int value)
    {
        int max_hp = PlayerManager.playerManager.spec.ret_battle_hp_int();
        int pre_hp = current_player_hp;
        current_player_hp += value;

        if(current_player_hp > max_hp)
        {
            current_player_hp = max_hp;
        }
        visualize_player_hp();

        if (pre_hp > 0 && current_player_hp <= 0)
        {
            PlayerManager.playerManager.set_active_stat_canvas(false);
            PlayerManager.playerManager.set_active_amuletcanvas(false);
            InventoryManager.inventoryManager.set_active_inventorycanvas(false);

            Death_in_dungeon();
        }
    }

    public void room_clear_dungeon()
    {
        situation = dungeon_situation.wait;
        canvas_Script.visualize_room_clear_obj();

        current_dungeon_struct.current_dungeon_position = dungeon_pos_in_game;

        save_dungeon_struct();
        PlayerManager.playerManager.save_spec();
        InventoryManager.inventoryManager.save_inventory();
    }

    public void end_battle(int gold, int soul, List<Item_information> drop_item)
    {
        battle_Phase = battle_phase.battle_end;
        command_time = 0.15f;

        PlayerManager.playerManager.function_amulet(Amulet_timing.end_battle);

        if(canvas_Script!=null)
        {
            canvas_Script.ret_battle_result_script().set_drop_data(gold, soul, drop_item);
        }
    }

    public void next_room()
    {
        int next_pos = dungeon_pos_in_game + 1;

        if(situation != dungeon_situation.wait)
        {
            return;
        }

        if(next_pos < current_dungeon_struct.dungeon_content.Count)
        {
            dungeon_pos_in_game = next_pos;
            canvas_Script.ret_additional_UI().visualize_dungeon_detail_text();

            if (current_dungeon_struct.dungeon_content[next_pos] == 1 || current_dungeon_struct.dungeon_content[next_pos] == 5) // 전투 돌입
            {
                Debug.Log("Battle Start");
                int enemy_code = current_dungeon_struct.dungeon_monster_code[next_pos];

                if(enemy_dictionary.ContainsKey(enemy_code)==true)
                {
                    Instantiate(enemy_dictionary[enemy_code]);
                    start_battle();
                }
            }
            else if(current_dungeon_struct.dungeon_content[next_pos]==2 || current_dungeon_struct.dungeon_content[next_pos] == 3 || current_dungeon_struct.dungeon_content[next_pos] == 4) // 이벤트 돌입
            {
                Debug.Log("Event Start");
                room_clear_dungeon();
            }
            else
            {
                Debug.Log("Nothing happen");
                room_clear_dungeon();
            }
        }
        else
        {

            
            if(current_dungeon_struct.dungeon_level >= 3 && InventoryManager.inventoryManager.ret_end_condition()==true)
            {
                PlayerManager.playerManager.next_turn();
                InventoryManager.inventoryManager.save_inventory();
                Util_Manager.utilManager.bgm_instant_stop();

                SceneManagerCode.sceneManagerCode.Scene_move("Clear");
            }
            else
            {
                current_dungeon_struct.in_dungeon = false;
                PlayerManager.playerManager.next_turn();
                save_dungeon_struct();
                PlayerManager.playerManager.save_spec();
                InventoryManager.inventoryManager.save_inventory();

                SceneManagerCode.sceneManagerCode.Scene_move("Camp");
            }
        }
    }

    #region dungeon_select function

    public void save_dungeon_struct()
    {
        current_dungeon_struct.saved_player_hp = current_player_hp;
        SaveManager.saveManager.SaveJsonFile(filename_dungeon, current_dungeon_struct);
    }

    public void init_dungeon_struct()
    {
        current_dungeon_struct = new DungeonStruct();
        save_dungeon_struct();
    }

    public void making_dungeon_struct_and_Move_Scene()
    {
        current_dungeon_struct = new DungeonStruct(dungeon_select);
        current_player_hp = current_dungeon_struct.saved_player_hp;
        dungeon_pos_in_game = current_dungeon_struct.current_dungeon_position;
        save_dungeon_struct();

        situation = dungeon_situation.wait;
        SceneManagerCode.sceneManagerCode.Scene_move("Dungeon");
        // SceneLoad
    }

    public void set_dungeon_select(DungeonSelect value)
    {
        dungeon_select = value;
    }

    public void null_dungeon_select()
    {
        dungeon_select = null;
    }

    #endregion

    #region return function()

    public int return_dungeon_turn()
    {
        return dungeon_turn;
    }

    public int return_dungeon_position_player()
    {
        return dungeon_pos_in_game;
    }

    public int return_player_hp_in_dungeon()
    {
        return current_player_hp;
    }

    public float ret_player_hp_ratio()
    {
        return ((float)current_player_hp / (float)PlayerManager.playerManager.spec.ret_battle_hp_int());
    }

    public int return_enemy_hp_in_dungeon()
    {
        return enemy.ret_enemy_hp();
    }

    public attacked_type ret_player_attacked()
    {
        return player_attacked_type;
    }

    public attacked_type ret_enemy_attacked()
    {
        return enemy_attacked_type;
    }

    public attack_type ret_player_attack()
    {
        return player_attack_type;
    }

    public attack_type ret_ememy_attack()
    {
        return enemy_attack_type;
    }

    public player_act_type ret_player_act()
    {
        return act_type;
    }

    public dungeon_situation ret_current_situation()
    {
        return situation;
    }

    public bool ret_in_dungeon()
    {
        return current_dungeon_struct.in_dungeon;
    }

    public bool ret_energy_status()
    {
        if(energy>0)
        {
            return true;
        }
        return false;
    }

    public int ret_energy()
    {
        return energy;
    }

    public string ret_energy_status_string()
    {
        string ret = energy.ToString() + " / " + max_energy.ToString();
        return ret;
    }

    public int ret_current_damage()
    {
        return damage;
    }

    private int ret_unequiped_attack_damage()
    {
        return PlayerManager.playerManager.spec.ret_current_atk_int();
    }

    public int ret_player_act_number()
    {
        return player_act_number;
    }

    public int ret_player_standard_damage()
    {
        if(InventoryManager.inventoryManager.player_weapon != null)
        {
            return InventoryManager.inventoryManager.player_weapon.ret_weapon_damage();
        }
        return ret_unequiped_attack_damage();
    }

    public bool ret_player_barrier()
    {
        return player_barrier;
    }

    public bool ret_player_guard()
    {
        return player_guard;
    }

    public Vector3 ret_enemy_position()
    {
        if(canvas_Script!=null)
        {
            return canvas_Script.ret_enemy_position();
        }
        return Vector3.zero;
    }

    public bool is_enemy()
    {
        if(enemy!=null)
        {
            return true;
        }
        return false;
    }

    #endregion

    public void make_particle_enemy_position(GameObject particle)
    {
        if(particle!=null)
        {
            Instantiate(particle, canvas_Script.ret_enemy_effected_position(), particle.transform.rotation);
        }
    }

    public void make_particle_player_position(GameObject particle)
    {
        if(particle!=null)
        {
            Instantiate(particle, canvas_Script.ret_player_effected_position(), particle.transform.rotation);
        }
    }

    public void set_require_energy(int value)
    {
        require_energy = value;
    }

    public void set_dungeon_canvas_script(DungeonCanvasScript value)
    {
        canvas_Script = value;
    }

    public DungeonCanvasScript ret_dungeon_canvas_script()
    {
        return canvas_Script;
    }

    public void player_equip_visualize(kind_of_weapon value)
    {
        if(canvas_Script != null)
        {
            canvas_Script.change_player_visual_weapon(value);
        }
    }

    public void set_enemy_script(Enemy value,Transform pos)
    {
        enemy = value;
        canvas_Script.set_enemy_effected_position(pos);
    }

    public void null_enemy_script()
    {
        enemy = null;
    }

    public void enemy_solution_plus()
    {
        if(enemy!=null)
        {
            enemy.plus_solution();
        }
    }

    public void set_player_barrier(bool value)
    {
        player_barrier = value;
        if(canvas_Script!=null)
        {
            canvas_Script.ret_player_Battle_UI().visualize_player_barrier_icon(value);
        }
    }

    public void visualize_player_hp()
    {
        if (canvas_Script != null)
        {
            if(current_player_hp > PlayerManager.playerManager.spec.ret_battle_hp_int())
            {
                current_player_hp = PlayerManager.playerManager.spec.ret_battle_hp_int();
            }

            canvas_Script.ret_player_Battle_UI().set_player_hp_UI(current_player_hp, PlayerManager.playerManager.spec.ret_battle_hp_int());
        }
    }

    public void set_player_guard(bool value)
    {
        player_guard = value;
        if (canvas_Script != null)
        {
            canvas_Script.ret_player_Battle_UI().visualize_player_guard_icon(value);
        }
    }

    public void Death_in_dungeon()
    {
        current_dungeon_struct.in_dungeon = false;
        save_dungeon_struct();

        PlayerManager.playerManager.Player_death();
        InventoryManager.inventoryManager.save_inventory();

        canvas_Script.death_Player_UI();
        // SceneManagerCode.sceneManagerCode.Scene_move("Camp");
    }
}
