using UnityEngine;
using System.Collections;

public class Reaper_shoot_player : MonoBehaviour {

    public PlayerBattleController player;
    public GameObject bullet;
    private float start_time;
    private bool start;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerBattleController>();
        start_time = 2.00f;
	}
	
	// Update is called once per frame
	void Update () {
	if(start_time>=0.0f)
        {
            start_time -= Time.deltaTime;
        }
        else
        {
            if(!start)
            {
                start = true;
                StartCoroutine("shoot");
            }
        }
	}

    IEnumerator shoot()
    {
        //Vector2 dir = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        //dir.Normalize();
        for (int i = -30;i<=30;i+=30)
        {
            float angle = i * Mathf.Deg2Rad + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
            //Vector2 shoot_dir = dir.normalized ;
            Vector2 shoot_dir = new Vector2(Mathf.Cos(angle+180*Mathf.Deg2Rad), Mathf.Sin(angle+180*Mathf.Deg2Rad)) ;
            //shoot_dir.Normalize();
            GameObject chong = (GameObject)Instantiate(bullet, this.transform.position, Quaternion.identity);
            float digree = Mathf.Atan2(chong.transform.position.y - player.transform.position.y, chong.transform.position.x - player.transform.position.x) *180/Mathf.PI - 90 +i;
            chong.GetComponent<Rigidbody2D>().AddForce(shoot_dir  * 160f);
            chong.transform.Rotate(0, 0, digree);
        }
        yield return null;
         
    }
}
