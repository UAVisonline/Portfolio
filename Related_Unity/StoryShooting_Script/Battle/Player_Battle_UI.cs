using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player_Battle_UI : MonoBehaviour {

    public PlayerBattleController player;
    public Image  health, blink ;
    public Sprite[] health_image;
    public Sprite[] blink_image;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerBattleController>();
	}
	
	// Update is called once per frame
	void Update () {
	if(player == null)
        {
            player = FindObjectOfType<PlayerBattleController>();
        }
    if(player.player_health >= 0)
        {
            health.sprite = health_image[player.player_health];
        }
        blink.sprite = blink_image[player.blink_chance];
    }
}
