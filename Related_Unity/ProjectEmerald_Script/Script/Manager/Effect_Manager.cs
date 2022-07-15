using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Manager : MonoBehaviour
{
    [SerializeField] private GameObject[] slash_effect = new GameObject[3];
    [SerializeField] private GameObject[] bullet_effect = new GameObject[4];
    private static Effect_Manager _effect_manager;
    
    public static Effect_Manager effect_manager
    {
        get
        {
            if (_effect_manager == null)
            {
                _effect_manager = FindObjectOfType<Effect_Manager>();
                if (_effect_manager == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _effect_manager;
        }
    }

    private void Awake()
    {
        if (_effect_manager == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else if (_effect_manager != null)
        {
            Destroy(this.gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        slash_effect = Resources.LoadAll<GameObject>("sword_slash");
        bullet_effect = Resources.LoadAll<GameObject>("bullet");
    }

    public GameObject Get_slash_effect(string str)
    {
        if (str == "big")
        {
            return slash_effect[0];
        }
        else if (str == "small")
        {
            return slash_effect[2];
        }
        else if (str == "medium")
        {
            return slash_effect[1];
        }
        else
        {
            return slash_effect[1];
        }
    }

    public GameObject Get_bullet_effect(string str)
    {
        if (str == "big")
        {
            return bullet_effect[0];
        }
        else if (str == "small")
        {
            return bullet_effect[2];
        }
        else if (str == "medium")
        {
            return bullet_effect[1];
        }
        else if(str == "no_effect")
        {
            return bullet_effect[3];
        }
        else
        {
            return bullet_effect[1];
        }
    }
}
