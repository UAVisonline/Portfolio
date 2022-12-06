using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
//플레이어의 탐험모드관련 스크립트(이동과 상호작용을 처리함)
public class PlayerController : MonoBehaviour {

    public float Movespeed;//플레이어의 이동속도
    public bool player_cannot_move;//플레이어가 현재 움직일수 있나 없나를 결정함
    private bool Moving;//플레이어가 현재 이동하고 있는지 이를 확인하는 변수
    private Animator anim;//애니메이터 선언
    private Rigidbody2D rb;//리지드바디2d 선언
    private static bool player_exist;//플레이어존재 static변수 선언, 처음값은 설정을 안하면 자동으로 false, 전투를 실행하러 갈때는 이 값을 다시 false로 바꾸어야함.
    public Vector2 lastmove;//플레이어의 마지막움직임을 기록하는 벡터
    public PositionManager p_manager;//포지션매니저 선언
    public move_world move_manager;//move_world 스크립트에서 정보를 받아오기 위해 선언
    public Text_manager t_manager;//텍스트매니저 선언
    public Transform tf;//트랜스폼 선언
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();//자신의 애니메이터를 불러옴
        rb = GetComponent<Rigidbody2D>();//자신의 리지드바디2D를 불러옴
        tf = GetComponent<Transform>();//자신의 트랜스폼을 불러옴
        p_manager = FindObjectOfType<PositionManager>();//필드위 포지션매니저를 불러옴
        t_manager = FindObjectOfType<Text_manager>();//필드위 텍스트매니저를 불러옴
       if (!player_exist)//플레이어가 처음 생성될경우
        {
            player_exist = true;
            DontDestroyOnLoad(this);//이 오브젝트를 씬을 로드할때 파괴시키지 않음.
        }
        else//플레이어가 이미 생성되어 있다면
        {
            Destroy(gameObject);//새로 생성된 오브젝트파괴
        }
        if(PlayerPrefs.GetInt("game_save")==1)//게임이 저장되어 있다면 (이 의미보다 크게 보아야하는것이 게임은 이미 저장되어 있으며 밑에 있는 코루틴을 실행시키는 의미로 보는것이 편함. 이는 배틀후에도 다시 씬을 불러와 이 오브젝트를 생성시킬때도 코루틴을 실행시키는 의미임.)
        {
            StartCoroutine(position_call(0.3f));
        }
       
