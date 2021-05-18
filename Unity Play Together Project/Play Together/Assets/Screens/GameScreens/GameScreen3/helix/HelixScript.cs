using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixScript : MonoBehaviour
{
    public GameObject HelixDestroyEffectPrefab;
    public void desTroyAnimation()
    {
        Invoke("destroySplashEffectOnGround", 0.5f);
        GameObject HelixDestroyEffect = Instantiate(HelixDestroyEffectPrefab, new Vector3(0, transform.position.y, 0), Quaternion.Euler(-90, 0, 0));
        Destroy(HelixDestroyEffect, 1f);
        Game3Management.game3Management.SpawnHelixes(gameObject);

    }
    void destroySplashEffectOnGround()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandson in child)
            {
                Destroy(grandson.gameObject);
            }
        }
    }
}


