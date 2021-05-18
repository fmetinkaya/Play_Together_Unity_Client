using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleObstacleScript : MonoBehaviour
{
    public Shader shader;

    bool isFall = false;
    void Start()
    {
        shader = GetComponent<Renderer>().material.shader;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "TriggerGround" && !isFall)
        {
            isFall = true;
            Debug.Log("exit " + name);
            float volume;

            if (GetComponent<BoxCollider>())
                GetComponent<MeshCollider>().enabled = true;

            volume = (GetComponent<MeshCollider>().sharedMesh.bounds.size.x * transform.localScale.x) *
            (GetComponent<MeshCollider>().sharedMesh.bounds.size.y * transform.localScale.y) *
            (GetComponent<MeshCollider>().sharedMesh.bounds.size.z * transform.localScale.z);

            Game4Management.game4Management.addScore(volume);

            Destroy(this, 2);
        }
    }

   

}
