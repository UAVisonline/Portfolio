using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Google.Protobuf.Protocol;

public enum frame_setting { test_low, low, medium, high, very_high } // 30, 60, 120, 144
    

public class GameManager : MonoBehaviour // GameManager Script, 
{
    private static GameManager _gamemanager = null;

    public static GameManager gamemanager // 
    {
        get
        {
            if (_gamemanager == null)
            {
                _gamemanager = FindObjectOfType<GameManager>();
                if (_gamemanager == null)
                {
                    Debug.LogError("There is no GameManager Class");
                }
            }
            return _gamemanager;
        }
    }

    [TabGroup("Related about GamePlaying")] [ReadOnly] [SerializeField] private AudioSource audiosource;

    [TabGroup("Related about Main Game")] [SerializeField] private MusinPatternObject music_patterns; // 
    [TabGroup("Related about Main Game")] [SerializeField] private DirectorObject director; // 
    [TabGroup("Related about Main Game")] [SerializeField] private GameObject StageObject; // 
    [TabGroup("Related about Main Game")] [SerializeField] private float playback_location; // 
    [TabGroup("Related about Main Game")] [SerializeField] private float music_length; // 
    [TabGroup("Related about Main Game")] [SerializeField] private bool multi_play; // 

    [TabGroup("Related about Main Game")] [SerializeField] private GeneratorArea rework_generator; // Note 

    [TabGroup("Related about Main Game")] [ReadOnly] [SerializeField] private bool game_start; // pattern
    [TabGroup("Related about Main Game")] [ReadOnly] [SerializeField] private bool game_end; //flag
    [TabGroup("Related about Main Game")] [ReadOnly] [SerializeField] private bool game_director; // director
    // ?�크 기능 추�?

    [TabGroup("Related about Score")] [ReadOnly] [SerializeField] private int highest_combo;
    [TabGroup("Related about Score")] [ReadOnly] [SerializeField] private int total_combo;
    [TabGroup("Related about Score")] [ReadOnly] [SerializeField] private int perfect;
    [TabGroup("Related about Score")] [ReadOnly] [SerializeField] private int good;
    [TabGroup("Related about Score")] [ReadOnly] [SerializeField] private int miss;
    [TabGroup("Related about Score")] [ReadOnly] [SerializeField] private float accuracy;

    [SerializeField] private string song_title;
    [SerializeField] private string song_level;
    [SerializeField] private Sprite jacket;
    [SerializeField] private bool multiplay;
    private level_difficulty song_level_type;
    private director_mode manager_director_mode;
    private frame_setting frame_option;
    private bool is_music_play_ready;

    [ReadOnly] [SerializeField] private int normal_correct;
    [ReadOnly] [SerializeField] private int normal_good_correct;
    [ReadOnly] [SerializeField] private int normal_uncorrect;
    [ReadOnly] [SerializeField] private int slide_correct;
    [ReadOnly] [SerializeField] private int slide_good_correct;
    [ReadOnly] [SerializeField] private int slide_uncorrect;
    [ReadOnly] [SerializeField] private int move_correct;
    [ReadOnly] [SerializeField] private int move_uncorrect;

    private WaitForSeconds wait_time = new WaitForSeconds(1.0f);

