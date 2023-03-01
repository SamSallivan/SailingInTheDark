using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GradientTest : MonoBehaviour
{
    public Gradient from;
    public Color mul;
    public Gradient to;

    // Update is called once per frame
    void Update()
    {
        to = MultuplyGradientKeys(from, mul);
    }
    public Gradient MultuplyGradientKeys(Gradient from, Color mul)
    {
        Gradient to = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[from.colorKeys.Length];
        GradientAlphaKey[] alphaKeys = from.alphaKeys;
        for (int i = 0; i < from.colorKeys.Length; i++)
        {
            float h, s, v, h1, s1, v1;
            Color.RGBToHSV(from.colorKeys[i].color, out h,out s,out v);
            Color.RGBToHSV(mul, out h1, out s1, out v1);
            Color color = Color.HSVToRGB(h1, s, v);
            colorKeys[i] = new GradientColorKey(color, from.colorKeys[i].time);
        }
        to.SetKeys(colorKeys, alphaKeys);
        return to;
    }
}
