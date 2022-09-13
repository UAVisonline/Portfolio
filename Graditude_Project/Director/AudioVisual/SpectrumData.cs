using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumData : MonoBehaviour
{
    private float[] spectrum = new float[128];
    public static float spectrum_value { get; private set; }

    // Update is called once per frame
    void Update()
    {
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        spectrum_value = spectrum[0]*100;
    }
}
