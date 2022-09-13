using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningFrameScript : TextAnimator
{
    [SerializeField] private Material frame_material;

    public void glitch_set(int value)
    {
        frame_material.SetFloat("_GlitchAmount", (float)value);
    }

    public void pixelate_set(int value)
    {
        frame_material.SetInt("_PixelateSize", value);
    }

    public void hsv_shift_set(int value)
    {
        frame_material.SetInt("_HsvShift", value);
    }
}
