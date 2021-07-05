using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ChangeSceneTimer : MonoBehaviour
{
    public float time;
    private float timer;

    public int nextSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        timer = time;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
