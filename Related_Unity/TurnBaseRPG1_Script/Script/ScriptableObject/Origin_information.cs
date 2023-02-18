using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Origin_information")]
public class Origin_information : ScriptableObject
{
    public Character_Origin origin;
    public Sprite origin_sprite;

    public string origin_name;
    public List<string> origin_information;

    public string ret_origin_information()
    {
        string ret = "";

        for(int i = 0 ; i < origin_information.Count ; i++)
        {
            ret += origin_information[i];
            if(i < origin_information.Count - 1)
            {
                ret += '\n';
            }
        }

        return ret;
    }
}
