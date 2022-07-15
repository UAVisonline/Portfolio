using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack_bullet_script : MonoBehaviour
{
    private bool left;
    private Rigidbody2D rb;

    public GameObject bullet_zone; // 적 피격 시 이펙트
    public float speed, limit_delete_time;
    private int effect_valuable; // 이펙트 좌우 반전 관련
    
    void Start()
    {
        if(GameObject.Find("Player").GetComponent<Transform>().localScale.x >=0.0f) // 플레이어 컨트롤러를 쓰면 되지 않나???
        {
            left = false;
            effect_valuable = 1;
        }
        else if(GameObject.Find("Player").GetComponent<Transform>().localScale.x < 0.0f)
        {
            left = true;
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            effect_valuable = -1;
        }
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(!left)
        {
            rb.velocity = new Vector3(speed*Time.deltaTime, 0f, 0f);
        }
        else
        {
            rb.velocity = new Vector3(-speed * Time.deltaTime, 0f, 0f);
        }
    }

    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (pos.x < 0f || pos.x > 1f) this.gameObject.SetActive(false);
        if (pos.y < 0f || pos.y > 1f) this.gameObject.SetActive(false);
    }

    /*
     * private void OnCollisionEnter2D(Collision2D collision) // 어차피 총알이 Trigger라서 필요없음
    {
        if (collision.gameObject.tag == "Enemy")
        {
            
            if (collision.gameObject.GetComponent<Enemy>().return_hp() > 0)
            {
                collision.gameObject.GetComponent<Enemy>().Damaged_bullet(effect_valuable, 1, this.transform.position);
                Instantiate(bullet_zone, this.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);
            }

        }
        else if(collision.gameObject.tag == "Ground")
        {
            GameObject effect = Effect_Manager.effect_manager.Get_bullet_effect("no_effect");
            if (effect_valuable == 1)
            {
                effect.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (effect_valuable == -1)
            {
                effect.GetComponent<SpriteRenderer>().flipX = true;
            }
            Instantiate(effect, this.transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.GetComponent<Enemy>().return_hp()>0)
            {
                collision.gameObject.GetComponent<Enemy>().Damaged_bullet(effect_valuable, 1,this.transform.position);
                Instantiate(bullet_zone, this.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);
            }
            
        }

        else if (collision.gameObject.tag == "Ground")
        {
            GameObject effect = Effect_Manager.effect_manager.Get_bullet_effect("no_effect");  // 총알 피격 이펙트 새로 생성 (그냥 위 변수로 설정해 연결하면 되지 않나?)
            if (effect_valuable==1)
            {
                effect.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (effect_valuable==-1)
            {
                effect.GetComponent<SpriteRenderer>().flipX = true;
            }
            Instantiate(effect, this.transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
}
