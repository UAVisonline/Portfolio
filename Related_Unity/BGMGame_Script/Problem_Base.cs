using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Problem_Base : MonoBehaviour
{
    [SerializeField] private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>(); // 음악문제를 저장할 Dictionary 생성
    [SerializeField] private List<string> problem_list; 
    //[SerializeField] private List<string> _answer_list;

    [SerializeField] private string[] answer_sheet = new string[4];
    [SerializeField] private int[] problem_size = new int[7]; // not use it (난이도 별 문제가 얼마나 있는가 저장)

    [SerializeField] private List<string> hints;
    [SerializeField] private string game_title;
    [SerializeField] private string bgm_title;

    AudioSource bgm;
    Text information_text;
    Text correct_wrong_text;
    [SerializeField] Problem_file problem_file;
    private static Problem_Base _problem;

    private bool training_mode;
    private bool arcade_mode;

    private bool correct;
    private int list_pos;
    private bool solved;
    private int answer_number;

    public int Load(string folder) // Resource Folder로 들어가서 음악 문제 불러옴
    {
        object[] obj = Resources.LoadAll(folder);
        for(int i = 0;i<obj.Length;i++)
        {
            GameObject _gameobject = (GameObject)obj[i];
            _cache[_gameobject.name] = _gameobject;
            Problem_file temp = _gameobject.GetComponent<Problem_file>(); // 왜 만든거임???
        }
        return obj.Length;
    }

    public GameObject Get(string key) 
    {
        return _cache[key];
    }

    public static Problem_Base problem // Singleton 설정
    {
        get
        {
            if (_problem == null)
            {
                _problem = FindObjectOfType<Problem_Base>();
                if (_problem == null)
                {
                    Debug.LogError("Can't Load Problem");
                }
            }
            return _problem;
        }
    }

    private void Awake() // Singleton 할당 및 이미 할당 된 경우 Object 삭제
    {
        if (_problem == null)
        {
            DontDestroyOnLoad(this.gameObject);
            _problem = FindObjectOfType<Problem_Base>();
            if (_problem == null)
            {
                Debug.LogError("Can't Load Problem");
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int loop = 0;loop<7;loop++)
        {
            int list_size = 1;
            if (loop == 0) list_size = Load("0_Very_Easy");
            else if (loop == 1) list_size = Load("1_Easy");
            else if (loop == 2) list_size = Load("2_Normal");
            else if (loop == 3) list_size = Load("3_Hard");
            else if (loop == 4) list_size = Load("4_Very_Hard");
            else if (loop == 5) list_size = Load("5_Expert");
            else if (loop == 6) list_size = Load("6_Nightmare");
            //int list_size = Load("2_Normal");
            for (int i = 0; i < list_size; i++)
            {
                if (loop == 0)
                {
                    problem_list.Add("VE" + i);
                }
                else if (loop == 1)
                {
                    problem_list.Add("EA" + i);
                }
                else if (loop == 2)
                {
                    problem_list.Add("NO" + i);
                }
                else if (loop == 3)
                {
                    problem_list.Add("HA" + i);
                }
                else if (loop == 4)
                {
                    problem_list.Add("VH" + i);
                }
                else if (loop == 5)
                {
                    problem_list.Add("EX" + i);
                }
                else if (loop == 6)
                {
                    problem_list.Add("NI" + i);
                }
            }
            problem_size[loop] = list_size;//난이도별 문제수
        } // 각 폴더별로 음악 게임 오브젝트를 불러옴 (이는 순서대로 설정됨)


        for (int i = 0; i < problem_list.Count; i++)
        {
            int replace = Random.Range(0, problem_list.Count - 1);
            if (i != replace) // 
            {
                string temp = problem_list[i];
                problem_list[i] = problem_list[replace];
                problem_list[replace] = temp;
            }
        }// 순서대로 설정된 음악 게임 오브젝트를 shuffle함

        
        bgm = this.GetComponent<AudioSource>();

        training_mode = false;
        solved = false;
        correct = false;
        list_pos = 0;
    }

    public bool ret_arcade_mode()
    {
        return arcade_mode;
    }

    public bool ret_training_mode()
    {
        return training_mode;
    }

    public bool ret_solved()
    {
        return solved;
    }

    public void jacket_black() // 음악 Jacket 어둡게
    {   
        Image bgm_sprite = GameObject.Find("Jacket").GetComponent<Image>();
        if (bgm_sprite!=null) bgm_sprite.color = Color.black;
        //jacket.color = Color.black;
    }

    public void answer_function(int i) // 정답 선택 시 채점 함수
    {
        if(!solved)
        {
            jacket_white();
            if (i == answer_number) // 정답을 맞혔다면
            {
                if (correct_wrong_text == null) correct_wrong_text = GameObject.Find("Correct_Wrong").GetComponent<Text>();
                correct_wrong_text.color = Color.blue;
                correct_wrong_text.text = "Correct!!!";
                ScoreManager.score_manager.score_up();
                set_correct(true);
            }
            else // 정답을 틀렸다면
            {
                if (correct_wrong_text == null) correct_wrong_text = GameObject.Find("Correct_Wrong").GetComponent<Text>();
                correct_wrong_text.color = Color.red;
                correct_wrong_text.text = "Wrong!!!";
                set_correct(false);
            }
            solved = true;

            if (information_text == null) information_text = GameObject.Find("Information").GetComponent<Text>();

            if (bgm_title != "")
            {
                information_text.text = game_title + " - " + bgm_title;
            }
            else
            {
                information_text.text = game_title;
            }
        }
    }

    public void pass_function()
    {
        if(!solved) // 문제가 안 풀렸으면
        {
            jacket_white(); // 재킷을 하얗게
            if (correct_wrong_text == null)
            {
                correct_wrong_text = GameObject.Find("Correct_Wrong").GetComponent<Text>();

            }
            correct_wrong_text.color = Color.black;
            correct_wrong_text.text = "Pass"; // 문제를 넘겼습니다
            solved = true;

            if (information_text == null) information_text = GameObject.Find("Information").GetComponent<Text>(); 

            if (bgm_title != "")
            {
                information_text.text = game_title + " - " + bgm_title;
            }
            else
            {
                information_text.text = game_title;
            } // 노래에 대한 정보 시각화 (게임 제목 및 bgm 제목)
        }
    }

    public void set_hint(string[] str) // 문제에 대한 힌트 설정
    {
        hints.Clear();
        for (int i = 0; i < str.Length; i++) hints.Add(str[i]);
    }

    public void set_information(string title,string bgm_name) // 노래 정보 변수 설정
    {
        game_title = title;
        bgm_title = bgm_name;
    }

    public void set_audiosource(AudioClip music) // 음악 변수 설정
    {
        bgm.clip = music;
    }

    public void set_jacket(Sprite image) // 음악 앨범 표지 설정
    {
        Image bgm_sprite = GameObject.Find("Jacket").GetComponent<Image>();
        if (bgm_sprite != null) bgm_sprite.sprite = image;
    }

    public void jacket_white()
    {
        Image bgm_sprite = GameObject.Find("Jacket").GetComponent<Image>();
        if (bgm_sprite != null) bgm_sprite.color = Color.white;
    }

    public void set_correct(bool status)
    {
        correct = status;
    }

    public void set_solved(bool status)
    {
        solved = status;
    }

    public bool return_correct()
    {
        return correct;
    }

    public void information_clear() // 시각화된 정보를 초기화
    {
        if(information_text==null) information_text = GameObject.Find("Information").GetComponent<Text>();
        information_text.text = "";
    }

    public void correct_wrong_clear() // 노래 정답 유무 시각화를 초기화
    {
        if (correct_wrong_text == null) correct_wrong_text = GameObject.Find("Correct_Wrong").GetComponent<Text>();
        correct_wrong_text.text = "";
    }

    public void Music_Play()
    {
        bgm.Play();
    }

    public void Music_Stop()
    {
        bgm.Stop();
    }

    public void Music_back_ten_second()
    {
        float time = bgm.time;
        if (time <= 10.00f)
        {
            bgm.Play();
        }
        else
        {
            StartCoroutine("Play_Back");
        }
        
        //bgm.Pl
    }

    public void Change_Problem()
    {
        Problem_delete();
        problem_file = Instantiate(Get(problem_list[++list_pos]), Vector2.zero, Quaternion.identity).GetComponent<Problem_file>(); // 음악 오브젝트 생성 후 참조
        problem_file.Problem_set(); // 정보 설정

        if (problem_list.Count - 1 == list_pos) // 모든 문제를 탐색한 경우
        {
            for(int i = 0;i<problem_list.Count;i++) 
            {
                int replace = Random.Range(0, problem_list.Count - 1);
                if(i!=replace)
                {
                    string temp = problem_list[i];
                    problem_list[i] = problem_list[replace];
                    problem_list[replace] = temp;
                }
            }
            list_pos = -1; // Shuffle후 배열 처음으로 이동
        }
    }

    public void Problem_delete() // 음악 오브젝트 삭제
    {
        if (problem_file != null)
        {
            Destroy(problem_file.gameObject);
        }
    }

    public void give_a_hint(int num) // 정보 시각화
    {
        if (information_text == null) information_text = GameObject.Find("Information").GetComponent<Text>();
        if(!ret_solved())
        {
            information_text.text = hints[num];
        }
    }

    public void set_answer_state(Problem_file problem) // 음악 선택지 시각화
    {
        answer_number = (int)Random.Range(0, 4);
        //Debug.Log("answer : " + answer_number);

        int[] bogi_list = new int[3];
        for(int i =0;i<3;i++)
        {
            bogi_list[i] = (int)Random.Range(0, problem.ret_bogi_size());
            for(int j =0;j<i;j++)
            {
                if(bogi_list[j]==bogi_list[i])
                {
                    i--;
                    break;
                }
            }
        }

        int tmp = 0;
        for(int i =0;i<4;i++)
        {
            if(i==answer_number)
            {
                answer_sheet[i] = problem.ret_answer();
            }
            else
            {
                answer_sheet[i] = problem.ret_bogi(bogi_list[tmp]);
                tmp++;
            }
        }

        for(int i =0;i<4;i++)
        {
            Text temp = GameObject.Find("Answer_" + i.ToString()).GetComponent<Text>();
            {
                temp.text = answer_sheet[i];
            }
        }
    }

    public void set_traing_mode(bool status)
    {
        training_mode = status;
    }

    public void set_arcade_mode(bool status)
    {
        arcade_mode = status;
    }

    public void Start_Game() // 게임 시작 버튼
    {
        if(training_mode)
        {
            StartCoroutine("Start_Practice");
        }
        
        if(arcade_mode)
        {
            StartCoroutine("Start_Arcade");
        }
    }

    public void Back_Game() // Main화면으로
    {
        StartCoroutine("Go_Main");
    }

    IEnumerator Start_Practice() // 연습모드 시작
    {
        while (true) // Scene이 완전 이동할 때 까지 대기
        {
            if (SceneManager.GetActiveScene().name == "Game") break;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        GameObject arcade_obj = GameObject.Find("Only_Arcade"); // 아케이드용 설정 오브젝트 탐색 (UI)
        arcade_obj.SetActive(false); // 그 후 해당 오브젝트 종료

        problem_file = Instantiate(Get(problem_list[list_pos]), Vector2.zero, Quaternion.identity).GetComponent<Problem_file>(); // 문제 오브젝트 생성
        problem_file.Problem_set(); 

        jacket_black();
        correct = false;
        solved = false;
        Music_Play();
        yield return new WaitForSeconds(1.0f);
        Directer_machine.directer.set_panel_slide(false);
    }

    IEnumerator Start_Arcade()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "Game") break;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        GameObject test_obj = GameObject.Find("Only_Test"); // 테스트용 설정 오브젝트 탐색 (UI)
        test_obj.SetActive(false); // 그 후 해당 오브젝트 종료

        ScoreManager.score_manager.arcade_text();
        problem_file = Instantiate(Get(problem_list[list_pos]), Vector2.zero, Quaternion.identity).GetComponent<Problem_file>();
        problem_file.Problem_set();

        jacket_black();
        correct = false;
        solved = false;
        Music_Play();
        yield return new WaitForSeconds(1.0f);
        Directer_machine.directer.set_panel_slide(false);
    }

    IEnumerator Go_Main()
    {
        Music_Stop();
        training_mode = false;
        arcade_mode = false;
        solved = false;
        correct = false;
        list_pos = 0;
        // 변수를 전부 초기값으로 설정

        for (int i = 0; i < problem_list.Count; i++)
        {
            int replace = Random.Range(0, problem_list.Count - 1);
            if (i != replace)
            {
                string temp = problem_list[i];
                problem_list[i] = problem_list[replace];
                problem_list[replace] = temp;
            }
        } //문제목록은 다시 Shuffle

        while (true)
        {
            if (SceneManager.GetActiveScene().name == "Main") break;
            yield return new WaitForSeconds(Time.deltaTime);
        } // Scene 이동

        Directer_machine.directer.set_panel_slide(false);
    }

    IEnumerator Play_Back() // 음악 뒤로 10초정도 이동
    {
        bgm.volume = 0.1f;
        bgm.pitch = -8.0f;
        yield return new WaitForSeconds(1.2f);
        bgm.pitch = 1.0f;
        bgm.volume = 1.0f;
    }
}
