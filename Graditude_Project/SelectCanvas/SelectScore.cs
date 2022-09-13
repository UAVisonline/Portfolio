using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SelectScore : MonoBehaviour
{
    [SerializeField] private Album_Select select_obj;

    [SerializeField] private TextMeshProUGUI acr_text;
    [SerializeField] private TextMeshProUGUI perfect_text;
    [SerializeField] private TextMeshProUGUI good_text;
    [SerializeField] private TextMeshProUGUI miss_text;
    [SerializeField] private TextMeshProUGUI combo_text;

    private void OnEnable()
    {
        localscore_visualiize();
    }

    public void localscore_visualiize()
    {
        acr_text.text = PlayerPrefs.GetFloat(select_obj.get_playerpref_string() + "_acr").ToString() + "%";
        perfect_text.text = "Perfect " + PlayerPrefs.GetInt(select_obj.get_playerpref_string() + "_perfect").ToString();
        good_text.text = "Good " + PlayerPrefs.GetInt(select_obj.get_playerpref_string() + "_good").ToString();
        miss_text.text = "Miss " + PlayerPrefs.GetInt(select_obj.get_playerpref_string() + "_miss").ToString();
        combo_text.text = PlayerPrefs.GetInt(select_obj.get_playerpref_string() + "_current_combo").ToString() + "/" + PlayerPrefs.GetInt(select_obj.get_playerpref_string() + "_total_combo").ToString();
    }
}
