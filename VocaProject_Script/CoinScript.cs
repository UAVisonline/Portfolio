using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coin_text; // Player Coin이 얼마나 있는지 보여줌

    private void OnEnable()
    {
        coin_text_init(); // Text 초기화
    }

    public void coin_text_init()
    {
        coin_text.text = ShopManager.shopmanager.get_coin().ToString();
        //Debug.Log(VocaMaster.vocaMaster.get_coin().ToString());
    }
}
