using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTransparentScript : MonoBehaviour
{
    public Material TransparentMaterial;
    Vector3 startPosition = new Vector3();
    public GameObject hole;
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = startPosition + new Vector3(hole.transform.position.x, 0, hole.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "G4Obstacle")
        {
            other.transform.GetComponent<Renderer>().material.shader = TransparentMaterial.shader;
            Color colorB = other.transform.GetComponent<Renderer>().material.color;
            colorB.a = 0.1f;
            other.transform.GetComponent<Renderer>().material.SetColor("_Color", colorB);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "G4Obstacle")
        {
            other.transform.GetComponent<Renderer>().material.shader = other.transform.GetComponent<HoleObstacleScript>().shader;
            Color colorB = other.transform.GetComponent<Renderer>().material.color;
            colorB.a = 1;
            other.transform.GetComponent<Renderer>().material.SetColor("_Color", colorB);
        }

    }
}