    private void Awake()
    {
        if(_gamemanager==null)
        {
            _gamemanager = FindObjectOfType<GameManager>();
            audiosource = this.GetComponent<AudioSource>();
            if(PlayerPrefs.HasKey("FrameSet")==false)
            {
                frame_set(frame_setting.medium);
            }
            else
            {
                frame_set((frame_setting)PlayerPrefs.GetInt("FrameSet"));
            }
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public bool get_multi_play()
    {
        return multi_play;
    }

    private void Update()
    {
        if(game_start==true && game_director==true) // ...
        {
            if (this.GetComponent<AudioSource>().isPlaying == false && is_music_play_ready==true)  // 
            {
                this.GetComponent<AudioSource>().Play();
            }

            if(audiosource.time > music_length - 0.2f) // 
            {
                set_game_start(false);
                set_game_direct(false);
                set_game_end(true);
                main_game_end();

                return;  //
            }
        }
        else
        {
            return;
        }

        if(rework_generator!=null)
        {
            rework_generator.Note_send(audiosource.time);
        }

        if(director != null)
        {
            director.Direct_update(audiosource.time);
        }

        playback_location = audiosource.time;
    }

    #region get_function
    public MusinPatternObject get_pattern()
    {
        return music_patterns;
    }

    public int get_total_combo()
    {
        return total_combo;
    }

    public int get_highest_combo()
    {
        return highest_combo;
    }

    public int get_perfect()
    {
        return perfect;
    }

    public int get_good()
    {
        return good;
    }

    public int get_miss()
    {
        return miss;
    }

    public float get_accuracy()
    {
        return accuracy;
    }

    public string get_song_title()
    {
        return song_title;
    }

    public string get_level()
    {
        return song_level;
    }

    public level_difficulty get_level_difficulty()
    {
        return song_level_type;
    }

    public director_mode get_director_mode()
    {
        return manager_director_mode;
    }

    public bool get_multiplay_status()
    {
        return multiplay;
    }
    public bool get_game_end()
    {
        return game_end;
    }
    #endregion

    #region set_function
    public void set_music(AudioClip music)
    {
        this.GetComponent<AudioSource>().clip = music;
    }

    public void set_clicked(bool status)
    {
        return;
    }
    public void set_multi_game(bool value)
    {
        multi_play = value;
    }
    public void set_game_end(bool value)
    {
        game_end = value;
    }

    public void set_game_start(bool value)
    {
        game_start = value;
    }

    public void set_game_direct(bool value)
    {
        game_director = value;
    }

    public void set_rework_generator(GeneratorArea generator)
    {
        rework_generator = generator;
    }

    public void set_Stage(GameObject value)
    {
        StageObject = value;
    }

    public void set_pattern(MusinPatternObject pattern)
    {
        music_patterns = pattern;
    }


    public void set_music_title(string value)
    {
        song_title = value;
    }

    public void set_music_level(string value)
    {
        song_level = value;
    }

    public void set_jacket(Sprite value)
    {
        jacket = value;
    }

    public void set_level_difficulty(level_difficulty value)
    {
        song_level_type = value;
    }

    public void set_director_mode(director_mode value)
    {
        manager_director_mode = value;
    }
    public void set_multiplay_status(bool value)
    {
        multiplay = value;
    }
    #endregion

    public void main_game_start() // main game
    {
        music_length = this.GetComponent<AudioSource>().clip.length;
        is_music_play_ready = true;

        this.total_combo = 0;
        this.highest_combo = 0;
        this.perfect = 0;
        this.good = 0;
        this.miss = 0;
        this.accuracy = 0.0f;

        set_game_start(true); // pattern obejct�� Okay
        set_game_end(false);
    }

    public void making_Stage()
    {
        if(StageObject!=null)
        {
            GameObject tmp = Instantiate(StageObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity); // 
            if (tmp.GetComponent<DirectorObject>() != null)
            {
                set_game_direct(true); // 
                director = tmp.GetComponent<DirectorObject>();
            }
        }
    }

    public void main_game_end()
    {
        is_music_play_ready = false;
        this.GetComponent<AudioSource>().Stop();

         //
        this.total_combo = ScoreObject.ScoreObj.get_total_combo();
        this.highest_combo = ScoreObject.ScoreObj.get_highest_combo();
        this.perfect = ScoreObject.ScoreObj.get_perfect_number();
        this.good = ScoreObject.ScoreObj.get_good_number();
        this.miss = ScoreObject.ScoreObj.get_miss_number();
        this.accuracy = ScoreObject.ScoreObj.get_accruacy();

        this.normal_correct = ScoreObject.ScoreObj.get_normal_correct();
        this.normal_good_correct = ScoreObject.ScoreObj.get_normal_good();
        this.normal_uncorrect = ScoreObject.ScoreObj.get_normal_uncorrect();
        this.slide_correct = ScoreObject.ScoreObj.get_slide_correct();
        this.slide_good_correct = ScoreObject.ScoreObj.get_slide_good();
        this.slide_uncorrect = ScoreObject.ScoreObj.get_slide_uncorrect();
        this.move_correct = ScoreObject.ScoreObj.get_move_correct();
        this.move_uncorrect = ScoreObject.ScoreObj.get_move_uncorrect();

        if (multi_play == true)
        {
            Debug.Log("Game manage: multi is true and send Result to Opponent");
            C_Result c_Result = new C_Result
            {
                TotalCombo = total_combo,
                HighestCombo = highest_combo,
                Perfect = perfect,
                Good = good,
                Miss = miss,
                Acr = accuracy
            };
            NetworkManager.Instance.Send(c_Result);
        }

        SceneManager.LoadScene("ResultScene");
    }

    private void main_game_stop_function1() // 
    {
        set_game_start(false);
        set_game_direct(false);
        is_music_play_ready = false;

        if (this.GetComponent<AudioSource>().isPlaying == true)
        {
            this.GetComponent<AudioSource>().Stop();
        }

    }

    private void main_game_stop_function2() //
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void Game_stop() // 
    {
        StartCoroutine("Stop_coroutine");
    }

    [Button]
    public void frame_set(frame_setting value)
    {
        frame_option = value;
        switch (frame_option)
        {
            case frame_setting.test_low:
                Application.targetFrameRate = 30;
                break;
            case frame_setting.low:
                Application.targetFrameRate = 30;
                break;
            case frame_setting.medium:
                Application.targetFrameRate = 60;
                break;
            case frame_setting.high:
                Application.targetFrameRate = 120;
                break;
            case frame_setting.very_high:
                Application.targetFrameRate = 144;
                break;
        }
        PlayerPrefs.SetInt("FrameSet", (int)frame_option);
    }

    IEnumerator Stop_coroutine()
    {
        main_game_stop_function1();
        yield return wait_time;

        main_game_stop_function2();
    }
}