         //p_manager.position_call();//시작할때 플레이어 위치를 포지션매니저에서 받아서 불러옴.
	}
	
	// Update is called once per frame
	void Update () {
        Moving = false;//플레이어는 움직이지 않음
        if(p_manager == null)//포지션매니저가 제대로 선언이 안 되어있으면
        {
            p_manager = FindObjectOfType<PositionManager>();//포지션매니저를 다시 받아옴
        }
        if(t_manager == null)//텍스트매니저가 제대로 선언이 안 되어있으면
        {
            t_manager = FindObjectOfType<Text_manager>();//텍스트매니저를 다시 받아옴
        }
        if(!t_manager.Player_moving  || !t_manager.fade_moving || player_cannot_move)//해당 참거짓변수로 플레이어가 이동이 가능한지 불가능한지 정함
        {
            rb.velocity = Vector2.zero;//플레이어의 속도는 zero
            anim.SetBool("moving", Moving);
            return;//밑에 있는 스크립트는 실행시키지 않음
        }
        if (Input.GetAxisRaw("Horizontal") > 0.2f || Input.GetAxisRaw("Horizontal") < -0.2f)//좌우방향표를 누룬다면
        {
            if (Input.GetAxisRaw("Horizontal") > 0.2f)//우로 가는 방향일 경우
            {
                rb.velocity = new Vector2(1.0f * Movespeed * Time.deltaTime, rb.velocity.y);//플레이어의 속도를 설정
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.2f) // 좌로 가는 방향일 경우
            {
                rb.velocity = new Vector2(-1.0f * Movespeed * Time.deltaTime, rb.velocity.y);//플레이어의 속도를 설정
            }
            //anim.SetFloat("x_move", Input.GetAxisRaw("Horizontal"));
            //anim.SetFloat("x_last", lastmove.x);
            Moving = true;//플레이어는 움직임
            lastmove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        if (Input.GetAxisRaw("Vertical") > 0.2f || Input.GetAxisRaw("Vertical") < -0.2f)//위아래 방향표를 누른다면
        {
            if (Input.GetAxisRaw("Vertical") > 0.2f)//아래로 가는 방향
            {
                rb.velocity = new Vector2(rb.velocity.x, 1.0f * Movespeed * Time.deltaTime);//플레이어의 속도를 설정
            }
            else if (Input.GetAxisRaw("Vertical") < -0.2f)//위로 가는 방향
            {
                rb.velocity = new Vector2(rb.velocity.x, -1.0f * Movespeed * Time.deltaTime);//플레이어의 속도를 설정
            }
            //anim.SetFloat("y_move", Input.GetAxisRaw("Vertical"));
            //anim.SetFloat("y_last", lastmove.y);
            Moving = true;//플레이어는 움직임
            lastmove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }
        if (Input.GetAxisRaw("Horizontal") < 0.2f && Input.GetAxisRaw("Horizontal") > -0.2f)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        if (Input.GetAxisRaw("Vertical") < 0.2f && Input.GetAxisRaw("Vertical") > -0.2f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        //애니메이터에서 해당 정보를 받아와 처리함
        anim.SetFloat("x_move", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("y_move", Input.GetAxisRaw("Vertical"));
        anim.SetFloat("x_last", lastmove.x);
        anim.SetFloat("y_last", lastmove.y);
        anim.SetBool("moving", Moving);
    }

    void OnTriggerEnter2D(Collider2D other)//트리거선언된 충돌체와 부딪힐 경우
    {
        if (other.tag == "move_world")//충돌체의 tag가 무브월드라면
        {

            move_manager = other.gameObject.GetComponent<move_world>();//부딪힌 오브젝트가 가진 무브월드 스크립트를 불러옴
            if(!move_manager.white_fade)
            {
                t_manager.fade_black_out_on();//화면을 암전
            }
            else if(move_manager.white_fade)
            {
                t_manager.fade_white_out_on();
            }
            //move_manager = other.gameObject.GetComponent<move_world>();//부딪힌 오브젝트가 가진 무브월드 스크립트를 불러옴
            p_manager = FindObjectOfType<PositionManager>();//포지션매니저를 다시 불러옴
            //포지션매니저에서 다음 씬에 플레이어위치를 읽어들어옴.
            p_manager.x_pos = move_manager.x_start;
            p_manager.y_pos = move_manager.y_start;
            p_manager.x_dir = move_manager.x_dir;
            p_manager.y_dir = move_manager.y_dir;
            p_manager.scene_name = move_manager.worldname;
            if (!move_manager.white_fade)
            {
                StartCoroutine(move_world(0.5f));
            }
            else if (move_manager.white_fade)
            {
                StartCoroutine(move_world(2.5f));
            }
            //StartCoroutine(move_world(0.5f));
            //SceneManager.LoadScene(p_manager.scene_name);
            //p_manager.position_call();
            /*this.transform.position = new Vector2(move_manager.x_start,move_manager.y_start);
            lastmove = new Vector2(move_manager.x_dir,move_manager.y_dir);*/
        }
    }

    public void player_move_world(float x,float y, String world_name)
    {
        p_manager = FindObjectOfType<PositionManager>();
        p_manager.x_pos = x;
        p_manager.y_pos = y;
        p_manager.scene_name = world_name;
        t_manager.fade_black_out_on();
        StartCoroutine(move_world(0.5f));
    }

    public void extra_save_position()//포지션 매니저가 현재 플레이어의 위치정보를 읽어들임
    {
        p_manager.x_pos = tf.transform.position.x;
        p_manager.y_pos = tf.transform.position.y;
        p_manager.x_dir = lastmove.x;
        p_manager.y_dir = lastmove.y;
        p_manager.scene_name = Application.loadedLevelName;
    }

    public void Save_position()//세이브데이터를 저장시키며 현재 플레이어의 위치를 체크포인트로 설정
    {
        PlayerPrefs.SetFloat("load_x_position",tf.transform.position.x);
        PlayerPrefs.SetFloat("load_y_position", tf.transform.position.y);
        PlayerPrefs.SetFloat("load_x_direction", lastmove.x);
        PlayerPrefs.SetFloat("load_y_direction", lastmove.y);
        PlayerPrefs.SetString("load_scene_name", Application.loadedLevelName);
        PlayerPrefs.SetInt("game_save", 1);
    }

    public void Destroy()//플레이어를 삭제하고 존재여부를 없애는 함수
    {
        if(player_exist)
        {
            Destroy(gameObject);
            player_exist = false;
        }
    }
    IEnumerator position_call(float time)//오브젝트 생성시 실행시키는 코루틴
    {
        yield return new WaitForEndOfFrame();//프레임이 끝날때까지 기다린다. 이는 포지션매니저가 플레이어 스크립트를 제대로 읽어오기 위해서다
        p_manager.position_call();//포지션매니저의 위치호출함수를 발동
        yield return new WaitForSeconds(time);//time만큼의 시간을 기다림
        t_manager.fade_black_out_off();//화면을 밝게한다.
    }

    IEnumerator move_world(float time)//무브월드랑 충돌할때 발생되는 코루틴
    {
        yield return new WaitForSeconds(time);//일정시간을 기다린다
        SceneManager.LoadScene(p_manager.scene_name);//씬을 불러온다
        p_manager.position_call();//포지션매니저의 위치호출함수를 발동
        yield return new WaitForSeconds(0.5f);//역시 기다린다
        t_manager.fade_black_out_off();//화면을 밝게한다
    }
}
