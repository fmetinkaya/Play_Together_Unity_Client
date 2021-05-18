using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixPieceScript : MonoBehaviour
{
    public enum HelixState
    {
        hide,
        real,
        obstacle
    }
    public HelixState helixState;
    public void SetState(HelixState helixState)
    {
        this.helixState = helixState;
        if (helixState == HelixState.hide)
        {
            SetActiveParent(false);
        }
        if (helixState == HelixState.real)
        {
            SetActiveParent(true);
            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
        if (helixState == HelixState.obstacle)
        {
            SetActiveParent(true);
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    public void SetActiveParent(bool isActive)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = isActive;
        gameObject.GetComponent<MeshCollider>().enabled = isActive;
    }
}
