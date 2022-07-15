using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "suspect_information", menuName = "ScriptableObject/suspect_information")]
public class suspect_information : ScriptableObject
{
    [SerializeField] private suspect suspect; // 범인 항목
    [SerializeField] private information auto_cacl;
    [SerializeField] private information human_cacl;

    public suspect get_suspect()
    {
        return suspect;
    }

    public information get_auto_cacl()
    {
        return auto_cacl;
    }

    public information get_human_cacl()
    {
        return human_cacl;
    }

    public void set_auto_cacl(information value)
    {
        auto_cacl = value;
    }

    public void set_human_cacl(information value)
    {
        human_cacl = value;
    }
}
