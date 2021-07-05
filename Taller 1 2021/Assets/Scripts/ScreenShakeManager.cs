using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    public static ScreenShakeManager instance;
    public float maxShake = 1.5f;

    Vector3 originalCamPos;

    float totalShakeMagnitude = 0;

    [System.Serializable]
    public struct ShakeLevel
    {
        public float minAmount;
        public float maxAmount;
        public float time;
    }

    public ShakeLevel[] customLevels;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        originalCamPos = transform.localPosition;
    }

    private void Update()
    {
        // Shaking
        if (totalShakeMagnitude > 0)
        {
            transform.localPosition = originalCamPos + (Vector3)Random.insideUnitCircle * totalShakeMagnitude;
        }
        else
        {
            transform.localPosition = originalCamPos;
        }
    }

    public void AddShakeManual(float amount)
    {
        totalShakeMagnitude += amount;
    }

    public void RemoveShakeManual(float amount)
    {
        totalShakeMagnitude -= amount;
    }

    public void AddShake(int level)
    {
        if (customLevels.Length > level)
        {
            //Debug.Log("Adding Shake Level " + level);
            AddShake(Random.Range(customLevels[level].minAmount, customLevels[level].maxAmount), customLevels[level].time);
        }
    }

    public void AddShake(float amount, float time)
    {
        float diff = IncreaseMagnitude(amount);

        StartCoroutine(EndShake(diff, time));
    }

    float IncreaseMagnitude(float amount)
    {
        float currMagnitude = totalShakeMagnitude;
        if (totalShakeMagnitude + amount > maxShake)
        {
            totalShakeMagnitude = maxShake;
        }
        else
        {
            totalShakeMagnitude = totalShakeMagnitude + amount;
        }

        float diff = totalShakeMagnitude - currMagnitude;
        return diff;
    }

    IEnumerator EndShake(float amount, float delay)
    {
        yield return new WaitForSeconds(delay);

        totalShakeMagnitude -= amount;

        if (totalShakeMagnitude < 0) totalShakeMagnitude = 0;
    }
}
