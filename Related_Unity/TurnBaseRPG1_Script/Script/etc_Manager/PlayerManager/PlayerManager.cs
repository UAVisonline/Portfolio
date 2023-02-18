using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _playerManager;

    public static PlayerManager playerManager
    {
        get
        {
            if(_playerManager==null)
            {
                _playerManager = FindObjectOfType<PlayerManager>();
                if(_playerManager==null)
                {
                    Debug.LogError("Can't Find PlayerManager");
                }
            }
            return _playerManager;
        }
    }

    [SerializeField] private string filename_spec;
    public CharacterSpec spec;
    [SerializeField] private int ref_chance;

    [SerializeField] private List<BaseAmuletScript> total_amulet;
    [SerializeField] private GameObject amulet_canvas;
    [SerializeField] private GameObject player_stat_canvas;

    private List<BaseAmuletScript> immediately_amulet = new List<BaseAmuletScript>(); // 즉시 효과 발동 (전투 중 사용 X)
    private List<BaseAmuletScript> start_battle_amulet = new List<BaseAmuletScript>(); // 전투 시작
    private List<BaseAmuletScript> before_attack_amulet = new List<BaseAmuletScript>(); // 공격 하기 전
    private List<BaseAmuletScript> after_attack_amulet = new List<BaseAmuletScript>(); // 공격 한 후
    private List<BaseAmuletScript> before_attacked_amulet = new List<BaseAmuletScript>(); // 공격 받기 전
    private List<BaseAmuletScript> after_attacked_amulet = new List<BaseAmuletScript>(); // 공격 받은 후
    private List<BaseAmuletScript> turn_end_amulet = new List<BaseAmuletScript>(); // 전투 턴 종료
    private List<BaseAmuletScript> battle_end_amulet = new List<BaseAmuletScript>(); // 전투 종료

    private List<BaseAmuletScript> life_end_amulet = new List<BaseAmuletScript>(); // 이번 생 종료
    // 플레이어 상태에 따른 부적배열

    private List<Amulet_information> protectd_amulet_information = new List<Amulet_information>();

    private Dictionary<int,int> specific_amulet_number = new Dictionary<int, int>(); // 현재 플레이어가 가진 Amulet을 상세하게 몇개 가졌는지를 저장

    [SerializeField] private string filename_schedule;
    public ScheduleInformation schedule_information;

    private void Awake()
    {
        if(_playerManager == null)
        {
            _playerManager = FindObjectOfType<PlayerManager>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(SaveManager.saveManager.ExistFile(filename_spec)==true)
        {
            Debug.Log("Find Character Spec file");
            spec = SaveManager.saveManager.LoadJsonFile<CharacterSpec>(filename_spec); //Player Spec Data Load

            for(int i =0;i<spec.carried_amulet.Count;i++) // Spec 내 Amulet 소지시작
            {
                AmuletManager.amuletmanager.insert_carried_amulet(spec.carried_amulet[i]);
            }

            protectd_amulet_information.Add(null);
            protectd_amulet_information.Add(null);
            for (int i =0;i<spec.protected_amulet.Count;i++) // spec 내 protected Amulet 설정 시작
            {
                if(spec.protected_amulet[i]!=0)
                {
                    protectd_amulet_information[i] = AmuletManager.amuletmanager.get_amulet_information(spec.protected_amulet[i]);
                }
            }

        }
        else
        {
            spec = new CharacterSpec(Character_Origin.None);
            Debug.Log("Can't Find Character Spec file");
        }


        if (SaveManager.saveManager.ExistFile(filename_schedule) == true) // 파일 스케줄 데이터 Load
        {
            Debug.Log("Find Schedule information file");
            schedule_information = SaveManager.saveManager.LoadJsonFile<ScheduleInformation>(filename_schedule);
        }
        else
        {
            schedule_information_init_and_save();
        }

        ref_chance = spec.current_chance;
    }

    public void make_spec(int value) // make new Character
    {
        while(total_amulet.Count>0)
        {
            dismiss_amulet(total_amulet[0]); // 기존 Amulet 전체 삭제
        }

        protectd_amulet_information.Clear();
        protectd_amulet_information.Add(null);
        protectd_amulet_information.Add(null);

        specific_amulet_number.Clear();
        spec = new CharacterSpec((Character_Origin)value); // 캐릭터 스펙 초기화

        schedule_information_init_and_save(); // 스케줄 정보 초기화 및 저장
        save_spec(); //  스펙 저장

        ref_chance = spec.current_chance;
    }

    public void save_spec()
    {
        SaveManager.saveManager.SaveJsonFile(filename_spec, spec);
    }

    #region Amulet_function_in_PlayerManager
    public void insert_amulet(BaseAmuletScript value) // Amulet Insert
    {
        if (specific_amulet_number.ContainsKey(value.information.code) == false) // 현재 캐릭터가 처음 얻는 Amulet이면
        {
            specific_amulet_number.Add(value.information.code, 1);
        }
        else // 이미 소지하고 있는 Amulet이라면
        {
            specific_amulet_number[value.information.code] += 1;
        }

        total_amulet.Add(value);
    }

    public void dismiss_amulet(BaseAmuletScript value) // Amulet 해제
    {
        if (specific_amulet_number.ContainsKey(value.information.code) == false) // 현재 캐릭터가 가지고 있지 않은 Amulet이라면 (현실적으로 불가능) 
        {
            specific_amulet_number.Add(value.information.code, 0);
        }
        else
        {
            specific_amulet_number[value.information.code] -= 1; // 해당 Amulet 소지숫자에서 하나 벗어남
            if (specific_amulet_number[value.information.code] <= 0) // Amulet을 하나도 소지하고 있지 않은 경우 (아니면 오류로 음수값인 경우)
            {
                if (spec.protected_amulet[0] == value.information.code) // 해당 Amulet이 현재 보호 Amulet 0 이라면
                {
                    spec.protected_amulet[0] = 0;
                    protectd_amulet_information[0] = null;
                }

                if (spec.protected_amulet[1] == value.information.code) // 해당 Amulet이 현재 보호 Amulet 1 이라면
                {
                    spec.protected_amulet[1] = 0;
                    protectd_amulet_information[1] = null;
                }

                specific_amulet_number[value.information.code] = 0;
            }
        }

        value.OnDismiss(); // Amulet 해제 효과 발동
        total_amulet.Remove(value); // 전체 Amulet에서 해당 Amulet 삭제
        Destroy(value.gameObject); // Amulet gameobject 파괴
    }

    public void insert_detail_amulet(BaseAmuletScript value, Amulet_timing timing) // Amulet을 발동 타이밍에 맞추어 List 내 입력
    {
        switch (timing)
        {
            case Amulet_timing.immediately:
                immediately_amulet.Add(value);
                break;
            case Amulet_timing.start_battle:
                start_battle_amulet.Add(value);
                break;
            case Amulet_timing.before_attack:
                before_attack_amulet.Add(value);
                break;
            case Amulet_timing.after_attack:
                after_attack_amulet.Add(value);
                break;
            case Amulet_timing.before_attacked:
                before_attacked_amulet.Add(value);
                break;
            case Amulet_timing.after_attacked:
                after_attacked_amulet.Add(value);
                break;
            case Amulet_timing.turn_end:
                turn_end_amulet.Add(value);
                break;
            case Amulet_timing.end_battle:
                battle_end_amulet.Add(value);
                break;
            case Amulet_timing.life_end:
                life_end_amulet.Add(value);
                break;
        }
    }

    public void dismiss_detail_amulet(BaseAmuletScript value, Amulet_timing timing) // Amulet을 발동 타이밍에 맞추어 List 내 삭제
    {
        switch (timing)
        {
            case Amulet_timing.immediately:
                immediately_amulet.Remove(value);
                break;
            case Amulet_timing.start_battle:
                start_battle_amulet.Remove(value);
                break;
            case Amulet_timing.before_attack:
                before_attack_amulet.Remove(value);
                break;
            case Amulet_timing.after_attack:
                after_attack_amulet.Remove(value);
                break;
            case Amulet_timing.before_attacked:
                before_attacked_amulet.Remove(value);
                break;
            case Amulet_timing.after_attacked:
                after_attacked_amulet.Remove(value);
                break;
            case Amulet_timing.turn_end:
                turn_end_amulet.Remove(value);
                break;
            case Amulet_timing.end_battle:
                battle_end_amulet.Remove(value);
                break;
            case Amulet_timing.life_end:
                life_end_amulet.Remove(value);
                break;
        }
    }

    public void function_amulet(Amulet_timing timing) // 발동 타이밍 내 Amulet의 기능을 전체 발동
    {
        switch (timing)
        {
            case Amulet_timing.immediately:
                for (int i = 0; i < immediately_amulet.Count; i++)
                {
                    immediately_amulet[i].OnFunction(timing);
                }
                break;
            case Amulet_timing.start_battle:
                for (int i = 0; i < start_battle_amulet.Count; i++)
                {
                    start_battle_amulet[i].OnFunction(timing);
                }
                break;
            case Amulet_timing.before_attack:
                for (int i = 0; i < before_attack_amulet.Count; i++)
                {
                    before_attack_amulet[i].OnFunction(timing);
                }
                break;
            case Amulet_timing.after_attack:
                for (int i = 0; i < after_attack_amulet.Count; i++)
                {
                    after_attack_amulet[i].OnFunction(timing);
                }
                break;
            case Amulet_timing.before_attacked:
                for (int i = 0; i < before_attacked_amulet.Count; i++)
                {
                    before_attacked_amulet[i].OnFunction(timing);
                }
                break;
            case Amulet_timing.after_attacked:
                for (int i = 0; i < after_attacked_amulet.Count; i++)
                {
                    after_attacked_amulet[i].OnFunction(timing);
                }
                break;
            case Amulet_timing.turn_end:
                for (int i = 0; i < turn_end_amulet.Count; i++)
                {
                    turn_end_amulet[i].OnFunction(timing);
                }
                break;
            case Amulet_timing.end_battle:
                for (int i = 0; i < battle_end_amulet.Count; i++)
                {
                    battle_end_amulet[i].OnFunction(timing);
                }
                break;
            case Amulet_timing.life_end:
                for (int i = 0; i < life_end_amulet.Count; i++)
                {
                    life_end_amulet[i].OnFunction(timing);
                }
                break;
        }
    }

    public int ret_number_same_amulet(int code_value) // code value값의 Amulet을 현재 Player가 얼마나 가지고 있는지 그 숫자를 반환함
    {
        if (specific_amulet_number.ContainsKey(code_value) == true)
        {
            return specific_amulet_number[code_value];
        }
        return 0;
    }

    public int ret_total_amulet_count() // 플레이어가 가지고 있는 Amulet 전체숫자를 반환함
    {
        return total_amulet.Count;
    }

    public Amulet_information ret_specific_amulet_information(int pos) // 플레이어가 가진 Amulet에서 특정 위치에 있는 Amulet 정보를 반환함
    {
        return total_amulet[pos].information;
    }

    public Amulet_information ret_protected_amulet_information(int pos) // pos위치 protected amulet의 정보를 반환함
    {
        if (pos < protectd_amulet_information.Count)
        {
            return protectd_amulet_information[pos];
        }
        return null;
    }

    public void insert_protected_amulet(int code_value, Amulet_information information_value) // code value 값 amulet의 정보를 protected amulet에 입력함
    {
        for (int i = 0; i < spec.protected_amulet.Count; i++)
        {
            if (spec.protected_amulet[i] == 0)
            {
                spec.protected_amulet[i] = code_value;
                protectd_amulet_information[i] = information_value;
                save_spec();
                break;
            }
        }
    }

    public void delete_protected_amulet(int code_value) // protected amulet에서 code value amulet을 삭제한다
    {
        for (int i = 0; i < spec.protected_amulet.Count; i++)
        {
            if (spec.protected_amulet[i] == code_value)
            {
                spec.protected_amulet[i] = 0;
                protectd_amulet_information[i] = null;
                save_spec();
                break;
            }
        }
    }

    public bool full_protected_amulet() // protected Amulet이 전부 차있는가?
    {
        for (int i = 0; i < spec.protected_amulet.Count; i++)
        {
            if (spec.protected_amulet[i] == 0)
            {
                return false;
            }
        }
        return true;
    }

    public bool same_protected_amulet(int code_value) // argument value로 받은 amulet code가 현재 보호중인 amulet인가?
    {
        for (int i = 0; i < spec.protected_amulet.Count; i++)
        {
            if (spec.protected_amulet[i] == code_value)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    public void Player_death() // 플레이어는 죽는다
    {
        spec.current_turn = 1;
        spec.current_chance += 1;

        function_amulet(Amulet_timing.life_end); // 죽을때 발동하는 Amulet 시전

        if (spec.current_chance > spec.max_chance)
        {
            spec.gameover_status = true; // GameOver
        }

        spec.death_set_hp();
        spec.death_set_atk();
        spec.death_set_pdef();
        spec.death_set_mdef();
        spec.death_set_str();
        spec.death_set_dex();
        spec.death_set_int();
        // spec 전이 설정

        List<BaseAmuletScript> temp = new List<BaseAmuletScript>();
        List<int> carried = new List<int>();

        bool first_protected = true;
        bool second_protected = true;
        if(spec.protected_amulet[0]==0)
        {
            first_protected = false;
        }

        if(spec.protected_amulet[1]==0)
        {
            second_protected = false;
        }


        for (int i =0;i<total_amulet.Count;i++) //amulet 파괴 설정
        {
            float rand = Random.Range(0, 1.0f);
            if(rand <= spec.dismiss_percent)
            {
                if (first_protected == true)
                {
                    if (total_amulet[i].information.code == spec.protected_amulet[0])
                    {
                        first_protected = false;
                        carried.Add(total_amulet[i].information.code);
                        Debug.Log("Protected");
                        continue;
                    }
                }

                if (second_protected == true)
                {
                    if (total_amulet[i].information.code == spec.protected_amulet[1])
                    {
                        second_protected = false;
                        carried.Add(total_amulet[i].information.code);
                        Debug.Log("Protected");
                        continue;
                    }
                }

                temp.Add(total_amulet[i]); // 파괴할 Amulet 목록
                Debug.Log("Destroy");
            }
            else
            {
                carried.Add(total_amulet[i].information.code); // 다음 생에서 가지고 갈 Amulet 목록 (code로 저장 -> spec쪽에 초기화 후 실제 데이터 저장)
                Debug.Log("Saved");
            }
        }

        for(int i =0;i<temp.Count;i++) // 파괴할 Amulet 목록
        {
            dismiss_amulet(temp[i]); // Amulet 해제
        }

        spec.carried_amulet.Clear();
        spec.carried_amulet = carried;

        schedule_information_refresh_and_save();
        save_spec();
    }

    public void next_turn() // 플레이어의 다음 턴
    {
        spec.current_turn += 1;
        schedule_information_refresh_and_save();
        if (spec.current_turn > spec.max_turn)
        {
            Player_death();
        }
    }

    #region schedule function
    public void save_schedule_information()
    {
        SaveManager.saveManager.SaveJsonFile(filename_schedule, schedule_information);
    }

    public void make_schedule_information()
    {
        schedule_information = new ScheduleInformation(Random.Range(0, 10), Random.Range(0, 10));
    }

    public void schedule_information_refresh_and_save()
    {
        schedule_information.soul_gacha = false;
        schedule_information.change_random_shop(Random.Range(0, 10), Random.Range(0, 10));
        save_schedule_information();
    }

    public void schedule_information_init_and_save()
    {
        make_schedule_information();
        save_schedule_information();
    }
    #endregion

    #region UI_Canvas_in_PlayerManager
    public void set_active_stat_canvas(bool value)
    {
        player_stat_canvas.SetActive(value);
    }

    public void set_active_amuletcanvas(bool value) // Amulet canvas를 켤지말지 설정함
    {
        amulet_canvas.SetActive(value);
    }

    public void refresh_stat_canvas()
    {
        if (player_stat_canvas.gameObject.activeInHierarchy == true)
        {
            player_stat_canvas.GetComponent<CharacterStatRoom>().refresh_spec();
        }
    }
    #endregion

    public bool same_ref_chance() // ref chance 확인(플레이어가 던전에서 돌아왔을때, 거기서 죽었는지 아닌지 확인하기 위해)
    {
        if (ref_chance != spec.current_chance)
        {
            ref_chance = spec.current_chance;
            return false;
        }
        return true;
    }

    public bool gold_cal(int value) // 골드 사용
    {
        if(spec.current_gold + value < 0)
        {
            return false;
        }
        spec.current_gold += value;
        save_spec();
        return true;
    }

    public bool soul_cal(int value) // 소울 사용
    {
        if (spec.currect_soul + value < 0)
        {
            return false;
        }
        spec.currect_soul += value;
        save_spec();
        return true;
    }
}
