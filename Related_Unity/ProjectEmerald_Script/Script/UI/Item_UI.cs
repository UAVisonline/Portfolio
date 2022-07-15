using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_UI : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[2];
    // Start is called before the first frame update
    void Start()
    {
        Item_set(Player_Manager.player_manager.frecuency_return().ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Item_set(string str)
    {
        this.GetComponent<Text>().text = str;
        int frecuency = int.Parse(this.GetComponent<Text>().text);
        if(frecuency==0)
        {
            GameObject.Find("Item").GetComponent<Image>().sprite = sprites[1];
        }
        else
        {
            GameObject.Find("Item").GetComponent<Image>().sprite = sprites[0];
        }
    }
}
