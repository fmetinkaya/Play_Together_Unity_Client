using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdObstacleScript : MonoBehaviour
{
    public bool isTrigger = true;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BirdScript>() != null && isTrigger)
        {
            isTrigger = false;
            Game1Management.game1Management.AddScore();
            other.gameObject.GetComponent<BirdScript>().audioSource.PlayOneShot(other.gameObject.GetComponent<BirdScript>().point);
        }
    }
}
