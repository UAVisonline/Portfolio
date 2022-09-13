using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DirectorObject : MonoBehaviour
{
    [System.Serializable]
    public struct direct_information // 직렬화 구조체, 연출 개개인의 요소를 설정함
    {
        public string function_name;// 명령어
        public float action_time; // 실행지점
        public int value; // 정수 인자
        public float value_float; // 실수 인자
        public int animation_value; // animation State용 정수 인자
    }

    [TableList] [SerializeField] private List<direct_information> directions; // 연출 요소를 배열로 설정

    [TabGroup("Light Color samples")] [SerializeField] private List<Color> colors; // Light 색깔 배열
    [TabGroup("Light Reference")] [SerializeField] private List<LightObject> lightObjects; // LightObject 배열
    [TabGroup("Light Reference")] [SerializeField] private Light sun_object; // 제일 위 조명, 없어도 상관은 없음

    [TabGroup("GameObject Reference")] [SerializeField] private List<DirectGameObject> gameobjects; // 연출용 게임오브젝트 배열
    [TabGroup("GameObject Reference")] [SerializeField] private Material first_skybox; // skybox material -> 이걸 배열로 선언해 skybox 바꾸는 연출 넣을려고 했는데 실패함

    [TabGroup("Particle Object Reference")] [SerializeField] private List<ParticleDirectObject> particleobjects; // 파티클 오브젝트 배열

    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private Color light_color; // 현재 설정된 light color
    [BoxGroup("Variable")] [SerializeField] private bool direct_status; // 넌 뭐냐??? 왜 만들었지??? 무슨 목적으로??? 게임 일시정지 용으로 만든건가???
    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private float time; // 현재 음악의 재생위치... 근데 이건 인자로 받아 처리하는데 왜 만들었지???
    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private int direct_position; // directions 배열의 현재 위치를 이 놈이 표기 
    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private Camera main_camera; // main Camera 멤버변수 -> 이놈한테 skybox 존재함

    [BoxGroup("Refernece")] [SerializeField] private Material common_material;

    private int index;

    private void Awake()
    {
        main_camera = Camera.main;

        time = 0.5f;
        Direct_update(time);
    }

    public void Direct_update(float time) // 인자로 받은 음악 재생시간에 맞추어 연출 재생
    {
        if (direct_position >= directions.Count) return; // 모든 연출 재생

        if(directions[direct_position].action_time <= time) 
        {
            if(directions[direct_position].function_name[0]!='=') // function name이 =로 시작안함 : 에디터 내 function name이 ======= 되있는것이 있음. 보기 편하게 할려고 만들었는데 이걸 인식하면 안되니까 조건문으로 예외처리
            {
                switch (directions[direct_position].function_name)
                {
                    case "Light_turn_on": // value Light를 켭니다
                        Light_turn_on(directions[direct_position].value);
                        break;
                    case "Light_turn_off": // value Light를 끕니다
                        Light_turn_off(directions[direct_position].value);
                        break;
                    case "Light_change": // value Light의 색을 light color로 바꿉니다 <현재 code 한계 상 이렇게만 바꿀 수 있음...>
                        Light_change(directions[direct_position].value);
                        break;
                    case "Light_animation": // value Light에 대해 animation value 값 애니메이션을 실행시킵니다
                        Light_animation_set(directions[direct_position].value, directions[direct_position].animation_value);
                        break;
                    case "set_light_color": // light color를 value index로 바꿉니다
                        set_light_color(directions[direct_position].value);
                        break;

                    case "Sun_power_change": // Sun light의 intensity를 value float로 바꿉니다
                        sun_object.intensity = directions[direct_position].value_float;
                        break;
                    case "Sun_light_change": // Sun light의 light color로 바꿉니다
                        sun_object.color = light_color;
                        break;

                    case "Skybox_set": // Skybox를 변경합니다
                        if(main_camera.GetComponent<Skybox>()!=null)
                        {
                            main_camera.GetComponent<Skybox>().material = first_skybox;
                        }
                        break;
                    case "Skybox_light_set":
                        if (main_camera.GetComponent<Skybox>() != null)
                        {
                            main_camera.GetComponent<Skybox>().material.SetColor("_Tint", light_color);
                        }
                        break;
                    case "Skybox_power_set":
                        if (main_camera.GetComponent<Skybox>() != null)
                        {
                            main_camera.GetComponent<Skybox>().material.SetFloat("_Exposure", directions[direct_position].value_float);
                        }
                        break;

                    case "GameObject_turn_on": // value Gameobject를 켭니다
                        GameObject_turn_on(directions[direct_position].value);
                        break;
                    case "GameObject_turn_off": // value Gameobject를 끕니다
                        GameObject_turn_off(directions[direct_position].value);
                        break;
                    case "GameObject_animation": // value Gameobjet의 animation value Animation을 실행합니다
                        Gameobject_animation_set(directions[direct_position].value, directions[direct_position].animation_value);
                        break;
                    case "GameObject_function": // value Gameobject의 animation value 값에 해당하는 function을 실행합니다
                        Gameobject_function(directions[direct_position].value, directions[direct_position].animation_value);
                        break;

                    case "Particle_on": // particle object를 켭니다
                        particleobjects[directions[direct_position].value].gameObject.SetActive(true);
                        break;
                    case "Particle_off": // particle object를 끕니다
                        particleobjects[directions[direct_position].value].gameObject.SetActive(false);
                        break;
                    case "Particle_play":// particle object를 재생
                        particleobjects[directions[direct_position].value].play();
                        break;
                    case "Particle_stop": // particle object를 종료
                        particleobjects[directions[direct_position].value].stop();
                        break;

                    case "Common_color_set":
                        Common_material_color_set(directions[direct_position].value);
                        break;
                    case "Common_glow_set":
                        Common_material_glow_set(directions[direct_position].value);
                        break;

                    case "---------------":
                        break;
                    default:
                        SendMessage(directions[direct_position].function_name);
                        break;

                }
            }
            direct_position += 1;
            Direct_update(time);
        }
    }

    public void set_status(bool value)
    {
        direct_status = value;
    }

    public void init_time() // 게임 재시작 시 연출 오브젝트는 전부 꺼버린다
    {
        for(int i =0;i< lightObjects.Count;i++)
        {
            Light_turn_off(i);
        }

        for(int i =0;i<gameobjects.Count;i++)
        {
            GameObject_turn_off(i);
        }

        for(int i =0;i<particleobjects.Count;i++)
        {
            particleobjects[i].gameObject.SetActive(false);
        }

        direct_position = 0;
    }

    private void set_index(int value)
    {
        index = value;
    }

    private void Light_turn_on(int value) 
    {
        lightObjects[value].gameObject.SetActive(true);
    }

    private void Light_turn_off(int value)
    {
        lightObjects[value].gameObject.SetActive(false);
    }

    private void Light_change(int value)
    {
        lightObjects[value].set_color_instant(light_color);
    }

    private void Light_animation_set(int value, int animation_value) 
    {
        string name = "Play";
        switch (animation_value)
        {
            case 0:
                name = "Idle";
                break;
            case 1:
                name += "1";
                break;
            case 2:
                name += "2";
                break;
            case 3:
                name += "3";
                break;
            case 4:
                name += "4";
                break;
            case 5:
                name += "5";
                break;
        }
        lightObjects[value].play_animation(name);
    }

    private void set_light_color(int value) 
    {
        light_color = colors[value];
    }

    private void GameObject_turn_on(int value)
    {
        gameobjects[value].gameObject.SetActive(true);
    }

    private void GameObject_turn_off(int value)
    {
        gameobjects[value].gameObject.SetActive(false);
    }

    private void Gameobject_animation_set(int value, int animation_value)
    {
        string name = "Play";
        switch (animation_value)
        {
            case 0:
                name = "Idle";
                break;
            case 1:
                name += "1";
                break;
            case 2:
                name += "2";
                break;
            case 3:
                name += "3";
                break;
            case 4:
                name += "4";
                break;
            case 5:
                name += "5";
                break;
            case 6:
                name += "6";
                break;
            case 7:
                name += "7";
                break;
            case 8:
                name += "8";
                break;
            case 9:
                name += "9";
                break;
        }
        gameobjects[value].play_animation(name);
    }

    private void Gameobject_function(int value, int function_value)
    {
        switch(function_value)
        {
            case 0:
                gameobjects[value].function0();
                break;
            case 1:
                gameobjects[value].function1();
                break;
            case 2:
                gameobjects[value].function2();
                break;
            case 3:
                gameobjects[value].function3();
                break;
            case 4:
                gameobjects[value].function4();
                break;
        }
    }

    private void Common_material_color_set(int pos)
    {
        common_material.SetColor("_ShapeColor", colors[pos]);
    }

    private void Common_material_glow_set(int pos)
    {
        common_material.SetColor("_GlowColor", colors[pos]);
    }
}
