using UnityEngine;
using System.Collections;

public class Cave_puzzle1_manager : MonoBehaviour {

    public int[] lever = new int[4];
    public int[] ans = { 1, 2, 3, 4 };
    public bool puzzle;
    public int where_put;
    public Text_manager t_manager;
    public TextAsset puzzle_clear, puzzle_not_clear;
	// Use this for initialization
	void Start () {
        //Debug.Log(Application.loadedLevelName);
        puzzle = false;
        where_put = 0;
        if(!PlayerPrefs.HasKey("cave_puzzle_1"))//퍼즐을 지금 처음 발견했을 경우
        {
            PlayerPrefs.SetInt("cave_puzzle_1", 0);//처음 퍼즐과 조우시 false값을 주도록 함
        }
        if(PlayerPrefs.HasKey("cave_puzzle_1"))
        {
            if(PlayerPrefs.GetInt("cave_puzzle_1")==0)
            {
                puzzle = false;
            }
            if(PlayerPrefs.GetInt("cave_puzzle_1") == 1)
            {
                puzzle = true;
            }
        }
        if(puzzle)
        {
            where_put = 5;
        }
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.SetActive(!puzzle);
        if(where_put==4)
        {
            if(t_manager == null)
            {
                t_manager = FindObjectOfType<Text_manager>();
            }
            StartCoroutine(lever_reset(0.1f));
        }
	}

    IEnumerator lever_reset(float time)
    {
        yield return new WaitForSeconds(time);
        for(int i = 0; i<=3;i++)
        {
            if(lever[i] != ans[i])
            {
                for(int j =0; j<=3;j++)
                {
                    lever[j] = 0;
                }
                text_call(puzzle_not_clear);
                break;
            }
            if(i==3)
            {
                where_put = 5;
                puzzle = true;
                PlayerPrefs.SetInt("cave_puzzle_1", 1);//data save start//퍼즐을 해결한 걸로 쳐버림. 다시 이 퍼즐과 조우해도 더이상 퍼즐을 풀지 않아도 됨.
                PlayerPrefs.Save();//데이터를 세이브함
                text_call(puzzle_clear);
                yield return null;
            }
        }
        where_put = 0;
    }

    public void text_call(TextAsset txt)
    {
        t_manager.text_enable(txt);
    }
}
