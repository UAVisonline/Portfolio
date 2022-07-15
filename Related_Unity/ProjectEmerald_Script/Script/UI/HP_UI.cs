using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_UI : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[3];
    public int hp_standard;
    // Start is called before the first frame update
    void Start()
    {
        hp_set();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hp_set()
    {
        int hp = Player_Manager.player_manager.hp_return();
        int limit = Player_Manager.player_manager.return_limit_hp();
        if(hp_standard<=hp)
        {
            this.GetComponent<Image>().sprite = sprites[0];
        }
        else if(hp_standard>hp && hp_standard<=limit)
        {
            this.GetComponent<Image>().sprite = sprites[1];
        }
        else if(hp_standard>limit)
        {
            this.GetComponent<Image>().sprite = sprites[2];
        }
    }
}
