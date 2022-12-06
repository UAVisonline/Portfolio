using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PositionManager : MonoBehaviour {

    public PlayerController player;//플레이어스크립트 선언
    public float x_pos, y_pos, x_dir, y_dir;//플레이어의 위치와 방향을 저장할 변수
    public static bool p_manager_exist;//처음에는 false로 선언되어 있음
    public string scene_name;//씬네임을 저장할 문자열
	// Use this for initialization
	void Start () {
        if(Application.loadedLevelName == "Main_Screen")//만약 시작했을때 현재 씬이 메인스크린이면 
        {
            if (PlayerPrefs.HasKey("load_x_position"))//로드받을 데이터가 있으면
            {
                x_pos = PlayerPrefs.GetFloat("load_x_position");//데이터를 로드받음
            }
            if (PlayerPrefs.HasKey("load_y_position"))
            {
                y_pos = PlayerPrefs.GetFloat("load_y_position");
            }
            if (PlayerPrefs.HasKey("load_x_direction"))
            {
                x_dir = PlayerPrefs.GetFloat("load_x_direction");
            }
            if (PlayerPrefs.HasKey("load_y_direction"))
            {
                y_dir = PlayerPrefs.GetFloat("load_y_direction");
            }
            if (PlayerPrefs.HasKey("load_scene_name"))
            {
                scene_name = PlayerPrefs.GetString("load_scene_name");
            }
        }
        if(!p_manager_exist)//처음 이 오브젝트가 생성되어 있다면
        {
            p_manager_exist = true;
            DontDestroyOnLoad(this);//로드시에도 이 오브젝트는 파괴하지 않음
        }
        else//처음 이 오브젝트가 생성되어 있는 것이 아니면
        {
            Destroy(gameObject);//파괴
        }
        player = FindObjectOfType<PlayerController>();//플레이어를 불러옴

    }
	
	// Update is called once per frame
	void Update () {
        if(player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
    }

    public void position_call()//플레이어의 위치를 정해서 옮겨줌
    {
        if(scene_name!="")//씬네임이 아무것도 없는것이 아니라면
        {
            SceneManager.LoadScene(scene_name);//씬로드
        }
        player = FindObjectOfType<PlayerController>();//플레이어를 불러옴
        player.transform.position = new Vector3(x_pos, y_pos, 0.0f);//플레이어의 위치를 정함
        //Debug.Log("aaa");
        player.lastmove = new Vector2(x_dir, y_dir);//플레이어의 방향을 정함
    }

    public void reset()//뉴게임을 누르게 된다면 가지고 있는 데이터를 이렇게 바꾸어 버림
    {
        x_pos = 0.0f;
        y_pos = 0.0f;
        x_dir = 0.0f;
        y_dir = -1.0f;
        scene_name = "";
    }

    public void load_position_data()//로드된 위치 데이터를 불러옴(전투시에 쓰이게 될것, 정확히는 플레이어가 죽으면 다시 로드해야되니까 그때 이 함수를 사용하게 됨.)
    {
        if (PlayerPrefs.HasKey("load_x_position"))//로드받을 데이터가 있으면
        {
            x_pos = PlayerPrefs.GetFloat("load_x_position");//데이터를 로드받음
        }
        if (PlayerPrefs.HasKey("load_y_position"))
        {
            y_pos = PlayerPrefs.GetFloat("load_y_position");
        }
        if (PlayerPrefs.HasKey("load_x_direction"))
        {
            x_dir = PlayerPrefs.GetFloat("load_x_direction");
        }
        if (PlayerPrefs.HasKey("load_y_direction"))
        {
            y_dir = PlayerPrefs.GetFloat("load_y_direction");
        }
        if (PlayerPrefs.HasKey("load_scene_name"))
        {
            scene_name = PlayerPrefs.GetString("load_scene_name");
        }
    }
}
