using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tool_information", menuName = "ScriptableObject/tool_information")]
public class tool_information : ScriptableObject
{
    [SerializeField] private murder_tool tool; // 범행도구 항목
    [SerializeField] private information auto_cacl;
    [SerializeField] private information human_cacl;

    public murder_tool get_tool()
    {
        return tool;
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
