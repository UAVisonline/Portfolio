using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    private Image image;
    private Text story_text;
    [SerializeField] private string[] saved_dialogue;
    [SerializeField]private Dialogue State;
    [SerializeField] private bool turn_on_dialogue;
    [SerializeField] private int dialogue_position, string_position;
    private static bool Destory_value;


    private static DialogueSystem _dialogue_System;

    public static DialogueSystem dialogue_System // Singleton 생성
    {
        get
        {
            if (_dialogue_System == null)
            {
                _dialogue_System = FindObjectOfType<DialogueSystem>();
                if (_dialogue_System == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _dialogue_System;
        }
    }

    private void Awake() // Singleton 할당
    {
        if (_dialogue_System == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else if (_dialogue_System != null)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.Find("Dialogue_Panel").GetComponent<Image>();
        story_text = GameObject.Find("Dialogue").GetComponent<Text>();
        dialogue_position = 0;
        string_position = 0;
    }

    void Update()
    {
       if(turn_on_dialogue)
        {
            if (dialogue_position < saved_dialogue.Length) 
            {
                string_input();
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
                {
                    if (string_position < saved_dialogue[dialogue_position].Length) // 대사가 전부 출력 안됐는데 엔터키를 누르면
                    {
                        string_position = saved_dialogue[dialogue_position].Length; 
                        story_text.text = saved_dialogue[dialogue_position]; // 대사 전부 출력
                    }
                    else
                    {
                        StartCoroutine(Dialogue_Set_Off()); 
                    }
                }
                //saved_dialogue[dialogue_position]
            }
        }
    }

    public void Dialogue_input(Dialogue dialogue)
    {
        State = dialogue;
        saved_dialogue = State.GetDialogue();
        StartCoroutine(Dialogue_Set_On());
    }

    public void string_input() // Dialogue string ++ => 대사를 한 글자씩 출력
    {
        if(string_position<saved_dialogue[dialogue_position].Length)
        {
            story_text.text += saved_dialogue[dialogue_position][string_position++];
        }
    }

    public bool Dialogue_status()
    {
        return turn_on_dialogue;
    }

    public void Dialogue_exit() // Dialogue 강제종료 (플레이어 피격에서 사용)
    {
        dialogue_position = saved_dialogue.Length - 1;
        story_text.text = "";
        string_position = 0;
        StartCoroutine(Dialogue_Set_Off());
    }

    IEnumerator Dialogue_Set_On() // Dialogue 실행 코루틴 (대사창 출력)
    {
        if (Player_Controller.player_controller != null)
        {
            Player_Controller.player_controller.Set_Can_Move(false);
        }
        yield return new WaitForEndOfFrame();
        while(image.color.a < 0.6f)
        {
            image.color += new Color(0f, 0f, 0f, 0.1f);
            yield return new WaitForSecondsRealtime(0.03f);
        }
        turn_on_dialogue = true; // 대사 출력 시작
        dialogue_position = 0;
        string_position = 0;
    }

    IEnumerator Dialogue_Set_Off() // Dialogue 종료 코루틴
    {
        yield return new WaitForEndOfFrame();
        if(dialogue_position<saved_dialogue.Length-1) // 아직 모든 Dialogue가 종료되지는 않았다
        {
            dialogue_position += 1;
            story_text.text = "";
            string_position = 0;
        }
        else // 모든 Dialogue가 종료 되었다
        {
            dialogue_position += 1;
            story_text.text = "";
            string_position = 0;
            turn_on_dialogue = false;
            while (image.color.a > 0.0f)
            {
                image.color -= new Color(0f, 0f, 0f, 0.1f);
                yield return new WaitForSecondsRealtime(0.03f);
                //Debug.Log(image.color.a);
            }
            if (Player_Controller.player_controller != null)
            {
                Player_Controller.player_controller.Set_Can_Move(true);
            }
        }
    }
}
