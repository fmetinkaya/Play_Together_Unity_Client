using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    Rigidbody2D rb2d;
    void Start()
    {

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(Game1Management.game1Management.gameMoveSpeed, 0);
    }
}
