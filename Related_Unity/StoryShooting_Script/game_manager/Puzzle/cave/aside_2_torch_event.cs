using UnityEngine;
using System.Collections;

public class aside_2_torch_event : MonoBehaviour {

    public aside_2_wall puzzle;
    public bool torch,button;
    public int button_num;
    public GameObject parent;
    public Animator anim, my_anim;
    private AudioSource ad_source;
    public AudioClip sound;
	// Use this for initialization
	void Start () {
        anim = parent.GetComponent<Animator>();
        my_anim = GetComponent<Animator>();
        puzzle = FindObjectOfType<aside_2_wall>();
        ad_source = GetComponent<AudioSource>();
        //PlayerPrefs.SetInt("cave_puzzle_2", 0);
    }
	
	// Update is called once per frame

    void Update()
    {
        if(puzzle!=null)
        {
            puzzle.answer[button_num - 1] = torch;
        }
        anim.SetBool("torch", torch);
        my_anim.SetBool("button", button);
    }

    void OnTriggerEnter2D()
    {
        button = true;
        torch = !torch;
        ad_source.PlayOneShot(sound);
    }

    void OnTriggerExit2D()
    {
        button = false;
        ad_source.PlayOneShot(sound);
        if (PlayerPrefs.GetInt("cave_puzzle_2") == 0)
        {
            puzzle.checking();
        }
    }
}
