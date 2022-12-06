using UnityEngine;
using System.Collections;

public class kid_boss_spell_anim : MonoBehaviour {

    public string battle_name;
    public float start_time, spell_time;
    public bool sp_start, rage;
    public Text_manager t_manager;
    public Animator anim;
    public Enemy enemy;
    public PlayerBattleController player;
    public GameObject normal_bullet, trap_making, meteor_make, huge_bullet;
    public bool blink, anim_return;
    private int meteor_count, original_first_health;
    public AudioClip shoot_noise, loop_shoot, teleport;
    public Sound_effect sound;
    public Sound_effect_loop sound_loop;
    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerBattleController>();
        enemy = FindObjectOfType<Enemy>();
        anim = GetComponent<Animator>();
        t_manager = FindObjectOfType<Text_manager>();
        battle_name = "kid_boss_battle";
        enemy.battle_name = battle_name;
        meteor_count = 0;
        original_first_health = enemy.first_health;
        sound = FindObjectOfType<Sound_effect>();
        sound_loop = FindObjectOfType<Sound_effect_loop>();
    }
	
	// Update is called once per frame
	void Update () {
        if (t_manager == null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
        if (start_time >= 0.0f)
        {
            t_manager.fade_black_out_off();
            start_time -= Time.deltaTime;
            return;
        }
        if (start_time < 0.0f)
        {
            if (!sp_start)
            {
                t_manager.sub_text.text = "";
                sp_start = true;
                StartCoroutine("Spell0_start");
            }
        }
        if (enemy.first_health <= 0.0f)
        {
            if (!rage)
            {
                StartCoroutine("rage_text");
                StartCoroutine("rage_spell");
                rage = true;
            }
        }
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("kid_boss_battle_blink_off"))
        {
            anim.SetBool("return", true);
            Debug.Log("aaaaa");
        }
        //anim.SetBool("return", false);
        anim.SetBool("blink", blink);
        anim.SetBool("rage", rage);
        anim.SetBool("choose", enemy.choose_time);
        anim.SetBool("health_die", enemy.anim_health_die);
        anim.SetBool("end", enemy.end_anim);
        anim.SetBool("choose_die", enemy.anim_choose_die);
        anim.SetBool("choose_live", enemy.anim_choose_live);
    }

    IEnumerator Spell0_start()
    {
        yield return new WaitForSeconds(0.5f);
        sound.Play_audio(shoot_noise);
        for (int i = 0; i <  30; i++)
        {
            GameObject bullet = (GameObject)Instantiate(normal_bullet, this.transform.position, Quaternion.identity);
            Vector2 dir = new Vector2(110.0f * Mathf.Sin(Mathf.PI * 2 * i / 30), 110.0f * Mathf.Cos(Mathf.PI * 2 * i / 30));
            bullet.GetComponent<Rigidbody2D>().AddForce(dir);
            yield return new WaitForSeconds(0.08f);
        }
        blink = true;
        enemy.no_hit = true;
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        StartCoroutine("Spell1_start");
    }

    IEnumerator Spell1_start()
    {
        yield return new WaitForSeconds(2.00f);
        int blink_num = 0;
        if(!blink)
        {
            blink = true;
            enemy.no_hit = true;
            this.GetComponent<BoxCollider2D>().isTrigger = true;
            sound.Play_audio(teleport);
            yield return new WaitForSeconds(2.00f);
        }
        for (int loop=0;loop<=2;loop++)
        {
            if (blink_num == 0)
            {
                this.transform.position = new Vector2(0.76f, Random.Range(-3.0f, -0.3f));  
            }
            else if(blink_num==1)
            {
                this.transform.position = new Vector2(-3.5f, Random.Range(-3.0f, -0.3f));
            }
            else
            {
                this.transform.position = new Vector2(Random.Range(-3.5f, 0.7f), -0.66f);
            }
            for (int i = -70; i <= 70; i += 20)
            {
                float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                GameObject chong = (GameObject)Instantiate(normal_bullet, this.transform.position, Quaternion.identity);
                chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 120f);
                sound.Play_audio(shoot_noise);
            }
            blink = false;
            enemy.no_hit = false;
            this.GetComponent<BoxCollider2D>().isTrigger = false;
            if (enemy.first_health<=0.0f)
            {
                break;
            }
            yield return new WaitForSeconds(1.0f);
            if(blink_num!=2)
            {
                blink_num++;
                blink = true;
                enemy.no_hit = true;
                this.GetComponent<BoxCollider2D>().isTrigger = true;
                sound.Play_audio(teleport);
                yield return new WaitForSeconds(1.2f);
            } 
        }
        if(enemy.first_health>0.0f)
        {
            meteor_count++;
            if(meteor_count==3)
            {
                meteor_count = 0;
                StartCoroutine("Spell3_start");
            }
            else if(enemy.first_health>=original_first_health/2)
            {
                StartCoroutine("Spell2_start");
            }
            else if(enemy.first_health<original_first_health/2)
            {
                StartCoroutine("Spell4_start");
            }
        }
    }

    IEnumerator Spell2_start()
    {
        yield return new WaitForSeconds(1.5f);
        for(int loop=0;loop<=2;loop++)
        {
            Instantiate(trap_making, player.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.8f);
            if(enemy.first_health<=0.0f)
            {
                break;
            }
            yield return new WaitForSeconds(2.5f);
            if (enemy.first_health <= 0.0f)
            {
                break;
            }
        }
        if (enemy.first_health > 0.0f)
        {
            meteor_count++;
            if (meteor_count == 3)
            {
                meteor_count = 0;
                StartCoroutine("Spell3_start");
            }
            else if (enemy.first_health >= original_first_health / 2)
            {
                StartCoroutine("Spell1_start");
            }
            else if (enemy.first_health < original_first_health / 2)
            {
                StartCoroutine("Spell4_start");
            }
        }
    }

    IEnumerator Spell3_start()
    {
        blink = true;
        enemy.no_hit = true;
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(1.00f);
        this.transform.position = new Vector3(-1.32f, -0.39f, 0.0f);
        yield return new WaitForSeconds(1.00f);
        blink = false;
        enemy.no_hit = false;
        this.GetComponent<BoxCollider2D>().isTrigger = false;
        Instantiate(meteor_make, this.transform.position, Quaternion.identity);
        while(true)
        {
            yield return new WaitForSeconds(1.5f);
            if(enemy.first_health<=0.0f)
            {
                break;
            }
            yield return new WaitForSeconds(1.5f);
            if (enemy.first_health <= 0.0f)
            {
                break;
            }
            yield return new WaitForSeconds(1.5f);
            if (enemy.first_health <= 0.0f)
            {
                break;
            }
            break;
        }
        if (enemy.first_health > 0.0f)
        {
            meteor_count++;
            if (meteor_count == 3)
            {
                meteor_count = 0;
                StartCoroutine("Spell3_start");
            }
            else if (enemy.first_health >= original_first_health / 2)
            {
                StartCoroutine("Spell1_start");
            }
            else if (enemy.first_health < original_first_health / 2)
            {
                StartCoroutine("Spell4_start");
            }
        }
    }

    IEnumerator Spell4_start()
    {
        yield return new WaitForSeconds(2.0f);
        sound_loop.Play_audio_loop(loop_shoot);
        for(int shoot_num=0;shoot_num<=149 ;shoot_num++)
        {
            if (enemy.first_health <= 0.0f)
            {

                break;
            }
            Vector2 dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            GameObject bullet = (GameObject)Instantiate(normal_bullet, this.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(dir * 80);
            yield return new WaitForSeconds(0.04f); 
        }
        sound_loop.Stop();
        if(enemy.first_health>0.0f)
        {
            meteor_count++;
            if (meteor_count == 3)
            {
                meteor_count = 0;
                StartCoroutine("Spell3_start");
            }
            else
            {
                StartCoroutine("Spell5_start");
            }
        }
    }

    IEnumerator Spell5_start()
    {
        blink = true;
        enemy.no_hit = true;
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        sound.Play_audio(teleport);
        yield return new WaitForSeconds(1.50f);
        for(int blink_attack=0;blink_attack<=6 ;blink_attack++)
        {
            Vector2 blink_position = new Vector2(Random.Range(-3.6f, 0.75f), Random.Range(-3.18f, -0.66f));
            this.transform.position = blink_position;
            blink = false;
            this.GetComponent<BoxCollider2D>().isTrigger = false;
            if(Mathf.Sqrt((player.transform.position.x-blink_position.x)*(player.transform.position.x - blink_position.x) + (player.transform.position.y - blink_position.y)* (player.transform.position.y - blink_position.y))>=1.0f)
            {
                for (int i = -50; i <= 50; i += 20)
                {
                    float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                    Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                    GameObject chong = (GameObject)Instantiate(normal_bullet, this.transform.position, Quaternion.identity);
                    chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 90f);
                }
                sound.Play_audio(shoot_noise);
            }
            else
            {
                blink_attack--;
            }
            yield return new WaitForSeconds(0.8f);
            blink = true;
            this.GetComponent<BoxCollider2D>().isTrigger = true;
            sound.Play_audio(teleport);
            yield return new WaitForSeconds(0.8f);
            if(enemy.first_health<=0.0f)
            {
                break;
            }
        }
        blink = false;
        enemy.no_hit = false;
        this.GetComponent<BoxCollider2D>().isTrigger = false;
        yield return new WaitForSeconds(1.5f);
        if (enemy.first_health > 0.0f)
        {
            meteor_count++;
            if (meteor_count == 3)
            {
                meteor_count = 0;
                StartCoroutine("Spell3_start");
            }
            else
            {
                StartCoroutine("Spell4_start");
            }
        }
    }

    IEnumerator rage_spell()
    {
        blink = true;
        enemy.no_hit = true;
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        sound.Play_audio(teleport);
        yield return new WaitForSeconds(2.0f);
        this.transform.position = new Vector2(-1.32f, -2.30f);
        yield return new WaitForSeconds(1.0f);
        blink = false;
        enemy.no_hit = false;
        this.GetComponent<BoxCollider2D>().isTrigger = false;
        sound_loop.Play_audio_loop(loop_shoot);
        while(true)
        {
            enemy.no_hit = false;
            if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
            {
                sound_loop.Stop();
                break;
            }
            for (int k = 0; k < 150; k++)
            {
                if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
                {
                    break;
                }
                for (int i = -120; i <= 120; i += 120)
                {
                    float angle = i * Mathf.Deg2Rad + Mathf.Atan2(Mathf.Cos(Mathf.PI * 2 * k / 25 - Mathf.Deg2Rad*90), Mathf.Sin(Mathf.PI * 2 * k / 25 + Mathf.Deg2Rad * 90));
                    Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                    GameObject chong = (GameObject)Instantiate(normal_bullet, this.transform.position, Quaternion.identity);
                    chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir*80f);
                }
                yield return new WaitForSeconds(0.08f);
                if(k==60 || k==90 || k == 115 || k== 135 || k== 149)
                {
                    for (float power = 150f; power >= 30.0f; power -= 20.0f)
                    {
                        for (int i = -25; i <= 25; i += 25)
                        {
                            float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                            Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                            GameObject chong = (GameObject)Instantiate(normal_bullet, this.transform.position, Quaternion.identity);
                            chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * power);
                        }
                    }
                    sound.Play_audio(shoot_noise);
                       
                }
            }
        }
    }

    IEnumerator rage_text()
    {
        t_manager.sub_text.text = "이방인! 좌절을 느끼고 그만 죽어라!";
        yield return new WaitForSeconds(2.50f);
        t_manager.sub_text.text = "";
    }
}
