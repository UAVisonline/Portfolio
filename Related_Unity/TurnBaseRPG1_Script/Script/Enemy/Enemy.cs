using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyScriptableObject enemy_information;
    protected CharacterSpec enemy_spec;

    [SerializeField] protected int current_hp;
    protected int remain_life;
    protected int enemy_function_pos = 0;
    protected int solution_var = 0;

    [SerializeField] protected float ATK_correlation;
    [SerializeField] protected float STR_correlation;
    [SerializeField] protected float DEX_correlation;
    [SerializeField] protected float INT_correlation;

    [SerializeField] protected Enemy_UI enemy_UI;
    [SerializeField] protected Transform effected_position;

    [SerializeField] protected AudioClip bgm;

    protected virtual void Start()
    {
        enemy_spec = new CharacterSpec(enemy_information.enemy_spec);
        remain_life = enemy_information.ret_alive_number();
        current_hp = enemy_spec.ret_battle_hp_int();
        enemy_UI = this.GetComponentInChildren<Enemy_UI>();

        if(enemy_UI!=null)
        {
            enemy_UI.visualize(current_hp, current_hp);
        }

        DungeonManager.dungeonManager.set_enemy_script(this,effected_position);
        this.transform.position = DungeonManager.dungeonManager.ret_dungeon_canvas_script().ret_enemy_position();

        if(bgm!=null)
        {
            Util_Manager.utilManager.bgm_play(bgm);
        }
    }

    public bool ret_enemy_alive()
    {
        if(current_hp > 0)
        {
            return true;
        }
        return false;
    }

    public int ret_enemy_hp()
    {
        return current_hp;
    }

    public float ret_enemy_ratio()
    {
        return (float)(current_hp) / (float)(enemy_spec.ret_battle_hp_int());
    }

    public CharacterSpec ret_enemy_spec()
    {
        return enemy_spec;
    }

    public EnemyScriptableObject ret_enemy_information()
    {
        return enemy_information;
    }

    public void plus_solution()
    {
        solution_var++;
    }

    protected int ret_enemy_damage()
    {
        float ret = enemy_spec.ret_current_atk_int() * ATK_correlation;
        ret += enemy_spec.ret_current_str_int() * STR_correlation;
        ret += enemy_spec.ret_current_dex_int() * DEX_correlation;
        ret += enemy_spec.ret_current_int_int() * INT_correlation;

        return Mathf.RoundToInt(ret);
    }

    public virtual int Hurt(int value, attack_type attack_type_value)
    {
        if(attack_type_value == attack_type.physical)
        {
            value -= enemy_spec.ret_current_pdef_int();
        }
        else if(attack_type_value == attack_type.magic)
        {
            value -= enemy_spec.ret_current_mdef_int();
        }

        if (value <= 0)
        {
            value = 1;
        }

        current_hp -= value;
        if (enemy_UI != null)
        {
            enemy_UI.visualize(current_hp, enemy_spec.ret_battle_hp_int());
        }
        check_dead();

        return value;
    }

    protected void heal(float ratio)
    {
        int value = Mathf.RoundToInt(enemy_spec.ret_battle_hp_int() * ratio);
        current_hp += value;

        if(current_hp > enemy_spec.ret_battle_hp_int())
        {
            current_hp = enemy_spec.ret_battle_hp_int();
        }

        if (enemy_UI != null)
        {
            enemy_UI.visualize(current_hp, enemy_spec.ret_battle_hp_int());
        }
    }

    protected virtual void check_dead()
    {
        if (current_hp <= 0)
        {
            if (remain_life > 0)
            {
                remain_life--;
                current_hp = enemy_spec.ret_battle_hp_int();
                enemy_UI.visualize(current_hp, enemy_spec.ret_battle_hp_int());
            }
            else
            {
                if (bgm != null)
                {
                    Util_Manager.utilManager.bgm_play(null);
                }

                // Item 및 Gold, Soul Drop
                int gold = enemy_spec.current_gold;
                int soul = enemy_spec.currect_soul;
                List<Item_information> drop = enemy_information.ret_drop_item_information();

                DungeonManager.dungeonManager.end_battle(gold,soul,drop);
                Destroy(this.gameObject);
            }
        }
    }

    public virtual void enemy_function()
    {
        Debug.Log( this.gameObject.name + " Doing " + enemy_function_pos + " Act");
        enemy_function_pos++;
    }
}
