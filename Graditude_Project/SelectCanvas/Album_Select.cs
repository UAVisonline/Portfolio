using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public enum level_difficulty {basic,skilled}

public enum director_mode { dedicated, common, nothing}

public enum multiplayer_setting { not_play, yes_play }

public class Album_Select : MonoBehaviour
{
    [BoxGroup("Reference")] [SerializeField] List<Album_Index> index_list;
    [BoxGroup("Reference")] [SerializeField] List<MusicTemplateObject> music_list;

    [BoxGroup("Reference")] [SerializeField] AudioSource audioSource;
    [BoxGroup("Reference")] [SerializeField] AudioClip move_sfx;

    [BoxGroup("UI reference")] [SerializeField] private TextMeshProUGUI title;
    [BoxGroup("UI reference")] [SerializeField] private TextMeshProUGUI artist;

    [BoxGroup("UI reference")] [SerializeField] private SelectButton normal_button;
    [BoxGroup("UI reference")] [SerializeField] private SelectButton hard_button;
    [BoxGroup("UI reference")] [SerializeField] private SelectButton dedicated_button;
    [BoxGroup("UI reference")] [SerializeField] private SelectButton common_button;
    [BoxGroup("UI reference")] [SerializeField] private SelectButton nothing_button;
    [BoxGroup("UI reference")] [SerializeField] private SelectButton not_multi_button;
    [BoxGroup("UI reference")] [SerializeField] private SelectButton multi_button;

    [BoxGroup("UI reference")] [SerializeField] private Image pre_image;
    [BoxGroup("UI reference")] [SerializeField] private Image current_image;
    [BoxGroup("UI reference")] [SerializeField] private Image next_image;

    [BoxGroup("Score reference")] [SerializeField] private GameObject score_not_ready;
    [BoxGroup("Score reference")] [SerializeField] private GameObject score_ready;

    private int current_music_index;
    private int pre_music_index;
    private int next_music_index;

    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private level_difficulty level;
    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private director_mode directing_mode;
    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private multiplayer_setting multiplayer_status;

    private void Awake()
    {

        set_all_index(0);
        image_jacket_change();

        audioSource = this.GetComponent<AudioSource>();
        audioSource.loop = true;

        level = level_difficulty.basic;
        directing_mode = director_mode.dedicated;
        multiplayer_status = multiplayer_setting.not_play;

        normal_button.Interact_change_level(level);
        hard_button.Interact_change_level(level);
        dedicated_button.Interact_change_director(directing_mode);
        common_button.Interact_change_director(directing_mode);
        nothing_button.Interact_change_director(directing_mode);
        not_multi_button.Interact_change_multiplay(multiplayer_status);
        multi_button.Interact_change_multiplay(multiplayer_status);
    }

    private void Start()
    {
        function_about_clip_change();
    }

    public level_difficulty get_level_difficulty()
    {
        return level;
    }

    public director_mode get_directing_mode()
    {
        return directing_mode;
    }

    public int get_music_index()
    {
        return current_music_index;
    }

    public MusicTemplateObject get_current_music_object()
    {
        return music_list[current_music_index];
    }

    public MusinPatternObject get_pattern()
    {
        GameManager.gamemanager.set_level_difficulty(level);
        switch (level)
        {
            case level_difficulty.basic:
                return music_list[current_music_index].get_normal_pattern();
            case level_difficulty.skilled:
                return music_list[current_music_index].get_hard_pattern();
        }
        return null;
    }

    public GameObject get_director()
    {
        GameManager.gamemanager.set_director_mode(directing_mode);
        switch (directing_mode)
        {
            case director_mode.dedicated:
                return music_list[current_music_index].get_stage();
            case director_mode.common:
                return music_list[current_music_index].get_common_stage();
            case director_mode.nothing:
                return music_list[current_music_index].get_nothing_stage();
        }
        return null;
    }

    public bool is_multiplay()
    {
        switch (multiplayer_status)
        {
            case multiplayer_setting.yes_play:
                return true;
            case multiplayer_setting.not_play:
                return false;
        }
        return false;
    }

