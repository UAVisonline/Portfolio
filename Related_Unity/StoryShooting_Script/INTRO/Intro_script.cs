using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
//게임시작시 나오는 인트로와 관련된 스크립트(이 스크립트로 게임 인트로를 처리함.)
public class Intro_script : MonoBehaviour {

    public TextAsset opening;//오프닝멘트를 저장
    public Text open_txt;//오프닝멘트를 나누어서 출력하기 위해 필요한 글자
    public GameObject intro_image;//로고이미지 게임 오브젝트
    public string[] opening_list;//오프닝 멘트를 나누어서 저장시킬 문자배열
    public float text_time, fade_time, original_text_time, original_fade_time, start_time, scene_time;//fade_time계열은 텍스트가 사라지고 다시 생기는 역할, text_time계열은 텍스트가 지속되어 보이는 시간을 맡는 역할
    //scene_time은 다음 씬으로 넘어가는 것을 해결하는 역할, start_time은 처음 시작시 text가 나오는걸 기다리게 되는 시간
    private float plus_time;//최종 텍스트까지 띄우게 된다면 게임로고가 나오기 까지 소요되는 시간을 뜻함.
    public int current_line, end_line;
    public bool text_ok_sign;//왜 있는거냐?
    public AudioClip intro;
    private AudioSource aduio;
    // Use this for initialization
    void Start () {
        current_line = 0;
        intro_image.SetActive(false);
        text_ok_sign = false;
        opening_list = opening.text.Split('\n');
        end_line = opening_list.Length - 1;
        original_fade_time = fade_time;
        original_text_time = text_time;
        fade_time = 0.00f;
        open_txt = GetComponent<Text>();//자신(이 스크립트는 텍스트에 있음.)의 text component를 받아옴.
        plus_time = 2 * original_fade_time + original_text_time;//플러스타임을 설정
        aduio = GetComponent<AudioSource>();
        aduio.PlayOneShot(intro);
        Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            aduio.Stop();
        }
        if (current_line <= end_line)
        {
            open_txt.text = opening_list[current_line];
        }
        open_txt.color = new Color(open_txt.color.r, open_txt.color.g, open_txt.color.b, (fade_time) / original_fade_time);
        if (current_line == end_line)
        {
            plus_time -= Time.deltaTime;
            if(plus_time <= 0.00f)
            {
                intro_image.SetActive(true);//게임로고등장
                open_txt.color = new Color(0f, 0f, 0f, 0f);//텍스트의 색깔을 투명화
                scene_time -= Time.deltaTime;//다음 씬으로 넘어가기 위한 
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    scene_time = 0.00f;
                }
                if (scene_time <= 0.00f)
                {
                    SceneManager.LoadScene("Main_Screen");
                }
                return;
            }
        }
        if(Input.GetKeyDown(KeyCode.Return))//게임인트로를 스킵할때 엔터를 누르면 문자열을 모두 출력시키고 게임로고가 나옴.
        {
            current_line = end_line;
            plus_time = 0.0f;
            return;  
        }
        if (start_time >= 0.00f)//밑에 있는 스크립트내용은 실행되는 것을 막기 위해서 return을 붙임. 이 내용은 처음 텍스트가 나오기까지 걸리는것을 관여하는 시간을 조정해서 start_time이 모두 소모되면 텍스트가 나오는 것을 관여함.
        {
            start_time -= Time.deltaTime;
            return;
        }
        //밑에 부분은 text가 fade_in, fade_out되는 것을 관여하는 스크립트내용임.
        if(fade_time < original_fade_time && text_time >= 0.00f)//글자가 fade_in시키는 과정인 동시에 그러기 위해서는 text_time이 소모되서는 안되기에 return을 붙임.
        {
            fade_time += Time.deltaTime;
            return;
        }
        if(fade_time >= original_fade_time)//글자가 최대밝기에 도달한다면
        {
            text_time -= Time.deltaTime;
        }
        if(text_time <= 0.00f)//텍스트가 나오는 시간이 끝나면
        {
            fade_time -= Time.deltaTime;//fade_out
            if(fade_time <= 0.00f)//완전히 fade_out이 되었다면
            {
                current_line++;//다음 텍스트로
                text_time = original_text_time;//text_time 초기화
            }
        }
        
	}
}
