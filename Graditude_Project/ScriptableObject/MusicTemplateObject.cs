using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Music_Template_Object",menuName = "Scriptable Object/Music_Template_Object")]
public class MusicTemplateObject : ScriptableObject // 음악 관련 정보를 담을 수 있게 만들어주는 Scriptable Object
{
    [TabGroup("UI Component")] [SerializeField] private Sprite album_jacket;

    [TabGroup("Song Information")] [SerializeField] private string song_title;
    [TabGroup("Song Information")] [SerializeField] private List<string> artist_name;

    [TabGroup("Song Information")] [SerializeField] private int easy_level;
    [TabGroup("Song Information")] [SerializeField] private int normal_level;
    [TabGroup("Song Information")] [SerializeField] private int hard_level;

    [BoxGroup("Audio Clip")] [SerializeField] private AudioClip short_clip; // 미리듣기 clip
    [BoxGroup("Audio Clip")] [SerializeField] private AudioClip full_clip; // 실제 노래 clip -> 게임 돌입 시 이게 재생

    [BoxGroup("Note Object")] [SerializeField] private MusinPatternObject normal_pattern;
    [BoxGroup("Note Object")] [SerializeField] private MusinPatternObject hard_pattern;

    [BoxGroup("Director Object")] [SerializeField] private GameObject director; // 해당 음악 전용 연출 장치
    [BoxGroup("Director Object")] [SerializeField] private GameObject common_director; // 음악 공용 연출 장치
    [BoxGroup("Director Object")] [SerializeField] private GameObject nothing_director;

    public Sprite get_album_jacket()
    {
        return album_jacket;
    }

    public string get_artist_name()
    {
        string artist = "";
        for(int i =0;i<artist_name.Count;i++)
        {
            artist += artist_name[i];
            if (i != artist_name.Count - 1) artist += ", ";
        }

        return artist;
    }

    public string get_song_title()
    {
        return song_title;
    }

    public string get_easy_level()
    {
        return easy_level.ToString();
    }

    public string get_normal_level()
    {
        return normal_level.ToString();
    }

    public string get_hard_level()
    {
        return hard_level.ToString();
    }

    public AudioClip get_short_clip()
    {
        return short_clip;
    }

    public AudioClip get_full_clip()
    {
        return full_clip;
    }

    public MusinPatternObject get_normal_pattern()
    {
        return normal_pattern;
    }

    public MusinPatternObject get_hard_pattern()
    {
        return hard_pattern;
    }

    public GameObject get_stage()
    {
        return director;
    }

    public GameObject get_common_stage()
    {
        return common_director;
    }

    public GameObject get_nothing_stage()
    {
        return nothing_director;
    }

    // 나머지 함수는 변수 반환 함수들
}
