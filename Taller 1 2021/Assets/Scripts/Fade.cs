using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image fadeOut;
    public float fadeTime;

    private void Start()
    {
        StartCoroutine(FadeOutCo());
    }

    private IEnumerator FadeOutCo()
    {
        float time = fadeTime;
        while (fadeOut.color.a >= 0)
        {
            time -= Time.deltaTime;
            float newAlpha = (time / fadeTime);
            fadeOut.color = new Color(fadeOut.color.r, fadeOut.color.g, fadeOut.color.b, newAlpha);
            yield return null;
        }
    }
}
