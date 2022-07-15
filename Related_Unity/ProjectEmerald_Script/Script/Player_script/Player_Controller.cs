using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    private Animator player_animator;// 해당 오브젝트의 컴퍼넌트
    private SpriteRenderer spr;//해당 오브젝트의 스프라이트렌더러
    private Rigidbody2D rb;//해당 오브젝트의 물리컴퍼넌트
    [SerializeField] private float jump_power;//근접공격후 텀 쿨타임
    [SerializeField] private AnimationClip sword_clip, gun_clip, dash_clip, jump_up_clip, jump_sword_clip,Item_clip; // 각 행동에 대한 애니메이션 값
    private float battle_action_time, invincible_time, ready_time, up_time, up_time_limit; // 플레이어 움직임 관련 시간 변수들 (쿨타임개념)
    
    private bool Can_Jump, Shooting, Swording, Rolling, Blink, Moving, look_left, Can_hurt, Using; // (애니메이터 관련 변수)
    private bool Can_move; // 움직임 조건 변수 (대화같은 상태에서 움직이면 안되므로)

    [SerializeField] private AudioSource audio;

    private enum current_State
    {
        S_standing,
        S_running,
        S_swording,
        S_shooting,
        S_rolling,
        S_blessing,
        S_hurting,
        S_dying,
        S_jumping,
        S_jumpswording,
        S_Using,
    }

    private current_State player_state = current_State.S_standing;
    private static Player_Controller _player_controller;

    public static Player_Controller player_controller
    {
        get
        {
            if (_player_controller == null)
            {
                _player_controller = FindObjectOfType<Player_Controller>();
                if (_player_controller == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _player_controller;
        }
    }

    private void Awake()
    {
        up_time_limit = jump_up_clip.length  + 0.30f; // 점프 가능 시간 초기화

        Can_Jump = false; 
        Can_move = true;
        Can_hurt = true;
        Rolling = false;
    }

    void Start()
    {
        this.transform.position = new Vector2(Player_Manager.player_manager.x_return(), Player_Manager.player_manager.y_return());
        
        look_left = Player_Manager.player_manager.left_return();
        if (look_left == true)
        {
            this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        
        spr = GetComponent<SpriteRenderer>();
        player_animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponentInChildren<AudioSource>();
        
    }

    private void FixedUpdate() // 플레이어 움직임 관련 (점프, 이동)
    {
        RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position - new Vector3(0.2f, 0.2f, 0.0f), Vector2.right, 0.4f); // 레이캐스트를 통한 지상과의 충돌 체크
        bool ground = false;
        foreach(var hit in hit2D)
        {
            if(hit.collider.CompareTag("Ground"))
            {
                ground = true;
                break;
            }
        }

        if (ground) // 플레이어가 땅에 있는 경우
        {
                Can_Jump = true;
                if(!Input.GetKey(KeyCode.D)) // 점프키 입력
                {
                    up_time = 0.0f; // 점프 시간 0으로 초기화
                }
        }
        else if (rb.velocity.y < 0.0f && !ground) // 공중에서 떨어지는 중
        {
            Can_Jump = false;
            up_time = up_time_limit + 0.1f; // 현재 점프하는 시간 값을 바꿔 강제로 점프불가능으로 교체
        }

        if (player_state == current_State.S_standing || player_state == current_State.S_running || player_state == current_State.S_jumping || player_state == current_State.S_jumpswording) // 서있거나, 뛰거나, 점프하거나, 점프 공격중이거나
        {
            if (Can_move) // 움직일 수 있으면
            {
                Run(); // 이동

                if (player_state != current_State.S_jumpswording) // 점프 공격중이 아니면
                {
                    flip();
                    if (!ground) // 근데 땅에 안 붙어 있으면
                    {
                        player_state = current_State.S_jumping; // 점프 중
                    }
                }

                if (Can_Jump) // 점프 가능할 때
                {
                    RaycastHit2D[] up_hit2D = Physics2D.RaycastAll(transform.position + new Vector3(0.2f, 2.4f, 0.0f), Vector2.left, 0.4f); // 플레이어 위쪽 레이캐스트
                    bool not_up = false; 
                    foreach(var hit in up_hit2D)
                    {
                        if (hit.collider.CompareTag("Ground"))
                        {
                            not_up = true; // 위에 지형이 존재
                            break;
                        }
                    }
                    Debug.DrawRay(transform.position + new Vector3(0.2f, 2.4f, 0.0f), Vector2.left * 0.4f, Color.red);

                    if(!not_up && ready_time<=0.0f && rb.mass>=1.0f) // 위가 비어있으면서 플레이어 질량이 1.0이상이면서 준비시간이 0.0 이하이면
                    {
                        if (Input.GetKey(KeyCode.D))
                        {
                            if (up_time < up_time_limit) // 현재 점프 시간이 제한시간을 넘지 않은 경우에 대해
                            {
                                up_time += Time.deltaTime;
                                if(up_time<jump_up_clip.length/2)
                                {
                                    rb.AddForce(new Vector2(0.0f, jump_power * 2.5f * (up_time_limit - up_time)), ForceMode2D.Impulse);
                                }
                                else if(up_time >= jump_up_clip.length / 2 && up_time<jump_up_clip.length)
                                {
                                    rb.AddForce(new Vector2(0.0f, jump_power * 1.8f * (up_time_limit - up_time)), ForceMode2D.Impulse);
                                }
                                else
                                {
                                    rb.AddForce(new Vector2(0.0f, jump_power * 1.2f * (up_time_limit - up_time)), ForceMode2D.Impulse);
                                }   
                                // 시간에 따라 점프로 올라가는 부분 변화
                            }
                        }
                    } 
                }
            }
            //Debug.DrawRay(transform.position - new Vector3(0.2f, 0.2f, 0.0f), Vector2.right * 0.4f, Color.red);
        }
        player_animator.SetBool("Can_jump", ground); // 점프애니메이션을 자연스럽게 만들어줌
    }

    
    void Update()
    {
        if (invincible_time > 0.0f) // 무적 시간이 있으면
        {
            invincible_time -= Time.deltaTime;
        }
        else if (invincible_time <= 0.0f) // 무적 시간이 없으면
        {
            if (Input.GetKeyDown(KeyCode.Backspace)) // 테스트용 (백스페이스 -> 플레이어 피격 진행)
            {
                // Hurt(-1.0f*this.transform.localScale.x);
            }
        }
        if (player_state == current_State.S_shooting || player_state == current_State.S_swording || player_state == current_State.S_rolling || player_state == current_State.S_Using) // 쏘거나, 베거나, 점멸하거나, 아이템을 쓰면
        {
            if (battle_action_time > 0.0f) // 전투 행동 시간이 있으면 (현재 전투 행동 진행중)
            {
                battle_action_time -= Time.deltaTime;
            }
            else if (battle_action_time <= 0.0f) // 전투 행동 시간이 없으면
            {
                Swording = false;
                Shooting = false;
                Rolling = false;
                Using = false;
                
                player_state = current_State.S_standing;
            }
        }

        if (player_state == current_State.S_standing || player_state == current_State.S_running) // 뛰거나 서있으면
        {
            if(ready_time>0.0f) // 준비 시간이 있으면 (다른 전투 행동을 할 수 없음)
            {
                ready_time -= Time.deltaTime;
            }
            if(ready_time<=0.0f) // 준비 시간이 없으면
            {
                if (Input.GetKeyDown(KeyCode.A)) // 원거리 공격
                {
                    Battle_Action("shoot");
                }
                else if (Input.GetKeyDown(KeyCode.S)) // 근거리 공격
                {
                    Battle_Action("sword");
                }
                else if(Input.GetKeyDown(KeyCode.W)) // 아이템 사용
                {
                    if(Player_Manager.player_manager.frecuency_return()>0) // 아이템 사용이 가능하면
                    Battle_Action("Item");
                }
            }
            if (Input.GetKeyDown(KeyCode.Space)) // 점멸
            {
                if (Rolling == false)
                {
                    Battle_Action("Roll");
                }
            }
        }

        if(player_state == current_State.S_jumpswording) // 점프 공격 중이면
        {
            if (battle_action_time > 0.0f)
            {
                battle_action_time -= Time.deltaTime;
            }
            else if (battle_action_time <= 0.0f)
            {
                Swording = false;
                Shooting = false;
                Rolling = false;
                Using = false;
                if(rb.velocity.y!=0.0f)
                {
                    player_state = current_State.S_jumping;
                }
                else
                {
                    player_state = current_State.S_standing;
                }
            }
        }

        
        if(player_state == current_State.S_jumping) // 점프 중이면
        {
            if (ready_time > 0.0f)
            {
                ready_time -= Time.deltaTime;
            }
            if(ready_time<=0.0f)
            {
                if (Input.GetKeyDown(KeyCode.S)) // 거기서 근거리 공격을 누르면
                {
                    Jump_Action("sword");
                }
            }
        }
        
        player_animator.SetBool("Gun_fight", Shooting);
        player_animator.SetBool("Swording", Swording);
        player_animator.SetBool("Rolling", Rolling);
        player_animator.SetBool("Using", Using);
        player_animator.SetInteger("y_velocity", (int)rb.velocity.y);
        player_animator.SetInteger("HP", Player_Manager.player_manager.hp_return());
    }

    private void Run()
    {
        if (Input.GetAxisRaw("Horizontal") != 0.0f) // 수평 키보드값이 0이 아닐때(입력이 존재할 때)
        {
            if(player_state != current_State.S_jumpswording) // 점프 공격 중이 아니면
            {
                player_state = current_State.S_running; // 플레이어는 뛰는 중
                rb.velocity = (new Vector3(500.0f * Input.GetAxisRaw("Horizontal") * Time.deltaTime, rb.velocity.y, 0.0f)); //움직인다
            }
            else // 공중 공격 중이면
            {
                if (rb.velocity.y != 0.0f)
                {
                    rb.velocity = (new Vector3(350.0f * Input.GetAxisRaw("Horizontal") * Time.deltaTime, rb.velocity.y, 0.0f)); // 더 느린 속도로 움직인다
                }   
            }
            Moving = true;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0.0f) //입력이 없을 때
        {
            if(player_state != current_State.S_jumpswording) // 점프 공격 중이 아니면
            {
                player_state = current_State.S_standing; // 플레이어는 가만히 서 있는 중
            }
            Moving = false;
        }
        player_animator.SetBool("Moving", Moving);
    }

    private void flip() // 플레이어 좌우변경 관련
    {
        if (Input.GetAxisRaw("Horizontal") > 0.0f)
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if(Input.GetAxisRaw("Horizontal") < 0.0f)
        {
            this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        if (this.transform.localScale.x == 1.0f)
        {
            look_left = false;
        }
        else if (this.transform.localScale.x == -1.0f)
        {
            look_left = true;
        }
    }

    public void Battle_Action(string str) // 전투 행동 실행
    {
        if(Can_move) // 움직일 수 있을 때
        {
            if (str == "shoot")
            {
                battle_action_time = gun_clip.length;
                player_state = current_State.S_shooting;
                Shooting = true;
                //player_animator.Play("Player_Gun_fight");
            }
            else if (str == "sword")
            {
                battle_action_time = sword_clip.length;
                player_state = current_State.S_swording;
                Swording = true;
                //player_animator.Play("Player_Swording");
            }
            else if (str == "Roll")
            {
                if(Player_Manager.player_manager.stamin_return()>=8)
                {
                    Player_Manager.player_manager.stamina_caculate(-8);
                    player_state = current_State.S_rolling;
                    battle_action_time = dash_clip.length;
                    Rolling = true;
                    Blink = false;
                } 
            }
            else if(str=="Item")
            {
                battle_action_time = Item_clip.length;
                player_state = current_State.S_Using;
                Using = true;
            }
            
            Moving = false;
        }
    }

    public void Jump_Action(string str) // 점프 행동 실행
    {
        if(str == "sword")
        {
            battle_action_time = jump_sword_clip.length; 
            player_state = current_State.S_jumpswording;
            Swording = true;
        }
    }

    public void Dash_Start(float power) // animation event (대시 시작)
    {
        rb.gravityScale = 0.0f;
        rb.mass = 0.0f;
        Can_hurt = false;
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.8f);
        float divide = 1.0f;

        if(transform.GetComponentInChildren<Dash_Support_script>().return_ground_check())
        {
            divide = 2.0f;
        }

        if (!look_left)
        {
            rb.AddForce(new Vector2(1.0f * power * Time.deltaTime / divide, 0.0f), ForceMode2D.Force);
        }
        else if (look_left)
        {
            rb.AddForce(new Vector2(-1.0f * power * Time.deltaTime / divide, 0.0f), ForceMode2D.Force);
        }
    }

    public void Dash_end() // animation event  (대시 종료)
    {
        rb.mass = 1.0f;
        Can_hurt = true;
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 1.0f);
        Blink = false;

        if (!look_left)
        {
            rb.velocity = new Vector3(400.0f * Time.deltaTime, 0.0f, 0.0f);           
        }
        else if (look_left)
        {
            rb.velocity = new Vector3(-400.0f * Time.deltaTime, 0.0f, 0.0f);
        }
    }

    public void gravity_return() //Animation에서 사용중이니 지우지 말것 (Player->Rolling Animation)
    {
        rb.gravityScale = 25.0f;
        rb.mass = 1.0f;
    }

    public void Hurt(float dir,float time = 1.2f, float power = 800.0f)
    {
        if(Can_hurt) // 플레이어가 다칠 수 있는 상태 (씬 로딩 중 공격받아 죽으면 안되므로)
        {
            if(invincible_time<=0.0f)
            {
                if (Player_Manager.player_manager.hp_return() > 0) // 체력이 0 초과인 경우
                {
                    if(DialogueSystem.dialogue_System.Dialogue_status()) // 현재 대화 중이면
                    {
                        DialogueSystem.dialogue_System.Dialogue_exit(); // 대화 강제로 종료
                    }

                    invincible_time = time; // 무적 타임 설정
                    player_animator.Play("Player_Hurt");
                    Player_Manager.player_manager.hp_caculate(-1);
                    rb.velocity = new Vector3(dir * power * Time.deltaTime, 0.0f, 0.0f); // 플레이어 넉백
                    StartCoroutine(Blink_Player());
                    Dramatic_UI.dramatic_manager.Player_hurt_effect();
                    battle_action_time = 0.0f; // 전투 행동 종료
                    Rolling = false;
                    Shooting = false;
                    Swording = false;
                    Using = false;
                    player_state = current_State.S_hurting;
                }

                if (Player_Manager.player_manager.hp_return() <= 0) // 체력이 0 이하이면 (else if가 아니라서 위 조건을 확인 후 역시 확인)
                {
                    player_state = current_State.S_dying;
                    StartCoroutine("Game_over");
                }
            } 
        }  
    }

    public void State_Standing() // Animation에서 자주 사용하는 거니 절대로 없애지 말것
    {
        player_state = current_State.S_standing;
    }

    public void State_Jumping() // Animation 전용 이벤트
    {
        player_state = current_State.S_jumping;
    }

    public void ready_time_set(float time)
    {
        ready_time = time;
    }

    private void Animation_gun_fight() // 애니메이션에서 사용하는 원거리 공격 함수 (발사체 생성)
    {
        Transform tf = GameObject.Find("Gun_fire").GetComponent<Transform>();
        Instantiate(bullet, tf.transform.position, Quaternion.identity);
    }

    public void animation_error_control(string str) // Animation 변수 제어용 이벤트 (아래 변수가 바로 적용되지 않는 현상 때문에 추가)
    {
        if(str == "sword")
        {
            Swording = false;
        }
        if(str == "shoot")
        {
            Shooting = false;
        }
        if(str == "Roll")
        {
            Rolling = false;
        }
        if(str=="Item")
        {
            Using = false;
        }
    }

    public void Set_Can_Move(bool status)
    {
        if(Player_Controller.player_controller != null)
        {
            Can_move = status;
            if (status == false)
            {
                Moving = false;
                player_animator.SetBool("Moving", Moving);
            }
        }
    }

    public void Set_Can_hurt(bool status)
    {
        if (Player_Controller.player_controller != null)
        {
            Can_hurt = status;
        }
    }

    public bool Return_Can_Hurt()
    {
        return Can_hurt;
    }

    public bool Return_Can_Move()
    {
        return Can_move;
    }

    public bool Return_left()
    {
        return look_left;
    }

    public float return_x_pos()
    {
        return this.transform.position.x;
    }

    public float return_y_pos()
    {
        return this.transform.position.y;
    }

    public bool Is_State_Can_Talking()
    {
        if (player_state == current_State.S_standing || player_state == current_State.S_running)
        {
            return true;
        }   
        else
        {
            return false;
        }
    }

    public void Player_Sound_Play(AudioClip audioClip, float pitch,float volume = 1.0f)
    {
        audio.Stop();
        audio.clip = audioClip;
        audio.pitch = pitch;
        audio.volume = volume;
        audio.Play();
    }

    private void Player_Sound_Play_Animation()
    {    
        audio.Play();
    }

    private void Player_sound_manager_Play_Animation()
    {
        Player_Audio_manager.player_audio_manager.Player_sound_process();
    }

    private void manager_sound_clip_set(string str) // 플레이어 오디오 매니저 소리를 설정
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/" + str);
        Player_Audio_manager.player_audio_manager.Sound_clip_set(clip);
    }

    private void Sound_clip_set(string str) // 플레이어 오디오를 설정
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/"+str);
        audio.clip = clip;
    }

    private void manager_sound_pitch_set(float pitch = 1.0f)
    {
        Player_Audio_manager.player_audio_manager.Sound_pitch_set(pitch);
    }

    private void Sound_pitch_set(float pitch = 1.0f)
    {
        audio.pitch = pitch;
    }

    private void manager_sound_volume_set(float volume = 1.0f)
    {
        Player_Audio_manager.player_audio_manager.Sound_volume_set(volume);
    }

    private void Sound_volume_set(float volume = 1.0f)
    {
        audio.volume = volume;
    }

    private void Animation_Item_using()
    {
        Player_Manager.player_manager.hp_caculate(2);
        Player_Manager.player_manager.Item_frecuency_caculate(-1);
    }

    IEnumerator Blink_Player()
    {
        int i = 0;
        while (invincible_time > 0.0f)
        {
            if(Player_Manager.player_manager.hp_return()<=0)
            {
                break;
            }
            if (spr.color.a == 1.0f)
            {
                spr.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
            }
            else
            {
                spr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            yield return new WaitForSecondsRealtime(0.1f);
            //Debug.Log(i++);
        }
        if (spr.color.a != 1.0f)
        {
            spr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    IEnumerator Game_over()
    {
        yield return new WaitForSeconds(1.5f);
        Dramatic_UI.dramatic_manager.fade_in(0.5f);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameOver");
    }
}
