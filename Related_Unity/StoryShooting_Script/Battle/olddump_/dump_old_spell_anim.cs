using UnityEngine;
using System.Collections;

public class dump_old_spell_anim : MonoBehaviour {

    public string battle_name;
    public GameObject bullet1,stone_respawn, stone_hole;
    public float start_time, choose_live_time, shoot_num;
    public bool sp_start, rage, attack;
    public Text_manager t_manager;
    public Animator anim;
    public Enemy enemy;
    public PlayerBattleController player;
    public AudioClip swing;
    public Sound_effect_loop sound_loop;
    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerBattleController>();
        enemy = FindObjectOfType<Enemy>();
        anim = GetComponent<Animator>();
        t_manager = FindObjectOfType<Text_manager>();
        battle_name = "dump_old_battle";
        enemy.battle_name = battle_name;
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
                StartCoroutine("Spell1_start");
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
        anim.SetBool("attack", attack);
        anim.SetBool("rage", rage);
        anim.SetBool("choose", enemy.choose_time);
        anim.SetBool("health_die", enemy.anim_health_die);
        anim.SetBool("choose_die", enemy.anim_choose_die);
        anim.SetBool("set", enemy.end_anim);
        if (enemy.end_anim)
        {
            if(PlayerPrefs.GetInt("bridge")!=2)
            {
                if(PlayerPrefs.GetInt("bridge")==1)
                {
                    PlayerPrefs.SetInt("repair_man_meet", 1);
                }
                else if(PlayerPrefs.GetInt("bridge") == 0)
                {
                    PlayerPrefs.SetInt("repair_man_meet", 0);
                }
                PlayerPrefs.SetInt("bridge", 2);
            }
        }
    }

    IEnumerator Spell1_start()
    {
        yield return new WaitForSeconds(1.0f);
        
        /*
        for(int loop=0;loop<=4;loop++)
        {
            if(loop%2==0)
            {
                for (int i = -60; i <= 60; i += 40)
                {
                    if (enemy.first_health > 0)
                    {
                        float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y + 3.886f, transform.position.x + 1.5f);
                        Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                        GameObject chong = (GameObject)Instantiate(stone_respawn, this.transform.position, Quaternion.identity);
                        chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 50f);
                    }

                }
            }
            else
            {
                if (enemy.first_health > 0)
                {
                    GameObject chong = (GameObject)Instantiate(stone_respawn, this.transform.position, Quaternion.identity);
                    Vector2 shoot_dir = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
                    shoot_dir.Normalize();
                    chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 50f);
                }
            }
            if(enemy.first_health>0)
            {
                yield return new WaitForSeconds(2.0f);
            }  
        }
       */
       if(enemy.first_health>0)
        {
            for(int loop=0;loop<=1;loop++)
            {
                if(enemy.first_health>0)
                {
                    for (int i = -20; i <= 20; i += 20)
                    {
                        float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                        Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                        GameObject chong = (GameObject)Instantiate(stone_respawn, this.transform.position, Quaternion.identity);
                        float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 + i;
                            chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 120f);
                            stone_horn_spawn res = chong.GetComponent<stone_horn_spawn>();
                            res.spawn_Time = 0.15f;
                    }
                    yield return new WaitForSeconds(2.0f);
                } 
            }
        }
        
        if(enemy.first_health>0)
        {
            StartCoroutine("Spell2_start");
        }
    }

    IEnumerator Spell2_start()
    {
        yield return new WaitForEndOfFrame();
        attack = true;
        for (int loop = 0;loop<=6;loop++)
        {
            if(enemy.first_health>0)
            {
                this.GetComponent<AudioSource>().PlayOneShot(swing);
                if(loop%2==0)
                {
                    for (int i = -80; i <= 80; i += 20)
                    {
                        if (enemy.first_health > 0)
                        {
                            float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y + 3.886f, transform.position.x + 1.5f);
                            Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                            GameObject chong = (GameObject)Instantiate(bullet1, new Vector2(this.transform.position.x + 0.5f,this.transform.position.y), Quaternion.identity);
                            float digree = Mathf.Atan2(chong.transform.position.y + 3.886f, chong.transform.position.x + 1.5f) * 180 / Mathf.PI - 90 + i;
                            chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 160f);
                            chong.transform.Rotate(0, 0, digree);
                        }

                    }
                }
                else
                {
                    for (int i = -70; i <= 90; i += 20)
                    {
                        if (enemy.first_health > 0)
                        {
                            float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y + 3.886f, transform.position.x + 1.5f);
                            Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                            GameObject chong = (GameObject)Instantiate(bullet1, new Vector2(this.transform.position.x + 0.5f, this.transform.position.y), Quaternion.identity);
                            float digree = Mathf.Atan2(chong.transform.position.y + 3.886f, chong.transform.position.x + 1.5f) * 180 / Mathf.PI - 90 + i;
                            chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 160f);
                            chong.transform.Rotate(0, 0, digree);
                        }

                    }
                }
                yield return new WaitForSeconds(1.4f);
            }
        }
        attack = false;
        if (enemy.first_health>0.0f)
        {
            StartCoroutine("Spell3_start");
        }
    }

    IEnumerator Spell3_start()
    {
        yield return new WaitForSeconds(1.5f);
        for(int loop = 0;loop<=2;loop++)
        {
            if(enemy.first_health>0.0f)
            {
                for(int num=0;num<=15;num++)
                {
                    Instantiate(stone_hole, new Vector2(Random.Range(-3.4f, 0.4f),player.transform.position.y+Random.Range(-1.5f,1.5f)), Quaternion.identity);
                }
                yield return new WaitForSeconds(2.4f);
            }
        }
        if(enemy.first_health>0.0f)
        {
            StartCoroutine("Spell2_start");
        }
    }

    IEnumerator rage_spell()
    {
        yield return new WaitForEndOfFrame();
        attack = true;
        sound_loop = FindObjectOfType<Sound_effect_loop>();
        sound_loop.Play_audio_loop(swing);
        while(true)
        {
            for (int loop=0;loop<=14;loop++)
            {
                int i = Random.Range(-30, 30);
                float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                GameObject chong = (GameObject)Instantiate(bullet1, new Vector2(this.transform.position.x + 0.5f, this.transform.position.y), Quaternion.identity);
                float digree = Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 + i;
                float shoot_speed = Random.Range(60, 100);
                chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * shoot_speed);
                chong.transform.Rotate(0, 0, digree);
                if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
                {
                    sound_loop.Stop();
                    break;
                }
                yield return new WaitForSeconds(0.18f);
            }
            for (int i = 0; i <= 0; i += 1)
            {
                if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
                {
                    break;
                }
                float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                GameObject chong = (GameObject)Instantiate(stone_respawn, this.transform.position, Quaternion.identity);
                float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 + i;
                chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 150f);
                stone_horn_spawn res = chong.GetComponent<stone_horn_spawn>();
                res.spawn_Time = 0.14f;
            }
            if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
            {
                break;
            }

        }
        attack = false;
    }

    IEnumerator rage_text()
    {
        if(PlayerPrefs.GetInt("kill")!=3)
        {
            t_manager.sub_text.text = "할아범!!! 지금부터가 진짜야!!!";
        }
        else
        {
            t_manager.sub_text.text = "하하....하.... 제발 내게서 떨어져버리라고!!!";
        }
        
        yield return new WaitForSeconds(2.50f);
        t_manager.sub_text.text = "";
    }
}
