using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CheckIfUnlocked : MonoBehaviour
{
    public int levelIndex;
    public Button button;

    private void Start()
    {
        if (PlayerPrefs.GetInt("level" + levelIndex + "locked") == 1)
        {
            button.interactable = false;
        }
    }
}
