using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Vector2 scrollSpeed;

    private Renderer myRenderer;
    //private Vector2 savedOffset;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newOffset = new Vector2(
            Mathf.Repeat(Time.time * scrollSpeed.x, 1),
            Mathf.Repeat(Time.time * scrollSpeed.y, 1));

        myRenderer.sharedMaterial.SetTextureOffset("_MainTex", newOffset);
    }
}
