using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBackground : MonoBehaviour
{

    BoxCollider2D groundCollider;
    float groundHorizontalLength;

    void Awake()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        groundHorizontalLength = groundCollider.bounds.size.x;
    }
    void Update()
    {
        if (transform.position.x < -groundHorizontalLength)
        {
            RepositionBackground();
        }
    }

    void RepositionBackground()
    {
        transform.position = new Vector3(transform.position.x + 6.3f * 2f, transform.position.y, 0);
    }

}
