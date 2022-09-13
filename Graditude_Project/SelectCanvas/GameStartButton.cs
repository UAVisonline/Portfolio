using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    [BoxGroup("Reference")] [SerializeField] private Album_Select select_obj;
    [BoxGroup("Reference")] [SerializeField] private Button button;
    [BoxGroup("Reference")] [SerializeField] private AudioSource audioSource;

    [BoxGroup("Reference")] [SerializeField] private Animator animator;

    [BoxGroup("Variable")] [SerializeField] private AudioClip start;
    [BoxGroup("Variable")] [SerializeField] private AudioClip wrong;

    private void Awake()
    {
        select_obj = FindObjectOfType<Album_Select>();
        button = this.GetComponent<Button>();
        audioSource = this.GetComponent<AudioSource>();
    }

    [Button]
    public void Click_button()
    {
        if(select_obj.is_pattern_exist()==true)
        {
            audioSource.PlayOneShot(start);
            Gamemanager_set();
            Game_canvas_change();
        }
        else
        {
            audioSource.PlayOneShot(wrong);
        }
    }

    private void Gamemanager_set()
    {
        GameManager.gamemanager.set_music(select_obj.get_current_music_object().get_full_clip());
        GameManager.gamemanager.set_Stage(select_obj.get_director()); // directing_mode assign together
        GameManager.gamemanager.set_music_title(select_obj.get_current_music_object().get_song_title());
        GameManager.gamemanager.set_music_level(select_obj.get_level_number());
        GameManager.gamemanager.set_pattern(select_obj.get_pattern()); // level_difficulty assign together
        //GameManager.gamemanager.set_level_difficulty(select_obj.get_level_difficulty());
        GameManager.gamemanager.set_multiplay_status(select_obj.is_multiplay());

        //SceneManager.LoadScene("3DnewVersion");
    }

    private void Game_canvas_change()
    {
        if(select_obj.is_multiplay()==true)
        {
            NetworkManager.Init();            
            NetworkManager.Instance.GamePlaySet(select_obj.get_music_index());            
            NetworkManager.Instance.EnterGameRoom();
            GameManager.gamemanager.set_multi_game(true);
            animator.SetBool("MultiPlay", true);
            Debug.Log("total count: " + GameManager.gamemanager.get_pattern().get_informations().Count);
        }
        else
        {
            SceneManager.LoadScene("3DnewVersion");
        }
    }
}
