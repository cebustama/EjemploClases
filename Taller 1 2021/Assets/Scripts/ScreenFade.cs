using System.Collections;
using System;
using UnityEngine;

using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    Image image;



    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Fade(float time)
    {
        StartCoroutine(FadeInOut(time));
    }

    IEnumerator FadeInOut(float time)
    {
        Color curColor = image.color;
        float currTime = time / 2f;
        while (Mathf.Abs(curColor.a - 1f) > 0.0001f)
        {
            currTime -= Time.deltaTime;
            curColor.a = Mathf.Lerp(curColor.a, 1f, currTime);
            image.color = curColor;
            yield return null;
        }

        currTime = time / 2f;
        while (Mathf.Abs(curColor.a - 0f) > 0.0001f)
        {
            currTime -= Time.deltaTime;
            curColor.a = Mathf.Lerp(curColor.a, 0f, currTime);
            image.color = curColor;
            yield return null;
        }
    }
}
