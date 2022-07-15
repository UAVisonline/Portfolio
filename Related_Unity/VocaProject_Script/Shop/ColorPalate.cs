using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalate", menuName = "Scriptable Object/ColorPalate")]
public class ColorPalate : ScriptableObject // Player Color Scriptable Object
{
    public string color_name;
    public bool purchased;
    public bool equipment;
    public bool updated;
    public int money;

    public Color cape_color;
    public Color body_color;
}
