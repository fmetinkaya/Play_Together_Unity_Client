using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

   void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Borders Trigger And Object was destroy");
        Destroy(col.transform.root.gameObject);
    }
}
