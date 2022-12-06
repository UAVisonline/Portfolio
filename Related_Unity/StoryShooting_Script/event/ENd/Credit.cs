using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Credit : MonoBehaviour {

    public Text end_Credit;
    public bool music, end;
    public AudioClip ending_BGM;
    public float credit_time, alpha_time, credit_end_pos, go_to_Main_menu, end_credit_alpha, Main_menu;
    private float original_alpha_time, credit_original_alpha;
	// Use this for initialization
	void Start () {
        original_alpha_time = alpha_time;
        credit_original_alpha = end_credit_alpha;

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Screen.height);
	if(credit_time>=0.0f)
        {
            credit_time -= Time.deltaTime;
        }
    else if(credit_time<0.0f)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha_time / original_alpha_time);
            if(alpha_time>=0.0f)
            {
                alpha_time -= Time.deltaTime;
            } 
        }
    if(alpha_time<0.0f)
        {
            if(!music)
            {
                Bgm_manager bg = FindObjectOfType<Bgm_manager>();
                bg.music_change(ending_BGM);
                music = true;
            }
            if(end_Credit.gameObject.GetComponent<RectTransform>().localPosition.y <= credit_end_pos)
            {
                if (Input.GetKey(KeyCode.Return))
                {
                    //if(Screen.height)
                    end_Credit.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, end_Credit.gameObject.GetComponent<RectTransform>().localPosition.y + Time.deltaTime * Screen.height / 5, 0f);
                }
                else
                {
                    end_Credit.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, end_Credit.gameObject.GetComponent<RectTransform>().localPosition.y + Time.deltaTime * Screen.height / 10, 0f);
                }
            }
            else
            {
                if(go_to_Main_menu>=0.0f)
                {
                    go_to_Main_menu -= Time.deltaTime;
                }
                else
                {
                    end_Credit.gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f,end_credit_alpha/credit_original_alpha);
                    end_credit_alpha -= Time.deltaTime;
                    if(end_credit_alpha<=0.0f)
                    {
                        if(!end)
                        {
                            end = true;
                            Bgm_manager bg = FindObjectOfType<Bgm_manager>();
                            bg.volume_to_down();
                            PlayerPrefs.DeleteAll();
                        }
                       if(Main_menu>0.0f)
                        {
                            Main_menu -= Time.deltaTime;
                        }
                        else
                        {
                            SceneManager.LoadScene("Main_Screen");
                        }
                    }
                }
            }
 
        }
	}
}
