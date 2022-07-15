using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocaMaster : MonoBehaviour
{
    private static VocaMaster _vocamaster;

    public static VocaMaster vocaMaster // Singleton으로 해당 Script를 불러올 수 있음
    {
        get
        {
            if(_vocamaster==null)
            {
                _vocamaster = FindObjectOfType<VocaMaster>();
            }
            return _vocamaster;
        }
    }

    [SerializeField] private List<string> voca_head;
    private Dictionary<string, VocaObject> voca_body;

    [SerializeField] private int voca_index;
    [SerializeField] private int voca_detail_index;

    private void Awake()
    {
        //ES3.DeleteFile();

        if(ES3.KeyExists("Voca_head")==true) // EasySave를 이용해 이전에 영단어를 저장한 적이 있다면
        {
            voca_head = ES3.Load<List<string>>("Voca_head"); // 해당 단어 목록을 불러옴 (String으로만)
        }
        else
        {
            voca_head = new List<string>(); // 없으면 List를 생성
        }

        if(ES3.KeyExists("Voca_body")==true) // 이전에 영단어 세부사항을 저장한 적이 있다면
        {
            voca_body = ES3.Load<Dictionary<string, VocaObject>>("Voca_body"); // 세부 사항을 Dictionary형태로 불러옴
        }
        else
        {
            voca_body = new Dictionary<string, VocaObject>(); // 없으면 Dictionary를 생성
        }
    }

    private void Start()
    {

    }

    public void Save() // 저장기능(영단어 및 세부사항)
    {
        ES3.Save("Voca_head", voca_head);
        ES3.Save("Voca_body", voca_body);
    }

    public void body_Save() // 저장기능 (세부사항 변경에 관하여)
    {
        ES3.Save("Voca_body", voca_body);
    }

    public int get_index() // 현재 단어 page를 반환
    {
        return voca_index;
    }

    public int get_count() // 저장된 단어 갯수를 반환
    {
        return voca_head.Count;
    }

    public int get_detail_index() // 세부 index를 반환
    {
        return voca_detail_index;
    }

    public bool is_list_content(int index) // index 위치에 단어가 저장되있는지 반환
    {
        if(index >= voca_head.Count)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public string get_list_content(int index) // index 내 단어를 반환
    {
        return voca_head[index];
    }

    public string get_dicitionary_body(string value) // value 단어를 Dictionary에서 찾아서 뜻 반환
    {
        if(voca_body.ContainsKey(value))
        {
            return voca_body[value].body;
        }

        return "-----"; // 없는 경우 -----을 반환 (이 경우 코드가 꼬여 오류가 발생했음을 의미함)
    }

    public bool get_dictionary_bd_check(string value) // value 단어를 Dictionary에서 찾아서 시험 내 사용여부 반환
    {
        if (voca_body.ContainsKey(value))
        {
            return voca_body[value].check_it;
        }
        return true;
    }

    public float get_dictionary_level(string value) // 단어의 level 반환
    {
        if (voca_body.ContainsKey(value))
        {
            return voca_body[value].level;
        }

        return 0.0f;
    }

    public void set_dictionary_check(string value) // 해당 단어를 Test에 있어서 사용하고 있습니다
    {
        if(voca_body.ContainsKey(value))
        {
            voca_body[value].check_it = true;
        }
    }

    public void set_dictionary_level(string value, float bo_jung) // 해당 단어의 level값을 bo_jung 인자만큼 더해줍니다
    {
        if(voca_body.ContainsKey(value))
        {
            voca_body[value].level += bo_jung;

            if(voca_body[value].level>8.0f) // 최대 level 초과인 경우
            {
                voca_body[value].level = 8.0f;
            }
            else if(voca_body[value].level < 1.0f) // 최소 level 미만인 경우
            {
                voca_body[value].level = 1.0f;
            }
        }
    }

    public void set_index(int value) // 단어 Page index를 설정합니다
    {
        voca_index = value;
    }



    public List<int> get_level_index_list(float min_value,float max_value) // 최소 level value 및 최대 level value 사이의 단어에 대해 Index List 형태로 만들어서 반환
    {
        List<int> ret = new List<int>();
        for(int i =0; i<voca_head.Count; i++)
        {
            if(voca_body.ContainsKey(voca_head[i])==true) // Dictionary에 단어가 존재
            {
                if(voca_body[voca_head[i]].check_it==false) // 근데 해당 단어를 이미 Test에서 사용한 적도 없음
                {
                    if (min_value <= voca_body[voca_head[i]].level && voca_body[voca_head[i]].level <= max_value) // 게다가 level 조건도 만족함
                    {
                        ret.Add(i);
                    }
                }
            }
        }
        return ret;
    }

    public void uncheck_all() // 단어 내용(Voca head)를 참조해 실제 VocaObject의 check 여부를 전부 해제합니다
    {
        for(int i =0;i<voca_head.Count;i++)
        {
            if(voca_body.ContainsKey(voca_head[i]) == true)
            {
                voca_body[voca_head[i]].check_it = false;
            }
        }
    }

    public void set_detail_index(int value) // 세부 index를 설정
    {
        voca_detail_index = value;
    }

    public bool insert_voca(string hd, string bd) // 단어를 실제 Data상으로 집어넣습니다
    {
        if(voca_body.ContainsKey(hd)==true) // 넣고자 하는 단어가 이미 존재하면 false를 반환
        {
            return false;
        }
        VocaObject voca = ScriptableObject.CreateInstance<VocaObject>(); // VocaObject Instance를 생성
        voca.head = hd; 
        voca.body = bd;
        voca.level = 3.0f;
        voca.check_it = false;
        // 해당 Object 내부 값을 설정

        voca_head.Add(hd);
        voca_body.Add(hd, voca);
        // 실제 VocaMaster에서 이를 참조할 수 있도록 설정

        Save(); // 실제 Data 상 저장
        return true;
    }

    public bool delete_voca(string hd) // 단어를 실제 Data 상에서 삭제합니다
    {
        if(voca_body.ContainsKey(hd)==true)
        {
            voca_body.Remove(hd);
            voca_head.Remove(hd);
            Save();
            return true;
        }

        return false;
    }

    public void Game_end()
    {
        Application.Quit();
    }
}
