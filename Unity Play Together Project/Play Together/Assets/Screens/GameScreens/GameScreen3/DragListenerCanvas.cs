using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragListenerCanvas : MonoBehaviour, IDragHandler
{
    public GameObject realCyclinder;
    public GameObject cameraCyclinder;

    public float rotateSpeed = -10f;
    public void OnDrag(PointerEventData data)
    {
        Debug.Log(data.delta);

        if (Time.timeScale == 1)
        {
            realCyclinder.transform.Rotate(Vector3.up, data.delta.x * Mathf.Deg2Rad * rotateSpeed);
            cameraCyclinder.transform.Rotate(Vector3.up, data.delta.x * Mathf.Deg2Rad * rotateSpeed);
        }
    }
}
