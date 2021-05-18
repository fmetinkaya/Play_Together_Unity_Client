using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public float moveSpeedTarget = 1f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0, moveSpeedTarget * Time.deltaTime, 0);
    }

}
