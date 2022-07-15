using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina_UI : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[3];
    public int stamina_standard;
    // Start is called before the first frame update
    void Start()
    {
        stamina_set();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void stamina_set()
    {
        int stamina = Player_Manager.player_manager.stamin_return();
        int limit = Player_Manager.player_manager.return_limit_stamina();
        if (stamina_standard <= stamina)
        {
            this.GetComponent<Image>().sprite = sprites[0];
        }
        else if (stamina_standard > stamina && stamina_standard <= limit)
        {
            this.GetComponent<Image>().sprite = sprites[1];
        }
        else if (stamina_standard > limit)
        {
            this.GetComponent<Image>().sprite = sprites[2];
        }
    }
}
