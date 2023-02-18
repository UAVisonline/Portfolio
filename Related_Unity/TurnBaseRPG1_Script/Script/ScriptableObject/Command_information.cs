using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Command")]
public class Command_information : ScriptableObject
{
    public string head;
    
    [TextArea]
    public string body;

    public int require_energy;
}
