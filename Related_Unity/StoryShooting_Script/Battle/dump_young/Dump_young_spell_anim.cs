using UnityEngine;
using System.Collections;

public class Dump_young_spell_anim : MonoBehaviour {

    public string battle_name;
    public GameObject bullet1, tong_namu_zone , wood_wall;
    public float start_time, choose_live_time;
    public bool sp_start, rage, attack;
    public Text_manager t_manager;
    public Animator anim;
    public Enemy enemy;
    public PlayerBattleController player;
    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerBattleController>();
        enemy = FindObjectOfType<Enemy>();
        anim = GetComponent<Animator>();
        t_manager = FindObjectOfType<Text_manager>();
        battle_name = "dump_young_battle";
        enemy.battle_name = battle_name;
        choose_live_time = 0.0f;
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
                StartCoroutine("Spell2_start");
                //StartCoroutine("Spell1_start", 1f);
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
        anim.SetBool("health_die", enemy.anim_health_die);
        anim.SetBool("health_die_dying", enemy.end_anim);
        anim.SetBool("choose", enemy.choose_time);
        anim.SetBool("choose_die", enemy.anim_choose_die);
        if(enemy.anim_choose_live)
        {
            choose_live_time += Time.deltaTime;
            if(choose_live_time>=2.5f)
            {
                anim.SetBool("choose_live", enemy.anim_choose_live);
            }
        }
        anim.SetBool("choose_set", enemy.end_anim);
    }

    IEnumerator Spell1_start(float time)
    {
        yield return new WaitForSeconds(1f);
            if(enemy.first_health>0)
            {
                attack = true;
                for (int i = -75; i <= 75; i += 8)
                {
                    if (enemy.first_health <= 0.0f)
                    {
                         break;
                     }
                    float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y, transform.position.x);
                    Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                    GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                    float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 + i;
                    chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 150f);
                    chong.transform.Rotate(0, 0, digree);
                    yield return new WaitForSeconds(0.075f);
                }
                for (int i = 75; i >= -75; i -= 8)
                {
                    if (enemy.first_health <= 0.0f)
                    {
                        break;
                    }
                    float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y , transform.position.x);
                    Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                    GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                    float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 + i;
                    chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 150f);
                    chong.transform.Rotate(0, 0, digree);
                    yield return new WaitForSeconds(0.075f);
                }
            for (int i = -95; i <= 78; i += 12)
            {
                if (enemy.first_health <= 0.0f)
                {
                    break;
                }
                float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y, transform.position.x);
                Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 + i;
                chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 150f);
                chong.transform.Rotate(0, 0, digree);
                yield return new WaitForSeconds(0.075f);
            }
            for (int i = 78; i >= -95; i -= 12)
            {
                if (enemy.first_health <= 0.0f)
                {
                    break;
                }
                float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y, transform.position.x);
                Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 + i;
                chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 150f);
                chong.transform.Rotate(0, 0, digree);
                yield return new WaitForSeconds(0.075f);
            }
        }
        attack = false;
        if(enemy.first_health>0)
        {
            int choose = Random.Range(0, 3);
            if(choose<=1)
            {
                StartCoroutine("Spell2_start");
            }
            else
            {
                StartCoroutine("Spell3_start");
            }
        }
    }

    IEnumerator Spell2_start()
    {
        yield return new WaitForSeconds(1.0f);
        attack = true;
        int not_same=-1;
        if(enemy.first_health>0.0f)
        {
            for (float i = 2.35f; i >= -2.35f; i -= 0.35f)
            {
                GameObject wall = (GameObject)Instantiate(wood_wall, new Vector2(2.00f, i), Quaternion.identity);
            }
            yield return new WaitForSeconds(2.5f);
            for(int i=0;i<=14;i++)
            {
                int j = Random.Range(0, 5);
                while(not_same==j)
                {
                    j = Random.Range(0, 5);
                }
                not_same = j;
                GameObject danger = (GameObject)Instantiate(tong_namu_zone, new Vector2(-3.75f + 1.4f * j, 2.3f), Quaternion.identity);
                yield return new WaitForSeconds(0.75f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        attack = false;
        if(enemy.first_health>0.0f)
        {
            StartCoroutine("Spell4_start");
        }
    }

    IEnumerator Spell3_start()
    {
        yield return new WaitForSeconds(0.5f);
        attack = true;
        for(int loop=0;loop <=12;loop++)
        {
            GameObject wall = (GameObject)Instantiate(wood_wall,new Vector3(Random.Range(-3.5f,3.00f),Random.Range(-2.35f,2.35f)), Quaternion.identity);
            Shield ex = wall.GetComponent<Shield>();
            ex.time = 12;
        }
        yield return new WaitForSeconds(2.5f);
        for(int loop=0;loop<=10;loop++)
        {
            if (enemy.first_health > 0.0f)
            {
                if(loop%2==0)
                {
                    if(player.blink_chance>=1)
                    {
                        int key_num = Random.Range(0, 4);
                        for (int namu_loop = 0; namu_loop <= 4; namu_loop++)
                        {
                            if(key_num!=namu_loop)
                            {
                                GameObject danger = (GameObject)Instantiate(tong_namu_zone, new Vector2(-3.75f + 1.4f * namu_loop, 2.3f), Quaternion.identity);
                            }
                        }
                    }
                    else
                    {
                        for (int i = -60; i <= 60; i += 12)
                        {
                            float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                            Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                            GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                            float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 + i;
                            chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 120f);
                            chong.transform.Rotate(0, 0, digree);
                        }
                    }
                }
                yield return new WaitForSeconds(0.8f);
                if(enemy.first_health<=0.0f)
                {
                    break;
                }
            }
        }
        attack = false;
        if(enemy.first_health>0.0f)
        {
                StartCoroutine("Spell2_start");
        }
    }

    IEnumerator Spell4_start()
    {
        attack = true;
        for(int loop=0;loop<=4;loop++)
        {
            float pos_x = player.transform.position.x;
            if (enemy.first_health > 0.0f)
            {
                for (float k = -0.8f; k <= 0.8f; k += 1.6f)
                {
                    for (float i = 2.35f; i >= -2.35f; i -= 0.35f)
                    {
                        GameObject wall = (GameObject)Instantiate(wood_wall, new Vector2(k + player.transform.position.x, i), Quaternion.identity);
                        Shield wall_shield = wall.GetComponent<Shield>();
                        wall_shield.time = 5f;
                        wall_shield.health = 8;
                    }
                }
                yield return new WaitForSeconds(1.5f);
            }
            if (enemy.first_health > 0.0f)
            {
                GameObject dangerzone = (GameObject)Instantiate(tong_namu_zone, new Vector2(pos_x, 2.3f), Quaternion.identity);
                yield return new WaitForSeconds(2.0f);
            }
        }
        attack = false;
        if(enemy.first_health>0.0f)
        {
            StartCoroutine("Spell3_start");
        }
    }

    IEnumerator rage_spell()
    {
        yield return new WaitForSeconds(3.0f);
        attack = true;
        int not_same = -1;
        while(true)
        {
            if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
            {
                break;
            }
            for (float i = 2.35f; i >= -2.35f; i -= 0.35f)
            {
                GameObject wall = (GameObject)Instantiate(wood_wall, new Vector2(2.00f, i), Quaternion.identity);
                Shield wood = wall.GetComponent<Shield>();
                wood.health = 12;
            }
            yield return new WaitForSeconds(3.0f);
            for(int a=0;a<=18;a++)
            {
                if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
                {
                    break;
                }
                int j = Random.Range(0, 5);
                while (not_same == j)
                {
                    j = Random.Range(0, 5);
                }
                not_same = j;
                GameObject danger = (GameObject)Instantiate(tong_namu_zone, new Vector2(-3.75f + 1.4f * j, 2.3f), Quaternion.identity);
                if(a%4==0)
                {
                    for (int i = -45; i <= 45; i += 15)
                    {
                        float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
                        Vector2 shoot_dir = new Vector2(Mathf.Cos(angle + 180 * Mathf.Deg2Rad), Mathf.Sin(angle + 180 * Mathf.Deg2Rad));
                        GameObject chong = (GameObject)Instantiate(bullet1, this.transform.position, Quaternion.identity);
                        float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) * 180 / Mathf.PI - 90 + i;
                        chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir * 120f);
                        chong.transform.Rotate(0, 0, digree);
                    }
                }
                yield return new WaitForSeconds(0.70f);
            }
            yield return new WaitForSeconds(2.5f);
            if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
            {
                break;
            }
            for (int loop = 0; loop <= 12; loop++)
            {
                GameObject wall = (GameObject)Instantiate(wood_wall, new Vector3(Random.Range(-3.5f, 3.00f), Random.Range(-2.35f, 2.35f)), Quaternion.identity);
            }
            yield return new WaitForSeconds(2.0f);
            for(int loop=0;loop<=4;loop++)
            {
                if (enemy.second_health <= 0.0f || enemy.rage_time <= 0.0f)
                {
                    break;
                }
                int key_num = Random.Range(0, 4);
                for (int namu_loop = 0; namu_loop <= 4; namu_loop++)
                {
                    if(namu_loop!=key_num)
                    {
                        GameObject danger = (GameObject)Instantiate(tong_namu_zone, new Vector2(-3.75f + 1.4f * namu_loop, 2.3f), Quaternion.identity);
                        yield return new WaitForSeconds(0.2f);
                    } 
                }
                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(2.5f);
        }
    }

    IEnumerator rage_text()
    {
        t_manager.sub_text.text = "방해꾼... 없앤다.... 방해... 없애?";
        yield return new WaitForSeconds(2.50f);
        t_manager.sub_text.text = "";
    }
}
