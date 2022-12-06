using UnityEngine;
using System.Collections;

public class Greed_Spell_anim : MonoBehaviour {

    public string battle;
    public GameObject Spell1, Spell2, Spell3;
    public Text_manager t_manager;
    public bool Sp1, Sp2, Sp3, anim_attack,sp_start,rage;
    public float spell_time, start_time;
    public Animator anim;
    public Enemy enemy;
    public PlayerBattleController player;
    private Sound_effect sf;
    private Sound_effect_loop sf_loop;
    public AudioClip  pattern_1;
	// Use this for initialization
	void Start () {
        //PlayerPrefs.DeleteKey("cave_puzzle_2");
        player = FindObjectOfType<PlayerBattleController>();
        enemy = FindObjectOfType<Enemy>();
        anim = GetComponent<Animator>();
        t_manager = FindObjectOfType<Text_manager>();
        sf = FindObjectOfType<Sound_effect>();
        sf_loop = FindObjectOfType<Sound_effect_loop>();
        battle = "greed_battle";
        enemy.battle_name = battle;
	}
	
	// Update is called once per frame
	void Update () {
        if(t_manager == null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
        if(start_time>= 0.0f)
        {
            t_manager.fade_black_out_off();
            start_time -= Time.deltaTime;
            return;
        }
        if(start_time<0.0f)
        {
            if(!sp_start)
            {
                t_manager.sub_text.text = "";
                sp_start = true;
                StartCoroutine("Spell1_start", 1f);
            }  
        }
        if(enemy.first_health>0)
        {
            if (player.transform.position.y - transform.position.y < -3.0f)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -60f * Time.deltaTime);
            }
            else if (player.transform.position.y - transform.position.y > 1.0f)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 60f * Time.deltaTime);
            }
            else
            {
                this.GetComponent<Rigidbody2D>().velocity =Vector2.zero;
            }
        }
        if(enemy.first_health<=0.0)
        {
            if(!rage)
            {
                StartCoroutine("rage_text");
                StartCoroutine("Rage_Spell");
                rage = true;
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        anim.SetFloat("Rage", enemy.first_health);
        anim.SetFloat("second_health", enemy.second_health);
        anim.SetBool("Choose", enemy.choose_time);
        anim.SetBool("choose_die", enemy.anim_choose_die);
        anim.SetBool("die", enemy.end_anim);
        anim.SetBool("Attack", anim_attack);
        anim.SetBool("end", enemy.end_anim);
	}

    IEnumerator Spell1_start(float time)
    {
        yield return new WaitForSeconds(time);
        if(enemy.first_health>0)
        {
            for (int i = -1; i <= 1; i++)
            {
                if(i!=0)
                {
                    Instantiate(Spell1, new Vector3(transform.position.x + 1.0f * i, transform.position.y, 0.0f), transform.rotation);
                }        
            }
            yield return new WaitForSeconds(5.0f);
        } 
        if (enemy.first_health > 0)
        {
            int i = Random.Range(2, 4);
            if (i == 2)
            {
                StartCoroutine("Spell2_start", 0.5f);
            }
            else
            {
                StartCoroutine("Spell3_start");
            }
        }
    }

    IEnumerator Spell2_start(float time)
    {
        yield return new WaitForSeconds(time);
        if (enemy.first_health>0)
        {
            anim_attack = true;
            for (int i = 0; i <= 3; i++)
            {
                GameObject obj = (GameObject)Instantiate(Spell2, transform.position,transform.rotation);
                obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(Mathf.PI * 2 * i / 4 + Mathf.PI / 4), Mathf.Sin(Mathf.PI * 2 * i / 4 + Mathf.PI / 4)).normalized * 60);
            }
            sf.Play_audio(pattern_1);
            yield return new WaitForSeconds(4.5f);
        }
        anim_attack = false;
        if (enemy.first_health > 0)
        {
            int i = Random.Range(1, 3);
            if (i == 1)
            {
                StartCoroutine("Spell1_start", 1f);
            }
            else
            {
                StartCoroutine("Spell3_start");
            }
        } 
    }

    IEnumerator Spell3_start()
    {
        yield return null;
        if(enemy.first_health>0)
        {
            anim_attack = true;
            Instantiate(Spell3, new Vector2(transform.position.x, 4.5f), transform.rotation);
            yield return new WaitForSeconds(6f);
            anim_attack = false;
        }
        if (enemy.first_health > 0)
        {
            int i = Random.Range(1, 3);
            if (i == 1)
            {
                StartCoroutine("Spell1_start", 1f);
            }
            else
            {
                StartCoroutine("Spell2_start",0.5f);
            }
        }
    }

    IEnumerator Rage_Spell()
    {
        yield return null;
        if(enemy.second_health>0 || enemy.rage_time >=0.0f)
        {
            Instantiate(Spell3, new Vector2(transform.position.x, 4.5f), transform.rotation);
            yield return new WaitForSeconds(2f);
        }
        while(true)
        {
            if (enemy.second_health <= 0 || enemy.rage_time <= 0.0f)
            {
                break;
            }
            for(int i=0;i<=3;i++)
            {
                if(i==0)
                {
                    Instantiate(Spell1, new Vector2(-3.5f, 2.85f), transform.rotation);
                }
                else if(i==1)
                {
                    Instantiate(Spell1, new Vector2(3.5f,2.85f), transform.rotation);
                }
                else if(i==2)
                {
                    Instantiate(Spell1, new Vector2(-3.5f, -4.2f), transform.rotation);
                }
                else
                {
                    Instantiate(Spell1, new Vector2(3.5f, -4.2f), transform.rotation);
                }
                yield return new WaitForSeconds(3f);
            }
            yield return new WaitForSeconds(5f);
        }
        
    }

    IEnumerator rage_text()
    {
        t_manager.sub_text.text = "이 몸이 진심으로 덤벼주도록 하지!!!";
        yield return new WaitForSeconds(2.50f);
        t_manager.sub_text.text = "";
    }
}
