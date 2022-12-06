using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Text_manager : MonoBehaviour {

    public string[] text_list;//대사파일을 받아서 대사로 받아야하는 문자열
    public TextAsset dialogue;//대사파일
    public AudioClip text_sound;
    public Text text, sub_text;//대사 텍스트
    public GameObject textbox, fade_box;//텍스트박스 : 대사창    페이드박스 : 페이드용 창
    public bool Player_moving, fade_moving, first_dialogue, fade_on, first_text_sound;//플레이어무빙 : 대사가 나올때 플레이어는 못 움직임, 페이드 무빙 : 페이드가 되있을때 플레이어는 못 움직임, 페이드 온 : 페이드가 켜져있을때, 퍼스트 다이얼로그 : ???
    public int currentLine, endLine;//현재 대사줄과 모든 대사의 줄
    public float dialogue_time,original_dialogue_time, fade_time, original_fade_time, fade_white_time,fade_black_time;//다이얼로그타임 : 이 시간이 다해야 다음 대사로 넘길수 있음,   페이드 타임 : 이 시간을 통해서 페이드 오브젝트의 알파값조정
    public static bool exist;//텍스트매니저 존재 여부
    public Image fade_image;//페이드 오브젝트의 이미지를 받아오기 위해 선언한 이미지 컴퍼넌트
    public Color fade_color;//페이드 오브젝트의 컬러를 이 컬러로 바꿈
    private AudioSource fx;

    //대사와 fade_in, fade_out을 동시에 처리함.
	// Use this for initialization
    void Awake()
    {
        //Application.targetFrameRate = 30;
    }

	void Start () {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Player_moving = true;//플레이어는 움직일수 있음
        fade_moving = true;
        textbox_disable();//텍스트박스 사라짐 함수호출
        original_dialogue_time = dialogue_time;//대사시간
        original_fade_time = fade_time;//페이드시간
        if(!exist)//이 오브젝트가 처음 생성되어 있다면
        {
            exist = true;//오브젝트 존재함
            DontDestroyOnLoad(this);//이 오브젝트는 로드해도 파괴되지 않음
        }
        else if(exist)//이미 존재하고 있다면
        {
            Destroy(gameObject);//새로운 오브젝트파괴
        }
        if (dialogue != null)//대사가 있으면
        {
            textbox_enable();//텍스트박스 활성화
            text_list = dialogue.text.Split('\n');//대사파일을 나누어서 대사안에 집어넣음
            endLine = text_list.Length - 1;//마지막줄 초기화
            Player_moving = false;//플레이어는 움직일 수 없음
        }
        fx = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Application.targetFrameRate = 60;
        Debug.Log(Application.targetFrameRate);
        if (Application.targetFrameRate != 60)
        {
            
            Application.targetFrameRate = 60;
        }
        if(textbox == null)//텍스트박스가 null이면
        {
            textbox = GameObject.Find("TextBox");//텍스트박스라는 오브젝트를 찾음
        }
        if(fade_box == null)//페이드박스가 null이면
        {
            fade_box = GameObject.Find("fade_scene"); //페이드박스라는 오브젝트를 찾음
            fade_image = fade_box.GetComponent<Image>();//페이드박스의 이미지 컴퍼너트를 받아옴
        }
        fade_image.color = fade_color;//페이드 이미지는 페이드컬러와 같아짐
        if(dialogue_time > 0.00f)//대사타임이 아직 0이상이면
        {
            dialogue_time -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Return) && currentLine <= endLine && dialogue_time <= 0.00f && dialogue != null)//대사파일이 null이 아니고 대사 타임도 0이하이며 아직 마지막줄까지 도달하지 않았을때 엔터키를 누르면
        {
            textbox_enable();//텍스트박스 활성화
            currentLine += 1;//현재 줄++
            dialogue_time = original_dialogue_time;//대사타임 다시 원상태로
            text.text = text_list[currentLine];//대사내용을 현재 내용으로 바꾸어줌

        }
        if(currentLine > endLine && dialogue != null)//모든 대사를 보았는데 대사가 null이 아니라면
        {
            textbox_disable();//텍스트박스 비활성화
            Player_moving = true;//플레이어는 움직일수 있음
            currentLine = 0;//현재 줄을 0으로 바꿈
            dialogue = null;//대사파일 null
        }
        if(fade_on)//화면암전이 켜져있다면
        {
            if(fade_time <= original_fade_time)//아직 암전시간이 남아있다면
            {
                fade_time += Time.deltaTime;//암전시간을 줄여서 화면을 암전시킴
            }
            fade_color.a = (fade_time / original_fade_time);//화면이 점점 암전됨
        }
        if(!fade_on)//화면 암전이 꺼져있다면
        {
            if(fade_time>=0.0f)
            {
                fade_time -= Time.deltaTime;
            }
            fade_color.a = (fade_time/original_fade_time);//화면 색을 밝게 함
        } 
	}

    public void text_enable(TextAsset txt)//대사를 받아와서 읽어오는 함수
    {
        dialogue = txt;//대사파일을 초기화
        text_list = dialogue.text.Split('\n');//대사를 나누어서 문자열에 집어넣음
        endLine = text_list.Length - 1;//끝줄 초기화
        currentLine = 0;//현재줄 초기화
        Player_moving = false;//플레이어는 움직일 수 없음
        first_dialogue = true;//???
        first_text_sound = false;
        textbox_enable();//텍스트박수 등장
        text.text = text_list[currentLine];//현재 텍스트의 내용을 보여줌
        dialogue_time = original_dialogue_time;//대사시간 원래대로 초기화 
    }

    public void textbox_enable()//대사창 보여줌
    {
        if(first_text_sound)
        {
            fx.PlayOneShot(text_sound);
        }
        else
        {          
            first_text_sound = true;
        }
        textbox.SetActive(true);//대사박스 활성화
        text.gameObject.SetActive(true);//대사 활성화
    }

    public void textbox_disable()//대사창 사라지게함
    {
        textbox.SetActive(false);//대사박스 비활성화
        text.gameObject.SetActive(false);//대사 비활성화
    }

    public void Only_textbox_enable()
    {
        textbox.SetActive(true);
        Player_moving = false;
    }

    public void Only_textbox_disable()
    {
        textbox.SetActive(false);
        Player_moving = true;
    }

    public void fade_black_out_on()//화면암전
    {
        if(!fade_on)
        {
            original_fade_time = fade_black_time;
            fade_time = 0.0f;
            fade_moving = false;//플레이어 못움직임
            fade_on = true;//화면암전 참
            fade_color = new Color(Color.black.r, Color.black.g, Color.black.b, 0.0f);//컬러는 블랙
        }
        
    }

    public void fade_black_out_off()//화면 밝아짐
    {
        if(fade_on)
        {
            fade_moving = true;//플레이어 움직임
            fade_on = false;//화면암전 거짓
                            //fade_time = original_fade_time;//페이드타임을 다시 원래대로
        }

    }

    public void fade_white_out_on()
    {
        if(!fade_on)
        {
            original_fade_time = fade_white_time;
            fade_time = 0.0f;
            fade_moving = false;
            fade_on = true;
            fade_color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.0f);
        }
       
    }
}
