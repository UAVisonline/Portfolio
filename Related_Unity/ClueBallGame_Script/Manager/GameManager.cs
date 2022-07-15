using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Interface_mode { standard, direction }

public enum suspect { kellog, rinda, android, godrick, nameless, nothing } // 5명의 용의자

public enum murder_tool { axe, poision, hammer, knife, crowbar, shovel, nothing} // 6개의 살인도구

public enum crime_scene { livingroom, library, bathroom, garden, kitchen, warehouse, bedroom, dressroom, nothing } // 8개의 장소

public enum information { none, wondering, not_this, this_one }

public class GameManager : MonoBehaviour
{
    private static GameManager _gamemanager;

    public static GameManager gamemanager // Singleton 정의
    {
        get
        {
            if(_gamemanager==null)
            {
                _gamemanager = FindObjectOfType<GameManager>();
                if(_gamemanager==null)
                {
                    Debug.LogError("There is no GameManager Class or Can't load GameManager Class");
                }
            }

            return _gamemanager;
        }
    }

    private int width;
    private int height;
    private bool isFullScreen;
    private Interface_mode interface_mode;

    [SerializeField] private GameObject mainScreenCanvas;
    [SerializeField] private GameObject standardInterfaceCanvas;
    [SerializeField] private GameObject directionInterfaceCanvas; 
    // 참조용 gameObject
    [SerializeField] private Information_list information_list; // 게임을 진행하면서 얻은 정보창 Script

    [SerializeField] private suspect true_suspector;
    [SerializeField] private suspect player_suspector;
    [SerializeField] private murder_tool true_tool;
    [SerializeField] private murder_tool player_tool;
    [SerializeField] private crime_scene true_place;
    [SerializeField] private crime_scene player_place;

    [SerializeField] private backgroundMusic background;
    [SerializeField] private AudioSource click_source;

    [SerializeField] private int original_life;
    private int life;
    private Supporter supporter;

    private bool pause;

    private void Awake()
    {
        width = Screen.width;
        height = Screen.height;
        isFullScreen = Screen.fullScreen; // Screen 옵션 불러오기

        pause = false; // 게임 일시정지 여부

        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("Try")==false)
        {
            PlayerPrefs.SetInt("Try", 0);
        }

        if (PlayerPrefs.HasKey("Standard") == false)
        {
            PlayerPrefs.SetInt("Standard", 0);
        }

        if (PlayerPrefs.HasKey("Directional") == false)
        {
            PlayerPrefs.SetInt("Directional", 0);
        }

        if (PlayerPrefs.HasKey("Correct") == false)
        {
            PlayerPrefs.SetInt("Correct", 0);
        }

        if (PlayerPrefs.HasKey("Correct_standard") == false)
        {
            PlayerPrefs.SetInt("Correct_standard", 0);
        }

        if (PlayerPrefs.HasKey("Correct_directional") == false)
        {
            PlayerPrefs.SetInt("Correct_directional", 0);
        }
        // PlayerPrefs 확인 및 없을경우 정보 설정

