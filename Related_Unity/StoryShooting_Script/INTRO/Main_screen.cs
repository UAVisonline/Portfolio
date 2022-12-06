using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Main_screen : MonoBehaviour {

    public Text NewGame, LoadGame, Exit;//newgame,loadgame,exit라는 text값을 저장할 글자
    public PositionManager p_manager;//포지션매니저를 불러옴.
    public Text_manager t_manager;//텍스트매니저를 불러옴.
    public int main_select;//newgame,loadgame을 선택하고 결정하는 변수
    public bool fade,okay;//newgame,loadgame text의 모습을 공개하는 참거짓변수
    public float time_delay;//텍스트를 띄우기위해 엔터연타시 자신이 원하지 않는 부분에 커서가 있을수 있으므로 엔터를 연타해도 이 시간이 남아있는한은 커맨드입력을 받지 않도록 결정짓는 변수
    private AudioSource fx_source;
    public AudioClip selcet_sound;
    public Bgm_manager bg_manager;
	// Use this for initialization
	void Start () {
        fade = false;//아직 텍스트가 나오지않았음을 의미함
        p_manager = FindObjectOfType<PositionManager>();//포지션매니저를 월드에서 불러옴
        t_manager = FindObjectOfType<Text_manager>();//텍스트매니저를 월드에서 불러옴
        fx_source = GetComponent<AudioSource>();
        t_manager.fade_black_out_off();
        main_select = 0;//게임이 저장되있지 않다면 커서가 newgame에 위치
        bg_manager = FindObjectOfType<Bgm_manager>();
        if(PlayerPrefs.GetInt("game_save")==1)//게임이 저장되어 있다면
        {
            main_select = 1;// 커서가 loadgame에 위치
        }
	}
	
	// Update is called once per frame
	void Update () {
        NewGame.color = Color.white;
        LoadGame.color = Color.white;
        Exit.color = Color.white;
        if(bg_manager==null)
        {
            bg_manager = FindObjectOfType<Bgm_manager>();
        }
        if(t_manager==null)
        {
            t_manager = FindObjectOfType<Text_manager>();//텍스트매니저를 월드에서 불러옴
            t_manager.fade_black_out_off();
        }
	    if(fade == false)//아직 텍스트가 표시되어 있지 않을때
        {
            NewGame.text = "";
            LoadGame.text = "";
            Exit.text = "";
            if(Input.GetKeyDown(KeyCode.Return))//엔터키를 누르면
            {
                fade = true;//텍스트를 표시한다
            }
        }
        if(fade)
        {
            text_option(main_select);//텍스트의 모습과 커서를 설정하는 함수 
            time_delay -= Time.deltaTime;//연타방지 시간을 줄임
            if(Input.GetKeyDown(KeyCode.DownArrow))//아래 방향표를 누르면
            {
                if(PlayerPrefs.HasKey("game_save"))//게임이 저장되어 있다면
                {
                    if(main_select != 2)//셀렉트변수가 exit(2)를 가리키지 않는다면
                    {
                        main_select++;//셀렉트변수를 올림
                    }
                }
                else//게임이 저장되어 있지 않다면
                {
                        main_select = 2;//셀렉트변수는 2를 계속해서 가리키도록 함
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))//윗방향표를 누르면
            {
                if (PlayerPrefs.HasKey("game_save"))//게임이 저장되어 있다면
                {
                    if (main_select != 0)//셀렉트변수가 newgame(0)을 가리키지 않는다면
                    {
                        main_select--;//셀렉트변수를 내림
                    }
                }
                else//게임이 저장되어 있지 않다면
                {
                    main_select = 0;//셀렉트변수는 계속해서 0을 가리키도록 함
                }
            }
            if(Input.GetKeyDown(KeyCode.Return) && time_delay <= 0.00f)//연타방지 시간이 지난 상태에서 엔터키를 누루면
            {
                if(!okay)
                {
                    //fx_source.clip = selcet_sound;
                    //fx_source.loop = false;
                    Next(main_select);//셀렉트변수에 따라 다음 명령을 전함.
                    okay = true;
                }
            }
        }
	}

    void text_option(int main_select)//텍스트의 모양을 결정
    {
        if(PlayerPrefs.GetInt("game_save") == 1)//게임이 저장되어 있다면
        {
            if (main_select == 0)//셀렉트변수가 0일 경우
            {
                NewGame.text = ">>> New  Game";
                LoadGame.text = "Load Game";
                Exit.text = "Exit";
            }
            if (main_select == 1)//셀렉트변수가 1일 경우
            {
                NewGame.text = "New  Game";
                LoadGame.text = ">>> Load Game";
                Exit.text = "Exit";
            }
            if (main_select == 2)//셀렉트변수가 2일 경우
            {
                NewGame.text = "New  Game";
                LoadGame.text = "Load Game";
                Exit.text = ">>> Exit";
            }
        }
        else//게임이 저장되어 있지 않다면
        {
            if (main_select == 0)//셀렉트변수가 0일 경우
            {
                NewGame.text = ">>> New  Game";
                LoadGame.text = "";
                Exit.text = "Exit";
            }
            if (main_select == 2)//셀렉트변수가 2일 경우
            {
                NewGame.text = "New  Game";
                LoadGame.text = "";
                Exit.text = ">>> Exit";
            }
        }
    }

    void Next(int main_select)//다음 씬을 정하는 명령
    {
        p_manager = FindObjectOfType<PositionManager>();//포지션매니저를 월드에서 불러옴
        t_manager = FindObjectOfType<Text_manager>();//텍스트매니저를 월드에서 불러옴
        if (main_select == 0)//셀렉트변수가 newgame(0)을 가리킬경우
        {
            fx_source.PlayOneShot(selcet_sound);
            bg_manager.music_stop();
            PlayerPrefs.DeleteAll();//저장데이터 전부 삭제
            PlayerPrefs.SetInt("current_death", 0);
            PlayerPrefs.SetInt("total_death", 0);
            PlayerPrefs.SetInt("kill", 0);
            PlayerPrefs.SetInt("Death_Point", 0);
            p_manager.reset();//포지션매니저가 플레이어의 위치를 결정시키는 변수를 모두 초기화시킴
            t_manager.fade_black_out_on();//텍스트매니저를 이용해서 화면암전
            StartCoroutine(newgame_start(3f));//해당 코루틴을 실행시킴
            //if(p_manager)
        }
        else if(main_select == 1) //셀렉트변수가 loadgame(1)을 가리킬경우
        {
            fx_source.PlayOneShot(selcet_sound);
            bg_manager.music_stop();
            t_manager.fade_black_out_on();//텍스트매니저를 이용해서 화면암점
            StartCoroutine(loadgame_start(1f));//해당 코루틴을 실행시킴
        }
        else if(main_select == 2)//셀렉트변수가 exit(2)을 가리킬경우
        {
            Application.Quit();//게임종료
        }
    }

    IEnumerator newgame_start(float time)//newgame 코루틴
    {
        yield return new WaitForSeconds(time);//time만큼의 시간을 기다림
        SceneManager.LoadScene("cave_start_point"); //처음시작위치로 넘어감
    }

    IEnumerator loadgame_start(float time)//loadgame 코루틴
    {
        yield return new WaitForSeconds(time);//time만큼의 시간을 기다림
        SceneManager.LoadScene(p_manager.scene_name);//포지션매니저가 저장한 씬이름으로 넘어감
    }

}
