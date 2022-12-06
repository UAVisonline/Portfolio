using UnityEngine;
using System.Collections;

public class Puzzle_manager : MonoBehaviour {

    public bool cave_puzzle_1;
    public static bool exist;
    // Use this for initialization
    void Start () {
        if (!exist)
        {
            exist = true;
            DontDestroyOnLoad(this);

        }
        else if (exist)
        {
            Destroy(gameObject);
        }

        //cave_aside_1 에서 퍼즐을 해결함
        if (PlayerPrefs.HasKey("cave_puzzle_1"))//퍼즐을 한번도 플레이 하지 않았을경우
        {
            if (PlayerPrefs.GetInt("cave_puzzle_1") == 0)//퍼즐을 실패했을경우
            {
                cave_puzzle_1 = false;
            }
            else//한번이라도 퍼즐을 성공시켰을경우
            {
                cave_puzzle_1 = true;
            }
            //Bool_reset(cave_puzzle_1, PlayerPrefs.GetInt("cave_puzzle_1"));
        }
    }
	
	// Update is called once per frame
	void Update () {
    }
}
