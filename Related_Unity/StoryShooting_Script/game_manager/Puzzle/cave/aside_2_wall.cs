using UnityEngine;
using System.Collections;

public class aside_2_wall : MonoBehaviour {

    public bool[] answer = new bool[6];
    public bool[] correct = { true, false, true, true, false, false };
    public Text_manager t_manager;
    public TextAsset solve;
	// Use this for initialization
	void Start () {
	if(PlayerPrefs.GetInt("cave_puzzle_2")>=1)
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(t_manager == null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
	}

    public void checking()
    {
        if(PlayerPrefs.GetInt("cave_puzzle_2")==0)
        {
            for (int i = 0; i < 6; i++)
            {
                if (answer[i] != correct[i])
                {
                    break;
                }
                if (i == 5)
                {
                    if (PlayerPrefs.GetInt("cave_puzzle_2") == 0)
                    {
                        PlayerPrefs.SetInt("cave_puzzle_2", 1);
                        gameObject.SetActive(false);
                        t_manager.text_enable(solve);
                    }
                }
            }
        } 
    }
}
