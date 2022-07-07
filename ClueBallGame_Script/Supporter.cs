using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Supporter : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text supporter_dialogue; // Supporter가 보일 Text 대화
    [SerializeField] private Image supporter_dialogue_bubble; // Supporter 말풍선

    [SerializeField] private List<Dialogue> dialogue_list; // 기본 대사가 저장될 위치

    [SerializeField] private Dialogue first_sentence; // 게임 시작 시 말하는 대사

    private void OnEnable()
    {
        set_dialogue(first_sentence);
    }

    public void set_dialogue(Dialogue dialogue) // Dialogue 설정 후 출력
    {
        string value = dialogue.get_string();

        if(value!="") // 비어있지 않은 경우
        {
            if(supporter_dialogue_bubble.gameObject.activeSelf==false)
            {
                supporter_dialogue_bubble.gameObject.SetActive(true);
            }
            supporter_dialogue.text = value;
        }
        else // 빈 대사인 경우
        {
            supporter_dialogue_bubble.gameObject.SetActive(false);
            supporter_dialogue.text = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData) // 초상화 클릭 시 랜덤 대사 출력
    {
        int number = Random.Range(0, dialogue_list.Count);
        set_dialogue(dialogue_list[number]);
    }
}