        //Debug.Log(Screen.currentResolution);
    }

    public Interface_mode get_mode()
    {
        return interface_mode;
    }

    public void set_resolution_size(int width_value, int height_value) // 해상도 설정
    {
        width = width_value;
        height = height_value;
    }

    public void set_isfull(bool value) // 전체화면 여부 설정
    {
        isFullScreen = value;
    }

    public void apply_resolution() // 실제 해상도 및 전체화면 반영
    {
        Screen.SetResolution(width, height, isFullScreen);
    }

    public suspect get_human_suspect()
    {
        return player_suspector;
    }

    public suspect get_computer_suspect()
    {
        return true_suspector;
    }

    public murder_tool get_human_tool()
    {
        return player_tool;
    }

    public murder_tool get_computer_tool()
    {
        return true_tool;
    }

    public crime_scene get_human_place()
    {
        return player_place;
    }

    public crime_scene get_computer_place()
    {
        return true_place;
    }
    // 컴퓨터 및 플레이어가 유추한 정보 반환
    public void set_interface_mode(Interface_mode value)
    {
        interface_mode = value;
        mainScreenCanvas.SetActive(false); // Main화면 Off

        switch(interface_mode) // Interface mode에 따른 InterfaceCanvas 상태 설정
        {
            case Interface_mode.standard:
                standardInterfaceCanvas.SetActive(true);
                directionInterfaceCanvas.SetActive(false);
                break;
            case Interface_mode.direction:
                standardInterfaceCanvas.SetActive(false);
                directionInterfaceCanvas.SetActive(true);
                break;
        }
    }

    public void set_suspect(suspect value)
    {
        player_suspector = value;
    }

    public void set_tool(murder_tool value)
    {
        player_tool = value;
    }

    public void set_place(crime_scene value)
    {
        player_place = value;
    }
    // 플레이어의 유추 정보 설정

    public int get_life()
    {
        return life;
    }

    public void set_life(int value)
    {
        life = value;
    }

    public bool get_pause()
    {
        return pause;
    }

    public void set_pause(bool value)
    {
        pause = value;
    }

    public Supporter get_supporter()
    {
        if(supporter==null)
        {
            Supporter temp = FindObjectOfType<Supporter>();
            supporter = temp;
        }
        return supporter;
    }
    // Supporter 정보 반환

    public void making_answer()
    {
        bool overlap = false;

        while(true)
        {
            int suspect_number = Random.Range(0, 1000);
            suspect_number = suspect_number % 5;

            suspect tmp = true_suspector;
            if(tmp == (suspect)suspect_number && overlap==false)
            {
                overlap = true;
                continue;
            }

            true_suspector = (suspect)suspect_number;
            break;
        } // 실제 용의자 설정

        overlap = false;
        while (true)
        {
            int tool_number = Random.Range(0, 1200);
            tool_number = tool_number % 6;

            murder_tool tmp = true_tool;
            if (tmp == (murder_tool)tool_number && overlap == false)
            {
                overlap = true;
                continue;
            }

            true_tool = (murder_tool)tool_number;
            break;
        } // 실제 범행도구 설정

        overlap = false;
        while(true)
        {
            int place_number = Random.Range(0, 1600);
            place_number = place_number % 8;

            crime_scene tmp = true_place;
            if (tmp == (crime_scene)place_number && overlap == false)
            {
                overlap = true;
                continue;
            }

            true_place = (crime_scene)place_number;
            break;
        } // 실제 범행현장 설정
        // overlap 변수를 통해 이전 선택지와 동일한 선택지가 나오는 것을 1번 방지함

        life = original_life;
    }

    public void set_visualize(bool value)
    {
        information_list.set_visualize(value);
    }

    public void information_animation_start(string value)
    {
        information_list.start_animation(value);
    }

    public void init_informations()
    {
        information_list.init_information();
    }

    public void click_play() // 클릭음 재생
    {
        click_source.Play();
    }

    public void background_music_play() // 음악 재생
    {
        background.set_turn(true);
        background.music_play();
    }

    public void background_music_slow_stop() // 음악 서서히 정지
    {
        background.set_turn(false);
    }

    public void background_music_instant_stop() // 음악 즉시 정지
    {
        background.set_turn(false);
        background.music_stop();
    }

    public int Reasoning_result() // 플레이어 추론 반영
    {
        int answer = 0;

        if (true_suspector == player_suspector) answer += 1;
        if (true_tool == player_tool) answer += 1;
        if (true_place == player_place) answer += 1;

        if(answer==0)
        {
            information_list.zero_match(player_suspector, player_tool, player_place);
        }
        else
        {
            information_list.more_match(player_suspector, player_tool, player_place, answer);
        }
        // 정답 갯수에 따른 정보 Note 설정 변경

        return answer;
    }
}
