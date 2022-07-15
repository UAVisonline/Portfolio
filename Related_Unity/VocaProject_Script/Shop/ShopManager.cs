using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private static ShopManager _shopmanager;

    public static ShopManager shopmanager // Singleton 형태로
    {
        get
        {
            if (_shopmanager == null)
            {
                _shopmanager = FindObjectOfType<ShopManager>();
            }
            return _shopmanager;
        }
    }

    [SerializeField] private Material player_material; //실제 Player Material
    [SerializeField] private Material shop_material; // 상점에서만 사용할 Player Material

    [SerializeField] private List<ColorPalate> colors; // ColorPalete ScriptableObject 항목들
    [SerializeField] private List<ShopColorPalate> palates; // 이를 표시해 줄 UI

    [SerializeField] private List<SkillScriptableObject> final_skill; // Skill ScriptableObject 항목들
    [SerializeField] private List<SkillCategory> skill_categories; // 이를 표시해 줄 UI

    private int coin; // 현재 소유 money

    private int equipment_index; // 현재 장착한 color index
    private int equiment_skill; // 현재 장착한 Skill index

    private void Awake()
    {
        //ES3.DeleteFile();

        for(int i =0;i<colors.Count;i++) // color object 배열에 접근
        {
            //ES3.DeleteKey("_palate" + colors[i].color_name);
            //ES3.DeleteKey("_palate" + colors[i].color_name + "_purchased");
            //ES3.DeleteKey("_palate" + colors[i].color_name + "_equipment");

            if (ES3.KeyExists("_palate"+colors[i].color_name)==true) // 해당 color가 이미 Data상 저장되어 있다면
            {
                colors[i].purchased = ES3.Load<bool>("_palate" + colors[i].color_name + "_purchased"); // 구매 여부를 반환
                colors[i].equipment = ES3.Load<bool>("_palate" + colors[i].color_name + "_equipment"); // 장착 여부를 반환
            }
            else // 아닌 경우에 대해서는
            {
                if(colors[i].money==0) // 해당 color 가격이 0인경우
                {
                    colors[i].equipment = true; // 장착함
                    colors[i].purchased = true; // 구매함
                }
                else // 0이 아닌경우
                {
                    colors[i].equipment = false;
                    colors[i].purchased = false;
                }
                ES3.Save("_palate" + colors[i].color_name,true);
                ES3.Save("_palate" + colors[i].color_name + "_purchased", colors[i].purchased);
                ES3.Save("_palate" + colors[i].color_name + "_equipment", colors[i].equipment);
                // 이를 Data 상 저장하도록 함
            }
        }
        for(int i =0;i<colors.Count;i++)
        {
            if(colors[i].equipment==true) // 장착된 Color에 대하여
            {
                equipment_index = i; //장착 Color index를 설정 (중복 시 앞에 있는 놈이 설정 됨)
                applied_shader(); // 이를 이용해 Material Shader 설정
                break;
            }
        }

        for(int i =0;i<final_skill.Count;i++) // Skill 배열에 접근
        {
            //ES3.DeleteKey("_skill" + final_skill[i].skill_name);
            //ES3.DeleteKey("_skill" + final_skill[i].skill_name + "_purchased");
            //ES3.DeleteKey("_skill" + final_skill[i].skill_name + "_equipment");

            if (ES3.KeyExists("_skill"+final_skill[i].skill_name)==true) // Data 상 해당 Skill이 이미 저장되있는 경우
            {
                final_skill[i].purchased = ES3.Load<bool>("_skill" + final_skill[i].skill_name + "_purchased");
                final_skill[i].equipment = ES3.Load<bool>("_skill" + final_skill[i].skill_name + "_equipment");
            }
            else // 그렇지 않은 경우
            {
                if(final_skill[i].money==0)
                {
                    final_skill[i].equipment = true;
                    final_skill[i].purchased = true;
                }
                else
                {
                    final_skill[i].equipment = false;
                    final_skill[i].purchased = false;
                }
                ES3.Save("_skill" + final_skill[i].skill_name, true);
                ES3.Save("_skill" + final_skill[i].skill_name + "_purchased", final_skill[i].purchased);
                ES3.Save("_skill" + final_skill[i].skill_name + "_equipment", final_skill[i].equipment);
            }
        }
        for(int i =0;i<final_skill.Count;i++)
        {
            if(final_skill[i].equipment==true)
            {
                equiment_skill = i; // 장착 Skill index를 설정
                break;
            }
        }

        ///ES3.DeleteKey("Coin");
        if (ES3.KeyExists("Coin") == true) // 현재 돈을 얼마나 가지고 있는지 Data가 있으면
        {
            coin = ES3.Load<int>("Coin");
        }
        else // 없는 경우 (처음 단어장을 시작하는 경우일 것이다)
        {
            coin = 15000;
            ES3.Save<int>("Coin", coin);
        }
    }

    public int get_color_index(string name) // 이름 값에 해당하는 color를 배열에서 찾아 index 반환 (UI 설정 상 필요)
    {
        for(int i =0;i<colors.Count;i++)
        {
            if(colors[i].color_name==name)
            {
                return i;
            }
        }
        return -1;
    }

    public int get_skill_index(string name) // 이름 값에 해당하는 skill을 배열에서 찾아 index 반환 (UI 설정 상 필요)
    {
        for(int i =0;i<final_skill.Count;i++)
        {
            if(final_skill[i].skill_name==name)
            {
                return i;
            }
        }
        return -1;
    }

    public void get_skill_object()
    {
        //return final_skill[equiment_skill].
    }

    public bool get_purchased(int index)
    {
        return colors[index].purchased;
    }

    public ColorPalate get_palate_information(int index)
    {
        return colors[index];
    }

    public bool get_equimented(int index)
    {
        return colors[index].equipment;
    }

    public int get_coin()
    {
        return coin;
    }

    public void plus_coin(int value)
    {
        coin += value;
        ES3.Save<int>("Coin", coin);
    }

    public bool minus_coin(int value)
    {
        if (coin >= value)
        {
            coin -= value;
            ES3.Save<int>("Coin", coin);
            return true;
        }
        return false;
    }

    public GameObject get_director()
    {
        return final_skill[equiment_skill].director;
    }

    public void purchased_color(int index) // index에 해당하는 Color Skin 구매
    {
        colors[index].purchased = true;
        colors[index].equipment = true; // 새 Color 구매 및 장착
        colors[equipment_index].equipment = false; // 기존 장착 Color 해제


        ES3.Save("_palate" + colors[index].color_name + "_purchased", colors[index].purchased);
        ES3.Save("_palate" + colors[index].color_name + "_equipment", colors[index].equipment);
        ES3.Save("_palate" + colors[equipment_index].color_name + "_equipment", colors[equipment_index].equipment);
        // Data 저장

        for(int i =0;i<palates.Count;i++)
        {
            if(palates[i].get_index()==index || palates[i].get_index() == equipment_index)
            {
                Debug.Log(i);
                palates[i].update_palate(); // 이 기능 사실 필요한게 맞는건가??? (어차피 위에서 ColorPalete ScriptableObject 값을 변경했으니까)
                palates[i].information_init();
                // UI 정보 시각화
            }
        }
        equipment_index = index;

        shop_shader(equipment_index); // 상점 내 Player Material 설정
        applied_shader(); // Player Material 설정
    }

    public void equipment_color(int index) // index에 해당하는 Color Skin 장착 (위에서 구매 기능만 제거)
    {
        colors[index].equipment = true;
        colors[equipment_index].equipment = false;
        ES3.Save("_palate" + colors[index].color_name + "_equipment", colors[index].equipment);
        ES3.Save("_palate" + colors[equipment_index].color_name + "_equipment", colors[equipment_index].equipment);

        for (int i = 0; i < palates.Count; i++)
        {
            if (palates[i].get_index() == index || palates[i].get_index() == equipment_index)
            {
                Debug.Log(i);
                palates[i].update_palate(); // 이 기능 사실 필요한게 맞는건가???
                palates[i].information_init();
            }
        }
        equipment_index = index;

        shop_shader(equipment_index);
        applied_shader();
    }

    public void purchased_skill(int index) // index에 해당하는 SkillMode 구매
    {
        final_skill[index].purchased = true;
        final_skill[index].equipment = true;
        final_skill[equiment_skill].equipment = false;

        ES3.Save("_skill" + final_skill[index].skill_name + "_purchased", final_skill[index].purchased);
        ES3.Save("_skill" + final_skill[index].skill_name + "_equipment", final_skill[index].equipment);
        ES3.Save("_skill" + final_skill[equiment_skill].skill_name + "_equipment", final_skill[equiment_skill].equipment);

        for(int i =0;i<skill_categories.Count;i++)
        {
            if(skill_categories[i].get_index()==index || skill_categories[i].get_index() == equiment_skill)
            {
                skill_categories[i].information_init();
            }
        }
        equiment_skill = index;
    }

    public void equipment_skill(int index) // index에 해당하는 SkillMode 장착
    {
        final_skill[index].equipment = true;
        final_skill[equiment_skill].equipment = false;

        ES3.Save("_skill" + final_skill[index].skill_name + "_equipment", final_skill[index].equipment);
        ES3.Save("_skill" + final_skill[equiment_skill].skill_name + "_equipment", final_skill[equiment_skill].equipment);

        for (int i = 0; i < skill_categories.Count; i++)
        {
            if (skill_categories[i].get_index() == index || skill_categories[i].get_index() == equiment_skill)
            {
                skill_categories[i].information_init();
            }
        }
        equiment_skill = index;
    }

    public void enter_btn() // 상점 돌입 시 (현재 장착중인 index Color로 Shop Shader 설정)
    {
        shop_shader(equipment_index);
    }

    public void shop_shader(int index)
    {
        shop_material.SetColor("_ColorChangeNewCol", colors[index].cape_color);
        shop_material.SetColor("_ColorChangeNewCol2", colors[index].body_color);
    }

    private void applied_shader() // 플레이어 Material 반영
    {
        player_material.SetColor("_ColorChangeNewCol", colors[equipment_index].cape_color);
        player_material.SetColor("_ColorChangeNewCol2", colors[equipment_index].body_color);
    }
}