    public string get_level_number()
    {
        switch (level)
        {
            case level_difficulty.basic:
                return music_list[current_music_index].get_normal_level();
            case level_difficulty.skilled:
                return music_list[current_music_index].get_hard_level();
        }
        return "0";
    }

    public Sprite get_music_sprite(int index)
    {
        return music_list[index].get_album_jacket();
    }

    public string get_playerpref_string()
    {
        string tmp = title.text;

        switch (level)
        {
            case level_difficulty.basic:
                tmp += "_basic";
                break;
            case level_difficulty.skilled:
                tmp += "_skilled";
                break;
        }
        return tmp;
    }

    public void set_current_music_index(int value)
    {
        current_music_index = value;
    }

    public void set_level_difficulty(level_difficulty value)
    {
        level = value;

        normal_button.Interact_change_level(level);
        hard_button.Interact_change_level(level);

        visulize_information();
    }

    public void set_directing_mode(director_mode value)
    {
        directing_mode = value;

        dedicated_button.Interact_change_director(directing_mode);
        common_button.Interact_change_director(directing_mode);
    }

    public void set_multiplayer(multiplayer_setting value)
    {
        multiplayer_status = value;

        not_multi_button.Interact_change_multiplay(multiplayer_status);
        multi_button.Interact_change_multiplay(multiplayer_status);
    }

    public int change_value_to_music_index(int value)
    {
        if(value>=music_list.Count)
        {
            value -= music_list.Count;
        }

        if(value < 0)
        {
            value += music_list.Count;
        }

        return value;
    }

    public bool is_pattern_exist()
    {
        switch (level)
        {
            case level_difficulty.basic:
                if (music_list[current_music_index].get_normal_pattern() != null)
                {
                    return true;
                }
                break;
            case level_difficulty.skilled:
                if (music_list[current_music_index].get_hard_pattern() != null)
                {
                    return true;
                }
                break;
        }
        return false;
    }

    private void setting_short_clip()
    {
        AudioClip tmp = music_list[current_music_index].get_short_clip();
        if (audioSource != null) audioSource.clip = tmp;
    }

    private void play_clip()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void stop_clip()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void set_all_index(int value)
    {
        current_music_index = change_value_to_music_index(value);
        pre_music_index = change_value_to_music_index(value - 1);
        next_music_index = change_value_to_music_index(value + 1);
    }

    private void image_jacket_change()
    {
        current_image.sprite = get_music_sprite(current_music_index);
        pre_image.sprite = get_music_sprite(pre_music_index);
        next_image.sprite = get_music_sprite(next_music_index);
    }

    private void function_about_clip_change() //노래 선택 시 음악 재생 및 정보변환
    {
        setting_short_clip();
        apply_music_to_text();
        play_clip();
        visulize_information();

        normal_button.change_text(music_list[current_music_index].get_normal_level());
        hard_button.change_text(music_list[current_music_index].get_hard_level());
    }

    public void btn_next_music_select()
    {
        set_all_index(current_music_index + 1);
        image_jacket_change();

        function_about_clip_change();
    }

    public void btn_pre_music_select()
    {
        set_all_index(current_music_index - 1);
        image_jacket_change();

        function_about_clip_change();
    }

    private void apply_none_to_text()
    {
        title.text = "-----";
        artist.text = "-----";

        normal_button.change_text("---");
        hard_button.change_text("---");
    }

    private void apply_music_to_text()
    {
        string tmp_title = music_list[current_music_index].get_song_title();
        string tmp_artist = music_list[current_music_index].get_artist_name();

        title.text = tmp_title;
        artist.text = tmp_artist;
    }

    private void clear_text()
    {
        title.text = "";
        artist.text = "";
    }

    private void visulize_information()
    {
        if(PlayerPrefs.HasKey(get_playerpref_string())==true)
        {
            score_not_ready.SetActive(false);
            score_ready.SetActive(true);
            if(score_ready.GetComponent<SelectScore>()!=null)
            {
                score_ready.GetComponent<SelectScore>().localscore_visualiize();
            }
        }
        else
        {
            score_not_ready.SetActive(true);
            score_ready.SetActive(false);
        }

    }
}
