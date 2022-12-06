using UnityEngine;
using System.Collections;

public class Reaper_spell_anim : MonoBehaviour {

    public string battle_name;
    public GameObject bullet1, bullet2, bullet3, gate_360, gate_4 ,spell_4;
    public float start_time, spell_time;
    public bool sp_start, rage;
    public Text_manager t_manager;
    public Animator anim;
    public Enemy enemy;
    public PlayerBattleController player;
    public bool left, right;
    private Vector2 position;
    public AudioClip bullet_sound;
    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerBattleController>();
        enemy = FindObjectOfType<Enemy>();
        anim = GetComponent<Animator>();
        t_manager = FindObjectOfType<Text_manager>();
        battle_name = "reaper_battle";
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
                StartCoroutine("Spell1_start", 1f);
            }
        }
        if(enemy.first_health<=0.0f)
        {
            if(!rage)
            {
                StartCoroutine("rage_text");
                StartCoroutine("rage_spell");
                rage = true;
            }
            transform.position = Vector3.Lerp(transform.position, position, 2.5f * Time.deltaTime);
        }
        anim.SetFloat("Rage", enemy.first_health);
        anim.SetFloat("Second_health", enemy.second_health);
        anim.SetBool("choose_time", enemy.choose_time);
        anim.SetBool("choose_die", enemy.anim_choose_die);
        anim.SetBool("die", enemy.end_anim);
        anim.SetBool("end", enemy.end_anim);
        anim.SetBool("Left", left);
        anim.SetBool("Right", right);
    }

    IEnumerator Spell1_start(float time)
    {
        yield return new WaitForSeconds(time);
        if(enemy.first_health>0.0f)
        {
            if(player.transform.position.x<=3.5f && player.transform.position.x>=-3.5f && player.transform.position.y<=2.0f && player.transform.position.y >= -4.0f)
            {
                GameObject obj_2 = (GameObject)Instantiate(gate_360, player.transform.position + new Vector3(-1f, -1f, 0f), Quaternion.identity);
                GameObject obj_3 = (GameObject)Instantiate(gate_360, player.transform.position + new Vector3(1f, 1f, 0f), Quaternion.identity);
                //GameObject obj_4 = (GameObject)Instantiate(gate_360, player.transform.position + new Vector3(1f, -1f, 0f), Quaternion.identity);
                yield return new WaitForSeconds(2.5f);
                for (int i = -30; i <= 30; i += 30)
                {
                    float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                    Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                    GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                    float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 +i;
                    chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 190f);
                    chong.transform.Rotate(0, 0, digree);
                }
            }
            else
            {
                int j = 6;
                while(j>0)
                {
                    if(j%3==0)
                    {
                        for (int i = -60; i <= 60; i += 30)
                        {
                            float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                            Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                            GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                            float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 +i;
                            chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 120f);
                            chong.transform.Rotate(0, 0, digree);
                        }
                    }
                    else
                    {
                        for (int i = -30; i <= 30; i += 30)
                        {
                            float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                            Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                            GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                            float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90+i;
                            chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 240f);
                            chong.transform.Rotate(0, 0, digree);
                        }
                    }
                    j--;
                    yield return new WaitForSeconds(0.8f);
                }
            }
        }
        if(enemy.first_health>0.0f)
        {
            if(player.transform.position.y<=-4.0f)
            {
                StartCoroutine("Spell2_start", 1.0f);
            }
            else
            {
                int i = Random.Range(1, 5);

                if(i%2==0)
                {
                    StartCoroutine("Spell3_start", 1f);
                }
                else
                {   
                    StartCoroutine("Spell4_start", 2f);  
                }
            }
        }
    }

    IEnumerator Spell2_start(float time)
    {
        yield return new WaitForSeconds(time);
        if(enemy.first_health>0.0f)
        {
            for (int k = 0; k <= 4; k++)
            {
                if(k%2==0)
                {
                    for (int i = -60; i <= 60; i += 30)
                    {
                        float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                        Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                        GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                        float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 +i;
                        chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 180f);
                        chong.transform.Rotate(0, 0, digree);
                    }
                    yield return new WaitForSeconds(0.8f);
                }
                else
                {
                    for (int i = -45; i <= 45; i += 45)
                    {
                        float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                        Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                        GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                        float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 +i;
                        chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 240f);
                        chong.transform.Rotate(0, 0, digree);
                    }
                    yield return new WaitForSeconds(0.5f);
                }
            }
            int next = Random.Range(1, 4);
            Debug.Log(next);
            if(next==1)
            {
                yield return new WaitForSeconds(1.3f);
                StartCoroutine("Spell2_start", 0.8f);
            }
            else if(next==2)
            {
                StartCoroutine("Spell4_start", 1f);
            }
            else
            {
                StartCoroutine("Spell1_start",0.5f);
            }
        }
    }

    IEnumerator Spell3_start(float time)
    {
        yield return new WaitForSeconds(0.8f);
        if(enemy.first_health>0.0f)
        {
            for (int i = 0; i <= 2; i++)
            {
                GameObject obj = (GameObject)Instantiate(gate_4, new Vector2(Random.Range(-3.5f, 3.5f), Random.Range(-5.4f, 2f)), Quaternion.identity);
                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForSeconds(3.0f);
                for (int j = 0; j <= 2; j++)
                {
                    for (int i = -45; i <= 45; i += 45)
                    {
                        float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                        Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                        GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                        float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90  +i;
                        chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 150f);
                        chong.transform.Rotate(0, 0, digree);
                    }
                    yield return new WaitForSeconds(0.45f);
                }
            int k = Random.Range(1, 3);
            Debug.Log(k);
            if(k==1)
            {
                StartCoroutine("Spell1_start", 0.5f);
            }
            else
            {
                StartCoroutine("Spell2_start", 0.5f);
            }
        }
    }

    IEnumerator Spell4_start(float time)
    {
        yield return new WaitForSeconds(0.6f);
        for(int i=0;i<=2;i++)
        {
            if(enemy.first_health>0.0f)
            {
                GameObject obj = (GameObject)Instantiate(spell_4, new Vector2(0f, 2.5f), Quaternion.identity);
                yield return new WaitForSeconds(2.0f);
            } 
        }
        yield return new WaitForSeconds(1.5f);
        if(enemy.first_health>0.0f)
        {
            for(int i=0;i<=25;i++)
            {
                GameObject obj = (GameObject)Instantiate(bullet3, this.transform.position, Quaternion.identity);
                obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(150f * Mathf.Cos(Mathf.PI * 2 * i / 25), 150f * Mathf.Sin(Mathf.PI * 2 * i / 25)));
                obj.transform.Rotate(new Vector3(0f, 0f, 360 * i / 25 - 270.0f));
            }
            this.GetComponent<AudioSource>().PlayOneShot(bullet_sound);
            if (Random.Range(1,4)!=1)
            {
                StartCoroutine("Spell3_start", 1f);
            }
            else
            {
                StartCoroutine("Spell2_start", 0.75f);
            }
        }
    }

    IEnumerator rage_spell()
    {
        yield return new WaitForSeconds(2.00f);
        while(true)
        {
            if(enemy.second_health<=0.0f || enemy.rage_time<=0.0f)
            {
                break;
            }
            if(enemy.second_health>0.0f && enemy.rage_time>0.0f)
            {
                if(transform.position.x>=0)
                {
                    position = new Vector2(Random.Range(-2.5f, -1.0f), Random.Range(-2.2f, 0.2f));
                }
                else
                {
                    position = new Vector2(Random.Range(1.0f, 2.5f), Random.Range(-2.2f, 0.2f));
                }
                if(position.x>this.transform.position.x)
                {
                    right = true;
                    left = false;
                }
                else if(position.x < this.transform.position.x)
                {
                    left = true;
                    right = false;
                }
                yield return new WaitForSeconds(1.2f);
                if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
                {
                    break;
                }
                for (int i = 0; i <= 36; i++)
                {
                    GameObject obj = (GameObject)Instantiate(bullet3, this.transform.position, Quaternion.identity);
                    obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(100f * Mathf.Cos(Mathf.PI * 2 * i / 36), 100f * Mathf.Sin(Mathf.PI * 2 * i / 36)));
                    obj.transform.Rotate(new Vector3(0f, 0f, 360 * i / 36 - 270.0f));
                }
                this.GetComponent<AudioSource>().PlayOneShot(bullet_sound);
                yield return new WaitForSeconds(2.4f);
                GameObject gate = (GameObject)Instantiate(gate_360, this.transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator rage_text()
    {
        t_manager.sub_text.text = "이제부터가 진짜다! 테일로스!";
        yield return new WaitForSeconds(2.50f);
        t_manager.sub_text.text = "";
    }

}
