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
            colorKeys[i] = new GradientColorKey(from.colorKeys[i].color * mul, from.colorKeys[i].time);
        }
        to.SetKeys(colorKeys, alphaKeys);
        return to;
    }
}
