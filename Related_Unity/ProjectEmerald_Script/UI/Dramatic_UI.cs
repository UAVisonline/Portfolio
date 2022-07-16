using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dramatic_UI : MonoBehaviour
{
    private Image dramatic_panel; // 페이드인/아웃용 이미지
    public GameObject pause_panel; // 일시정지용 오브젝트
    private Text support_text; // 원거리 공격 효과 없으면 나오는 Text
    private bool load, pause; // Scene Load, Game pause

    private static Dramatic_UI _dramatic_manager;

    public static Dramatic_UI dramatic_manager // Singleton 생성
    {
        get
        {
            if (_dramatic_manager == null)
            {
                _dramatic_manager = FindObjectOfType<Dramatic_UI>();
                if (_dramatic_manager == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _dramatic_manager;
        }
    }

    private void Awake() // Singleton 할당
    {
        if (_dramatic_manager == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else if (_dramatic_manager != null)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dramatic_panel = GameObject.Find("Dramatic_Panael").GetComponent<Image>();
        support_text = GameObject.Find("Support_string").GetComponent<Text>();
        pause_panel = transform.Find("Pause_Panael").gameObject;
        
        load = false;
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.fixedDeltaTime);
        if(!load) // Scene Load하는 중이 아니면
        {
            if(Input.GetKeyDown(KeyCode.Escape)) // Esc키 누르면
            {
                Set_Pause(); // Pause
            }
        }
    }

    public void Set_Pause()
    {
        Player_Controller controller = FindObjectOfType<Player_Controller>(); // 플레이어 오브젝트를 찾고
        if (controller != null) // 해당 오브젝트가 존재하면
        {
            if (!pause) // Pause 중이 아니면
            {
                Time.timeScale = 0.0f;
                pause_panel.SetActive(true);
                pause = true;
                // Pause
            }
            else if (pause) // Pause 중이면
            {
                Time.timeScale = 1.0f;
                pause_panel.SetActive(false);
                pause = false;
                // Pause 해제
            }
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    private void panel_clear() // 패널 투명화
    {
        dramatic_panel.color = Color.clear;
    }

    private void Orange_panel_set_255(float alpha) // 패널을 오렌지 색깔로
    {
        dramatic_panel.color = new Color(255.0f / 255.0f, 147.0f / 255.0f, 0, alpha/255);
    }

    private void Red_panel_set_255(float alpha) // 패널을 빨강 색깔로
    {
        dramatic_panel.color = new Color(255.0f / 255.0f, 0f, 0f, alpha / 255);
    }

    private void Black_panel_set_255(float alpha) // 패널을 검정 색깔로
    {
        dramatic_panel.color = new Color(0f,0f,0f, alpha / 255);
    }

    public void Player_hurt_effect() // 플레이어 피격 효과 재생
    {
        StartCoroutine(Orange_blink_effect());
    }

    public void Warp_scene(string name,float time = 0.3f) // 씬 이동
    {
        StartCoroutine(Warp(name,time));
    }

    public void fade_in(float time = 0.3f) // 패이드 인
    {
        StartCoroutine(Fade_in(time));
    }

    public void fade_out(float time = 0.3f) // 패이드 아웃
    {
        StartCoroutine(Fade_out(time));
    }

    public void No_effect_bullet_string() // 총알이 적에게 효과가 없으면
    {
        if(support_text.color.a==0.0f)
        {
            StartCoroutine(string_support());
            //Debug.Log("www");
        } 
    }

    IEnumerator Orange_blink_effect() 
    {
        Red_panel_set_255(100);
        yield return new WaitForSecondsRealtime(0.06f);// 0.06초간 화면 빨강으로
        Black_panel_set_255(100);
        yield return new WaitForSecondsRealtime(0.02f); // 0.02초간 화면 검정으로
        panel_clear();
    }

    IEnumerator Fade_in(float time)
    {
        //yield return new WaitForEndOfFrame();
        Player_Controller controller = FindObjectOfType<Player_Controller>();
        if (controller != null)
        {
            Player_Controller.player_controller.Set_Can_Move(false); // 플레이어는 움직일 수 없다
        }
        float alpha = 0.0f;
        while(alpha<255.0f)
        {
            alpha += 255.0f * Time.deltaTime / time;// time만큼 시간이 걸려서
            Black_panel_set_255(alpha); // 화면을 암전
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator Fade_out(float time)
    {
        //yield return new WaitForEndOfFrame();
        float alpha = dramatic_panel.color.a*255.0f;
        while (alpha>0.0f)
        {
            alpha -= 255.0f * Time.deltaTime / time; // time만큼 시간이 걸려서
            Black_panel_set_255(alpha); // 화면 암전 해제
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Player_Controller controller = FindObjectOfType<Player_Controller>();
        if (controller != null)
        {
            Player_Controller.player_controller.Set_Can_Move(true); // 플레이어는 움직일 수 있다
        }
    }

    IEnumerator Warp(string name,float time = 0.3f)
    {
        float limit = 0.0f;
        load = true;
        fade_in(time);
        while (limit < time) // time 만큼 페이드 인 대기
        {
            limit += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        SceneManager.LoadScene(name);

        while (SceneManager.GetActiveScene().name != name) // 씬이 Load되지 않았다면
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        if(Player_Controller.player_controller!=null)
        {
            Player_Controller.player_controller.Set_Can_Move(false); // 플레이어는 움직일 수 없음
        }

        while (limit > 0.0f) // time 만금 대기
        {
            limit -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        fade_out(time); // 그 후 페이드 아웃

        if (Player_Controller.player_controller != null)
        {
            Player_Controller.player_controller.Set_Can_Move(true); // 이제 플레이어는 움직일 수 있음
        }
        load = false; // 로드 완료
    }

    IEnumerator string_support()
    {
        float limit = 0.0f;
        while(limit<0.5f)
        {
            support_text.color = new Color(1f, 1f, 1f, (200.0f/255.0f)*limit/0.5f); 
            limit += Time.deltaTime;
            //Debug.Log(limit);
            yield return new WaitForSeconds(Time.deltaTime);
        } // 0.5초동안 text의 투명도를 조절
        
        yield return new WaitForSeconds(1.5f);
        while(limit > 0.0f)
        {
            support_text.color = new Color(1f, 1f, 1f, 1.0f/255.0f + (200.0f / 255.0f) * (limit / 0.5f));
            limit -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        } // 0.5초동안 text의 투명도를 해제
        yield return new WaitForSeconds(1.5f);
        support_text.color = new Color(1f, 1f, 1f, 0f);

    }
}
