using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioClip[] Attack_Sound; // 공격용 사운드
    public AudioClip Death_Sound; // 죽을 때 사운드

    [SerializeField] protected bool detection; // 플레이어 탐지
    public float detect_limit_time, follow_limit_time; // 플레이어 탐지제한 시간 및 플레이어 추격제한 시간
    protected float detect_time, follow_time; // 플레이어 탐지 시간 및 플레이어 추격 시간

    [SerializeField] protected bool size_big, size_small, No_effect_bullet; // 몬스터 크기 및 원거리 공격에 대해 무적유무
    protected float invincible_time; // 몬스터 무적 시간 
    protected int super_armor; // 몬스터 넉백까지 걸리는 수치 (슈퍼아머)
    [SerializeField] protected float set_invincible_time, knockback_power; //
    [SerializeField] protected int hp, limit_super_armor;
    [SerializeField] protected string monster_saved_name;

    protected Transform Player_transform;

    protected Animator monster_animator;

    protected void Awake()
    {
        if(PlayerPrefs.GetString(monster_saved_name)=="Die")
        {
            gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    protected void Start()
    {
        monster_animator = this.GetComponent<Animator>();
        Player_transform = FindObjectOfType<Player_Controller>().transform;
    }

    protected void Update()
    {
        if(follow_time>0.0f) // 플레이어를 따라다니는 중
        {
            follow_time -= Time.deltaTime;
        }
        else if(follow_time<=0.0f)
        {
            if(detect_time>0.0f) // 플레이어를 탐지한 경우
            {
                detect_time -= Time.deltaTime;
            }
            else if(detect_time<=0.0f) // 플레이어 탐지도 못한 경우
            {
                detection = false;
            }
        }
        
        if(invincible_time>0.0f)
        {
            invincible_time -= Time.deltaTime;
        }
        monster_animator.SetInteger("HP", hp);
    }

    public void Damaged_bullet(float transform_x,int damage,Vector2 position)
    {
        //Debug.Log(hp);
        if (hp>=1)
        {
            if (invincible_time <= 0.0f)
            {
                GameObject effect;
                if (No_effect_bullet) // 원거리공격에 대해 효과 안 받음
                {
                    effect = Effect_Manager.effect_manager.Get_bullet_effect("no_effect");
                    Dramatic_UI.dramatic_manager.No_effect_bullet_string();
                }
                else // 원거리 공격을 받는 경우
                {
                    if (size_big)
                    {
                        effect = Effect_Manager.effect_manager.Get_bullet_effect("big");
                    }
                    else if (size_small)
                    {
                        effect = Effect_Manager.effect_manager.Get_bullet_effect("small");
                    }
                    else
                    {
                        effect = Effect_Manager.effect_manager.Get_bullet_effect("medium");
                    }
                }

                if (transform_x==1) // 날라오는 총알이 오른쪽으로 향하는 경우
                {
                    effect.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (transform_x == -1) // 왼쪽으로 향하는 경우
                {
                    effect.GetComponent<SpriteRenderer>().flipX = true;
                }
                Instantiate(effect, position, Quaternion.identity); // 이펙트 생성

                if (!No_effect_bullet)
                {
                    hp -= damage; // 체력감소
                    if (hp <= 0)
                    {
                        monster_animator.Play("Hurt");
                    }
                    invincible_time = set_invincible_time; 
                }
            }
        }
        
    }

    public void Damaged_sword(float player_scaled,int damage,int break_armor)
    {
        if(hp>=1)
        {
            if (invincible_time <= 0.0f)
            {
                GameObject effect;
                if (size_big)
                {
                    effect = Effect_Manager.effect_manager.Get_slash_effect("big");
                }
                else if (size_small)
                {
                    effect = Effect_Manager.effect_manager.Get_slash_effect("small");
                }
                else
                {
                    effect = Effect_Manager.effect_manager.Get_slash_effect("medium");
                }

                if (player_scaled < 0.0f)
                {
                    effect.GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (player_scaled >= 0.0f)
                {
                    effect.GetComponent<SpriteRenderer>().flipX = false;
                }
                Instantiate(effect, this.transform.position, Quaternion.Euler(0f, 0f, Random.Range(-8f, 5f)));
                hp -= damage;

                if (hp <= 0)
                {
                    monster_animator.Play("Hurt");
                    KnockBack(player_scaled);
                }
                
                super_armor += break_armor;
                if (super_armor >= limit_super_armor)
                {
                    monster_animator.Play("Hurt");
                    KnockBack(player_scaled);
                    super_armor = 0;
                }
                invincible_time = set_invincible_time;

            }
        }
        
    }

    public virtual void Monster_Dying() // 특정 몬스터 사망에 대하여 (스폰 X)
    {
        if(monster_saved_name=="")
        {
            gameObject.SetActive(false); // 단순 사망
        }
        else
        {
            gameObject.SetActive(false);
            PlayerPrefs.SetString(monster_saved_name, "Die"); // 스폰 금지
        }
    }

    public int return_hp()
    {
        return hp;
    }

    public bool Return_No_effect_bullet()
    {
        return No_effect_bullet;
    }

    public void KnockBack(float dir)
    {
        Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(new Vector3(knockback_power * Time.deltaTime *dir, 0f, 0f), ForceMode2D.Impulse);
    }

    public void detect() // 플레이어 탐지 설정
    {
        detection = true;
        follow_time = follow_limit_time;
        detect_time = detect_limit_time;
    }

    public bool return_detection()
    {
        return detection;
    }

    public void Animation_Attack_Sound(int i) // 어택 사운드 종류 선택 후  출력
    {
        this.GetComponent<AudioSource>().clip = Attack_Sound[i];
        this.GetComponent<AudioSource>().Play();
    }

    private void Animation_Dying_Sound() // 사망 사운드 출력
    {
        if(Death_Sound!=null)
        {
            this.GetComponent<AudioSource>().clip = Death_Sound;
            this.GetComponent<AudioSource>().Play();
        }
    }

}
