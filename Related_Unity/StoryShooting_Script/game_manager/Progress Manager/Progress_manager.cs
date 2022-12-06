using UnityEngine;
using System.Collections;

public class Progress_manager : MonoBehaviour {

    //PlayerPrefs.GetInt("game_save_on"); 1이면 로드가능, 없거나 0이면 로드 불가능
    public static bool exist;
	// Use this for initialization
	void Start () {
        if(!exist)
        {
            exist = true;
            DontDestroyOnLoad(this);

        }
        else if(exist)
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Bool_reset(bool load, int reset)
    {
        if(reset == 0)
        {
            load = false;
        }
        else if(reset == 1)
        {
            load = true;
        }
    }

    public bool Bool_Load(bool load)
    {

        return load;
    }
}
