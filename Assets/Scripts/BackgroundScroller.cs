using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float speed = -3f;
    public float width;

    private SpriteRenderer sr;

    private bool stopped = false;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped) return;
        transform.position += new Vector3(Time.deltaTime * speed, 0);
        if (transform.position.x < -width)
		{
            transform.position += new Vector3(width, 0);
		}
    }

    public void Stop() => stopped = true;
}
