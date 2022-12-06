using UnityEngine;
using System.Collections;

public class Save_point_interaction : MonoBehaviour {

    public PlayerController player;//플레이어선언
    public TextAsset save_txt;//세이브할때 나오는 대사 
    public Text_manager t_manager;//텍스트매니저 선언
    public Animator anim;
    public AudioSource audio;
    public AudioClip save_sound;
    public float x_dir,y_dir;//플레이어가 어디방향을 보고있어야 세이브가 되는지를 결정
    public bool can_save;//세이브가능 참거짓변수
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();//플레이어 불러옴
        anim = GetComponentInParent<Animator>();
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(player == null)//플레이어가 선언이 안 되어있다면
        {
            player = FindObjectOfType<PlayerController>();//플레이어를 다시 불러옴
        }
        if(can_save)//세이브가 가능하다면
        {
            if (t_manager.Player_moving && player.lastmove.x == x_dir && player.lastmove.y == y_dir)//플레이어의 방향이 올바르고 텍스트매니저의 플레이어무빙이 참이라면(무빙은 대사가 나올때 거짓이고 이 조건을 붙이지않으면 대사가 나오는 중인지 아닌지를 구별못해서 무한대사가 나오게 됨)
            {
                if (Input.GetKeyDown(KeyCode.Return))//엔터키를 누르면
                {
                    audio.PlayOneShot(save_sound);
                    t_manager.text_enable(save_txt);//대사출력
                    player.Save_position();//플레이어의 위치를 체크포인트화해서 저장
                    StartCoroutine("save_point_animator");
                }
            }
        }
	    
	}

    void OnTriggerEnter2D()
    {
        can_save = true;//세이브가능
        t_manager = FindObjectOfType<Text_manager>();//텍스트매니저 불러옴
    }

    void OnTriggerExit2D()
    {
        can_save = false;//세이브불가능
    }

    IEnumerator save_point_animator()
    {
        anim.SetBool("Save_active", true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("Save_active", false);
    }
}
