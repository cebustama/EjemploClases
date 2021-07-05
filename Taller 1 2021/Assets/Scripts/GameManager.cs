using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool esconderMouse = false;

    private void Awake()
    {
        //Cursor.visible = esconderMouse;
    }

    // Update is called once per frame
    void Update()
    {
        // Cámara lenta
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 0.5f;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            Time.timeScale = 1;
        }
        
    }

}