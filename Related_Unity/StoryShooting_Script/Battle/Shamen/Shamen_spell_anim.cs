using UnityEngine;
using System.Collections;

public class Shamen_spell_anim : MonoBehaviour {

    public string battle_name;
    public float start_time, spell_time, speed;
    public bool sp_start, rage;
    public Text_manager t_manager;
    public Animator anim;
    public Enemy enemy;
    public PlayerBattleController player;
    public Sound_effect sound_ef;
    public GameObject homing_white, homing_red, white, red, blue_energy, rage_bullet;
    private float original_first_health, original_second_health, original_rage_time, rage_speed;
    private bool hide;
    private int divide_attack_num;
    public AudioClip bit_magic;
    //private int homing_count, red_homing_timing;
    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerBattleController>();
        enemy = FindObjectOfType<Enemy>();
        anim = GetComponent<Animator>();
        t_manager = FindObjectOfType<Text_manager>();
        sound_ef = FindObjectOfType<Sound_effect>();
        battle_name = "shamen_battle";
        enemy.battle_name = battle_name;
        original_first_health = enemy.first_health;
        original_second_health = enemy.second_health;
        original_rage_time = enemy.rage_time;
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
                //this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                enemy.no_hit = true;
                hide = true;
                StartCoroutine("Spell2_start");
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
        anim.SetBool("hide",hide);
        anim.SetBool("rage", rage);
        anim.SetBool("health_die", enemy.anim_health_die);
        anim.SetBool("choose", enemy.choose_time);
        anim.SetBool("choose_die", enemy.anim_choose_die);
        anim.SetBool("choose_live", enemy.anim_choose_live);
        anim.SetBool("end", enemy.end_anim);
    }

    IEnumerator Spell1_start()
    {
        float homing_respawn_time, original_respawn_time;
        int red_appear, original_red_appear;
        bool fast;
        if(enemy.first_health>=original_first_health*2/3)
        {
            homing_respawn_time = 1.0f;
            red_appear = 3;
        }
        else if(enemy.first_health>=original_first_health/3)
        {

            homing_respawn_time = 0.80f;
            red_appear = 5;
        }
        else
        {
            homing_respawn_time = 0.6f;
            red_appear = 7;
        }
        original_respawn_time = homing_respawn_time;
        original_red_appear = red_appear;
        red_appear = 0;
        for(float end_time=0;end_time<=20.0f;end_time+=original_respawn_time)
        {
            if(enemy.first_health<=0.0f)
            {
                break;
            }
            if(red_appear==original_red_appear)
            {
                red_appear = 0;
                int where = Random.Range(0, 4);
                if(where==0)
                {
                    Instantiate(homing_red, new Vector2(2.75f, Random.Range(-5.7f, -0.3f)), Quaternion.identity);
                }
                else if(where==1)
                {
                    Instantiate(homing_red, new Vector2(-2.75f, Random.Range(-5.7f, -0.3f)), Quaternion.identity);
                }
                else if(where==2)
                {
                    Instantiate(homing_red, new Vector2(Random.Range(-2.4f, 2.4f),-0.27f), Quaternion.identity);
                }
                else
                {
                    Instantiate(homing_red, new Vector2(Random.Range(-2.4f, 2.4f), -5.89f), Quaternion.identity);
                }
            }
            else
            {
                red_appear++;
                int where = Random.Range(0, 4);
                if (where == 0)
                {
                    Instantiate(homing_white, new Vector2(2.75f, Random.Range(-5.7f, -0.3f)), Quaternion.identity);
                }
                else if(where==1)
                {
                    Instantiate(homing_white, new Vector2(-2.75f, Random.Range(-5.7f, -0.3f)), Quaternion.identity);
                }
                 else if (where == 2)
                {
                    Instantiate(homing_white, new Vector2(Random.Range(-2.4f, 2.4f), -0.27f), Quaternion.identity);
                }
                else
                {
                    Instantiate(homing_white, new Vector2(Random.Range(-2.4f, 2.4f), -5.89f), Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(homing_respawn_time);
        }
        if(enemy.first_health>0.0f)
        {
            yield return new WaitForSeconds(1.20f);
        }
        if (enemy.first_health > 0.0f)
        {
            StartCoroutine("Spell2_start");
        }
    }

    IEnumerator Spell2_start()
    {
        yield return new WaitForSeconds(1.5f);
        float homing_respawn_time, original_respawn_time;
        int red_appear, original_red_appear, all_bullet;
        if (enemy.first_health >= original_first_health / 2)
        {
            homing_respawn_time = 0.40f;
            red_appear = 3;
            all_bullet = 40;
            speed = 80;
        }
        else
        {

            homing_respawn_time = 0.25f;
            red_appear = 5;
            all_bullet = 95;
            speed = 80;
        }
        original_respawn_time = homing_respawn_time;
        original_red_appear = red_appear;
        for(int i=1;i<=all_bullet;i++)
        {
            int num = Random.Range(0, 4);
            if(num == 0)
            {
                if(i%red_appear==0)
                {
                    GameObject bullet = (GameObject)Instantiate(red, new Vector2(3.03f, player.transform.position.y + Random.Range(-1.4f, 1.4f)), Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.left*speed );
                }
                else
                {
                    GameObject bullet = (GameObject)Instantiate(white, new Vector2(3.03f, player.transform.position.y + Random.Range(-1.4f, 1.4f)), Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed );
                }
            }
            else if(num==1)
            {
                if (i % red_appear == 0)
                {
                    GameObject bullet = (GameObject)Instantiate(red, new Vector2(-3.03f, player.transform.position.y + Random.Range(-1.4f, 1.4f)), Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed );
                }
                else
                {
                    GameObject bullet = (GameObject)Instantiate(white, new Vector2(-3.03f, player.transform.position.y + Random.Range(-1.4f, 1.4f)), Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed );
                }
            }
            else if(num==2)
            {
                if (i % red_appear == 0)
                {
                    GameObject bullet = (GameObject)Instantiate(red, new Vector2(player.transform.position.x + Random.Range(-1.4f, 1.4f), 0.4f), Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed );
                }
                else
                {
                    GameObject bullet = (GameObject)Instantiate(white, new Vector2(player.transform.position.x + Random.Range(-1.4f, 1.4f), 0.4f), Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed );
                }
            }
            else
            {
                if (i % red_appear == 0)
                {
                    GameObject bullet = (GameObject)Instantiate(red, new Vector2(player.transform.position.x + Random.Range(-1.4f, 1.4f), -6.0f), Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed );
                }
                else
                {
                    GameObject bullet = (GameObject)Instantiate(white, new Vector2(player.transform.position.x + Random.Range(-1.4f, 1.4f), -6.0f), Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed );
                }
            }
            sound_ef.Play_audio(bit_magic);
            yield return new WaitForSeconds(homing_respawn_time);
            if(enemy.first_health<=0.0f)
            {
                break;
            }
        }
        if (enemy.first_health > 0.0f)
        {
            yield return new WaitForSeconds(2.00f);
        }
        if (enemy.first_health > 0.0f)
        {
            StartCoroutine("Spell3_start");
        }
    }

    IEnumerator Spell3_start()
    {
        yield return new WaitForSeconds(1.5f);
        int bullet_number;
        float bullet_time;
        if (enemy.first_health >= original_first_health * 2 / 3)
        {
            bullet_time = 0.5f;
            bullet_number = 16;
        }
        else if (enemy.first_health >= original_first_health / 3)
        {
            bullet_time = 0.4f;
            bullet_number = 24;
        }
        else
        {
            bullet_time = 0.3f;
            bullet_number = 32;
        }
        for(int i=0;i<bullet_number;i++)
        {
            if(enemy.first_health<=0.0f)
            {
                break;
            }
            Instantiate(blue_energy, new Vector2(Random.Range(-3.31f, 3.52f), -0.5f), Quaternion.identity);
            yield return new WaitForSeconds(bullet_time);
        }
        if(enemy.first_health>0.0f)
        {
            hide = false;
            enemy.no_hit = false;
            this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            yield return new WaitForSeconds(1.5f);
        }
        if (enemy.first_health > 0.0f)
        {
            yield return new WaitForSeconds(1.5f);
        }
        //yield return new WaitForSeconds(3.0f);
        enemy.no_hit = true;
        this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        hide = true;
        if (enemy.first_health>0.0f)
        {
            StartCoroutine("Spell2_start");
        }
    }

    IEnumerator rage_spell()
    {
        hide = true;
        this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        enemy.no_hit = true;
        yield return new WaitForSeconds(1.0f);
        hide = false;
        this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        enemy.no_hit = false;
        Instantiate(rage_bullet, new Vector2(this.transform.position.x-2.0f, this.transform.position.y), Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        Instantiate(rage_bullet, new Vector2(this.transform.position.x + 2.0f, this.transform.position.y), Quaternion.identity);
        //Instantiate(rage_bullet, new Vector2(2.0f, -1.0f), Quaternion.identity);
        while (true)
        {
            enemy.no_hit = false;
            if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
            {
                break;
            }
            for (int i=0;i<=6;i++)
            {
                Instantiate(blue_energy, new Vector2(-3.3f + 1.00f*i, -0.5f), Quaternion.identity);
            }
            if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
            {
                break;
            }
            yield return new WaitForSeconds(1.20f);
        }
        
    }

    IEnumerator rage_text()
    {
        t_manager.sub_text.text = "나쁜놈... 각오하는게 좋을거다.";
        yield return new WaitForSeconds(2.50f);
        t_manager.sub_text.text = "";
    }

}
