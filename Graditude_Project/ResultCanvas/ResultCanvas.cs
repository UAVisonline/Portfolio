using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using TMPro;


public class ResultCanvas : MonoBehaviour
{
    [BoxGroup("UI member")] [SerializeField] private TextMeshProUGUI song_title_text;
    [BoxGroup("UI member")] [SerializeField] private TextMeshProUGUI song_level_text;
    [BoxGroup("UI member")] [SerializeField] private TextMeshProUGUI combo_text;
    [BoxGroup("UI member")] [SerializeField] private TextMeshProUGUI perfect_text;
    [BoxGroup("UI member")] [SerializeField] private TextMeshProUGUI good_text;
    [BoxGroup("UI member")] [SerializeField] private TextMeshProUGUI miss_text;
    [BoxGroup("UI member")] [SerializeField] private TextMeshProUGUI full_combo_text;
    [BoxGroup("UI member")] [SerializeField] private TextMeshProUGUI accuracy_text;

    [BoxGroup("UI member")] [SerializeField] private Button okay_button;

    [BoxGroup("UI opponent")] [SerializeField] private GameObject opponent_result_object;
    [BoxGroup("UI opponent")] [SerializeField] private TextMeshProUGUI opponent_song_title_text;
    [BoxGroup("UI opponent")] [SerializeField] private TextMeshProUGUI opponent_song_level_text;
    [BoxGroup("UI opponent")] [SerializeField] private TextMeshProUGUI opponent_combo_text;
    [BoxGroup("UI opponent")] [SerializeField] private TextMeshProUGUI opponent_perfect_text;
    [BoxGroup("UI opponent")] [SerializeField] private TextMeshProUGUI opponent_good_text;
    [BoxGroup("UI opponent")] [SerializeField] private TextMeshProUGUI opponent_miss_text;
    [BoxGroup("UI opponent")] [SerializeField] private TextMeshProUGUI opponent_full_combo_text;
    [BoxGroup("UI opponent")] [SerializeField] private TextMeshProUGUI opponent_accuracy_text;

    [BoxGroup("Reference")] [SerializeField] private Animator animator;
    [BoxGroup("Reference")] [SerializeField] private GameObject result_animation;

    WaitForSeconds wait_time = new WaitForSeconds(1.5f);

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
		StartCoroutine(ShowOppentResult());
        int perfect = GameManager.gamemanager.get_perfect();
        int good = GameManager.gamemanager.get_good();
        int miss = GameManager.gamemanager.get_miss();
        int highest_combo = GameManager.gamemanager.get_highest_combo();
        int total_combo = GameManager.gamemanager.get_total_combo();
        float acr = GameManager.gamemanager.get_accuracy();
        string title = GameManager.gamemanager.get_song_title();

        song_title_text.text = title;
        song_level_text.text = "LEVEL " + GameManager.gamemanager.get_level();
        combo_text.text = "COMBO " + highest_combo.ToString() + "/" + total_combo.ToString();
        perfect_text.text = "PERFECT " + perfect.ToString();
        good_text.text = "GOOD " + good.ToString();
        miss_text.text = "MISS " + miss.ToString();
        accuracy_text.text = "Accuracy " + string.Format("{0:N2}", acr) + "%";

        if (miss == 0)
        {
            full_combo_text.gameObject.SetActive(true);
        }

        if(title=="")
        {
            return;
        }

        if (GameManager.gamemanager.get_level_difficulty() == level_difficulty.basic)
        {
            if (PlayerPrefs.HasKey(title + "_basic") == false)
            {
                PlayerPrefs.SetInt(title + "_basic", 1);
                PlayerPrefs.SetInt(title + "_basic_perfect", perfect);
                PlayerPrefs.SetInt(title + "_basic_good", good);
                PlayerPrefs.SetInt(title + "_basic_miss", miss);
                PlayerPrefs.SetInt(title + "_basic_total_combo", total_combo);
                PlayerPrefs.SetInt(title + "_basic_current_combo", highest_combo);
                PlayerPrefs.SetFloat(title + "_basic_acr", acr);
            }
            else
            {
                if (PlayerPrefs.GetFloat(title + "_basic_acr") < acr)
                {
                    PlayerPrefs.SetInt(title + "_basic_perfect", perfect);
                    PlayerPrefs.SetInt(title + "_basic_good", good);
                    PlayerPrefs.SetInt(title + "_basic_miss", miss);
                    PlayerPrefs.SetInt(title + "_basic_total_combo", total_combo);
                    PlayerPrefs.SetInt(title + "_basic_current_combo", highest_combo);
                    PlayerPrefs.SetFloat(title + "_basic_acr", acr);
                }
            }

        }
        else if (GameManager.gamemanager.get_level_difficulty() == level_difficulty.skilled)
        {
            if (PlayerPrefs.HasKey(title + "_skilled") == false)
            {
                PlayerPrefs.SetInt(title + "_skilled", 1);
                PlayerPrefs.SetInt(title + "_skilled_perfect", perfect);
                PlayerPrefs.SetInt(title + "_skilled_good", good);
                PlayerPrefs.SetInt(title + "_skilled_miss", miss);
                PlayerPrefs.SetInt(title + "_skilled_total_combo", total_combo);
                PlayerPrefs.SetInt(title + "_skilled_current_combo", highest_combo);
                PlayerPrefs.SetFloat(title + "_skilled_acr", acr);
            }
            else
            {
                if (PlayerPrefs.GetFloat(title + "_skilled_acr") < acr)
                {
                    PlayerPrefs.SetInt(title + "_skilled_perfect", perfect);
                    PlayerPrefs.SetInt(title + "_skilled_good", good);
                    PlayerPrefs.SetInt(title + "_skilled_miss", miss);
                    PlayerPrefs.SetInt(title + "_skilled_total_combo", total_combo);
                    PlayerPrefs.SetInt(title + "_skilled_current_combo", highest_combo);
                    PlayerPrefs.SetFloat(title + "_skilled_acr", acr);
                }
            }
        }

    }
    IEnumerator ShowOppentResult()
    {
        yield return new WaitForSeconds(1.5f);
        if (GameManager.gamemanager.get_multi_play())
        {
            result_animation.SetActive(true);
            opponent_result_object.SetActive(true);
            opponent_song_title_text.text = GameManager.gamemanager.get_song_title();
            opponent_song_level_text.text = "LEVEL " + GameManager.gamemanager.get_level();
            opponent_combo_text.text = "COMBO " + NetworkManager.Instance.OpponentHighestCombo.ToString()
                + "/" + NetworkManager.Instance.OpponentTotalCombo.ToString();
            opponent_perfect_text.text = "PERFECT " + NetworkManager.Instance.OpponentPerfect.ToString();
            opponent_good_text.text = "GOOD " + NetworkManager.Instance.OpponentGood.ToString();
            opponent_miss_text.text = "MISS " + NetworkManager.Instance.OpponentMiss.ToString();
            opponent_accuracy_text.text = "Accuracy " + string.Format("{0:N2}", NetworkManager.Instance.OpponentAcr) + "%";
            if (NetworkManager.Instance.OpponentMiss == 0)
            {
                opponent_full_combo_text.gameObject.SetActive(true);
            }
        }
    }

    public void click_event()
    {
        animator.SetBool("OnlyFalse", true);
        okay_button.gameObject.SetActive(false);
        StartCoroutine("Scene_load");
    }

    IEnumerator Scene_load()
    {
        yield return wait_time;
        NetworkManager.Instance.LeaveGame();
        SceneManager.LoadScene("SelectScene");
    }
}
