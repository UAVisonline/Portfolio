using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DramaticManager : MonoBehaviour
{
    private static DramaticManager _dramaticManager;

    public static DramaticManager dramaticmanager // Singleton 설정
    {
        get
        {
            if (_dramaticManager == null)
            {
                _dramaticManager = FindObjectOfType<DramaticManager>();
                if (_dramaticManager == null)
                {
                    Debug.LogError("There is no DramaticManager Class or Can't load DramaticManager Class");
                }
            }
            return _dramaticManager;
        }
    }

    [SerializeField] private Animator animator;
    [SerializeField] private float scene_time;

    [SerializeField] private bool correct;
    private bool dramatic_bool;

    [SerializeField] private Image human_suspect;
    [SerializeField] private Image computer_suspect;
    [SerializeField] private Image human_tool;
    [SerializeField] private Image computer_tool;
    [SerializeField] private Image human_place;
    [SerializeField] private Image computer_place;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private List<Sprite> human_sprites;
    [SerializeField] private List<Sprite> tool_sprites;
    [SerializeField] private List<Sprite> place_sprites;

    [SerializeField] private Sprite transparent;

    [SerializeField] private List<AudioClip> clips;

    private void Awake()
    {
        dramatic_bool = false; // 화면전환 관련 변수 설정
        animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if(scene_time>0.0f)
        {
            scene_time -= Time.deltaTime;
        }
        animator.SetFloat("Scene", scene_time); // 이를 이용해 화면 전환 효과 구현
    }

    public void set_scene_time(float value)
    {
        scene_time = value;
        animator.SetFloat("Scene", scene_time);
    }

    public void set_correct(bool value)
    {
        correct = value;
    }

    public void animation_start(string name)
    {
        animator.Play(name, -1, 0.0f);
    }

    public void init_image() // animation event
    {
        human_suspect.sprite = transparent;
        human_tool.sprite = transparent;
        human_place.sprite = transparent;
        computer_suspect.sprite = transparent;
        computer_tool.sprite = transparent;
        computer_place.sprite = transparent;
    } // 정보 시각화 내용을 초기화

    public void human_event()
    {
        suspect human = GameManager.gamemanager.get_human_suspect();
        switch (human)
        {
            case suspect.kellog:
                human_suspect.sprite = human_sprites[0];
                break;
            case suspect.rinda:
                human_suspect.sprite = human_sprites[1];
                break;
            case suspect.android:
                human_suspect.sprite = human_sprites[2];
                break;
            case suspect.godrick:
                human_suspect.sprite = human_sprites[3];
                break;
            case suspect.nameless:
                human_suspect.sprite = human_sprites[4];
                break;
        }

        suspect computer = GameManager.gamemanager.get_computer_suspect();
        switch (computer)
        {
            case suspect.kellog:
                computer_suspect.sprite = human_sprites[0];
                break;
            case suspect.rinda:
                computer_suspect.sprite = human_sprites[1];
                break;
            case suspect.android:
                computer_suspect.sprite = human_sprites[2];
                break;
            case suspect.godrick:
                computer_suspect.sprite = human_sprites[3];
                break;
            case suspect.nameless:
                computer_suspect.sprite = human_sprites[4];
                break;
        }
    } // animation event

    public void tool_event()
    {
        murder_tool human = GameManager.gamemanager.get_human_tool();
        switch (human)
        {
            case murder_tool.axe:
                human_tool.sprite = tool_sprites[0];
                break;
            case murder_tool.poision:
                human_tool.sprite = tool_sprites[1];
                break;
            case murder_tool.hammer:
                human_tool.sprite = tool_sprites[2];
                break;
            case murder_tool.knife:
                human_tool.sprite = tool_sprites[3];
                break;
            case murder_tool.crowbar:
                human_tool.sprite = tool_sprites[4];
                break;
            case murder_tool.shovel:
                human_tool.sprite = tool_sprites[5];
                break;
        }

        murder_tool computer = GameManager.gamemanager.get_computer_tool();
        switch (computer)
        {
            case murder_tool.axe:
                computer_tool.sprite = tool_sprites[0];
                break;
            case murder_tool.poision:
                computer_tool.sprite = tool_sprites[1];
                break;
            case murder_tool.hammer:
                computer_tool.sprite = tool_sprites[2];
                break;
            case murder_tool.knife:
                computer_tool.sprite = tool_sprites[3];
                break;
            case murder_tool.crowbar:
                computer_tool.sprite = tool_sprites[4];
                break;
            case murder_tool.shovel:
                computer_tool.sprite = tool_sprites[5];
                break;
        }
    } // animation event

    public void place_event()
    {
        crime_scene human = GameManager.gamemanager.get_human_place();
        switch (human)
        {
            case crime_scene.livingroom:
                human_place.sprite = place_sprites[0];
                break;
            case crime_scene.library:
                human_place.sprite = place_sprites[1];
                break;
            case crime_scene.bathroom:
                human_place.sprite = place_sprites[2];
                break;
            case crime_scene.garden:
                human_place.sprite = place_sprites[3];
                break;
            case crime_scene.kitchen:
                human_place.sprite = place_sprites[4];
                break;
            case crime_scene.warehouse:
                human_place.sprite = place_sprites[5];
                break;
            case crime_scene.bedroom:
                human_place.sprite = place_sprites[6];
                break;
            case crime_scene.dressroom:
                human_place.sprite = place_sprites[7];
                break;
        }

        crime_scene computer = GameManager.gamemanager.get_computer_place();
        switch (computer)
        {
            case crime_scene.livingroom:
                computer_place.sprite = place_sprites[0];
                break;
            case crime_scene.library:
                computer_place.sprite = place_sprites[1];
                break;
            case crime_scene.bathroom:
                computer_place.sprite = place_sprites[2];
                break;
            case crime_scene.garden:
                computer_place.sprite = place_sprites[3];
                break;
            case crime_scene.kitchen:
                computer_place.sprite = place_sprites[4];
                break;
            case crime_scene.warehouse:
                computer_place.sprite = place_sprites[5];
                break;
            case crime_scene.bedroom:
                computer_place.sprite = place_sprites[6];
                break;
            case crime_scene.dressroom:
                computer_place.sprite = place_sprites[7];
                break;
        }
    } // animation event
    // 플레이어 및 컴퓨터가 설정한 정답 정보 시각화

    public void false_image() // 플레이어 선택 및 컴퓨터가 설정한 정보 image object (Setactive False)
    {
        human_suspect.gameObject.SetActive(false);
        human_tool.gameObject.SetActive(false);
        human_place.gameObject.SetActive(false);
        computer_suspect.gameObject.SetActive(false);
        computer_tool.gameObject.SetActive(false);
        computer_place.gameObject.SetActive(false);
    } // animation event

    public void text_change() // animation event (문제를 맞췄느냐? 틀렸느냐?에 따른 연출 차이를 재생)
    {
        if(correct)
        {
            text.text = "Correct!!!";
            sfx_play(1);
        }
        else
        {
            text.text = "UnCorrect...";
            sfx_play(2);
        }
    }

    public void sfx_play(int index) // index 효과음 재생
    {
        if(index < clips.Count)
        {
            this.GetComponent<AudioSource>().PlayOneShot(clips[index]);
        }
    }

    public void set_bool(bool value) // 화면 전환 변수 설정 (value로 설정)
    {
        dramatic_bool = value;
    }

    public bool get_bool() // 화면 전환 변수 반환
    {
        return dramatic_bool;
    }
}
