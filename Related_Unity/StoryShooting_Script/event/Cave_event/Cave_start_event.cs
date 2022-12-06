using UnityEngine;
using System.Collections;

public class Cave_start_event : MonoBehaviour {
    //동굴에서 처음 시작할 경우 나오는 이벤트 
    public Text_manager t_manager;//텍스트매니저를 선언
    public TextAsset txt;//이벤트용 텍스트파일
	// Use this for initialization
	void Start () {

        t_manager = FindObjectOfType<Text_manager>(); //텍스트매니저를 불러옴
    }
	
	// Update is called once per frame
	void Update () {
        if(t_manager == null)//텍스트매니저가 선언되어있지 않다면(이 경우에는 텍스트매니저가 선언이 안되어있을수 있음.)
        {
            t_manager = FindObjectOfType<Text_manager>();//텍스트매니저를 다시 불러옴
        }
        if (!PlayerPrefs.HasKey("game_save"))//game 저장변수가 값이 아무것도 없다면
        {
            t_manager.text_enable(txt);//이벤트용 텍스트를 띄어버림
            PlayerPrefs.SetInt("game_save", 0);//game저장변수를 대입(단 1이 실제로 게임이 저장되어 있다는 것을 의미함)
        }
        if (t_manager.currentLine > t_manager.endLine)//이벤트용 텍스트가 끝나면
        {
            StartCoroutine(game_start(1f));//게임시작코루틴실행
        }
	}

    IEnumerator game_start(float time)
    {
        yield return new WaitForSeconds(time);//time의 시간이 지나면
        t_manager.fade_black_out_off();//화면이 다시 밝아짐
    }
}
