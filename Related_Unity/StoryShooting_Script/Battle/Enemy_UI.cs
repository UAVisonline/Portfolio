using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy_UI : MonoBehaviour {

    public Enemy enemy;
    public Text time_text;
    public Image first_health, second_health, save, kill;
	// Use this for initialization
	void Start () {
        enemy = FindObjectOfType<Enemy>();
    }
	
	// Update is called once per frame
	void Update () {
        if(enemy==null)
        {
            enemy = FindObjectOfType<Enemy>();
        }
        first_health.fillAmount = enemy.first_health_return();
        second_health.fillAmount = enemy.second_health_return();
        if(enemy.first_health <= 0)
        {
            if(enemy.rage_time >= 0.0f)
            {
                int time = (int)enemy.rage_time;
                time_text.text = "Rage Time : " + time.ToString();
            }
            else
            {
                time_text.text = "Rage Time Over";
            }
        }
        if(enemy.choose_situation)
        {
            save.color = new Color(255, 255, 255, 255);
            kill.color = new Color(255, 255, 255, 255);
        }
        else
        {
            save.color = new Color(255, 255, 255, 0);
            kill.color = new Color(255, 255, 255, 0);
        }
    }
}
