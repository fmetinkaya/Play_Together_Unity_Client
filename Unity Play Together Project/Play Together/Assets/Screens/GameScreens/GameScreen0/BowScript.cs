using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using TMPro;


public class BowScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject game0Management;
    Game0Management game0ManagementScript;
    LineRenderer bowStringLinerenderer;
    List<Vector3> bowStringPosition;
    public GameObject arrowPrefab;
    GameObject arrow;
    public Camera camera;
    float length;
    Vector3 stringPullout;
    public GameObject scorePrefab;
    public AudioSource audioSource;
    public AudioClip bowDraw;
    public AudioClip bowRelease;



    void Start()
    {
        game0ManagementScript = game0Management.GetComponent<Game0Management>();

        audioSource.volume = PlayerPrefs.GetFloat("Setting_SoundFx");

        bowStringLinerenderer = transform.GetChild(0).gameObject.AddComponent<LineRenderer>();
        bowStringLinerenderer.positionCount = 3;
        bowStringLinerenderer.endWidth = 0.05F;
        bowStringLinerenderer.startWidth = 0.05F;
        bowStringLinerenderer.useWorldSpace = false;
        bowStringLinerenderer.material = Resources.Load("Materials/bowStringMaterial") as Material;
        bowStringPosition = new List<Vector3>();
        bowStringPosition.Add(new Vector3(-0.44f, 1.43f, 2f));
        bowStringPosition.Add(new Vector3(-0.44f, -0.06f, 2f));
        bowStringPosition.Add(new Vector3(-0.43f, -1.32f, 2f));
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, bowStringPosition[1]);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
        bowStringLinerenderer.sortingOrder = 1;
        stringPullout = new Vector3(-0.44f, -0.06f, 2f);

    }

    void Update()
    {
        drawBowString();
    }

    public void drawBowString()
    {
        bowStringLinerenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, stringPullout);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        this.transform.localRotation = Quaternion.identity;
        arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        arrow.transform.localScale = this.transform.localScale;
        arrow.transform.localPosition = this.transform.position + new Vector3(0.7f, 0, 0);
        arrow.transform.localRotation = this.transform.localRotation;
        arrow.transform.SetParent(this.transform);

        arrow.GetComponent<ArrowScript>().game0Management = game0Management;

        audioSource.PlayOneShot(bowDraw);
        //Debug.Log("Dokunan parmak hareket etmeye başladı: " + eventData.pointerId + " " + eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Parmak hareket ediyor: " + eventData.pointerId + " " + eventData.position + " " + camera.WorldToScreenPoint(transform.position));

        Vector2 mousePos = new Vector2(transform.position.x - camera.ScreenToWorldPoint(eventData.position).x, transform.position.y - camera.ScreenToWorldPoint(eventData.position).y);
        float angleZ = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angleZ);

        length = mousePos.magnitude / 3f;
        length = Mathf.Clamp(length, 0, 1);
        stringPullout = new Vector3(-(0.44f + length), -0.06f, 2f);


        Vector3 arrowPosition = arrow.transform.localPosition;
        arrowPosition.x = (0.7f - length);
        arrow.transform.localPosition = arrowPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Parmak dokunmayı kesti: " + eventData.pointerId + " " + eventData.position);
        audioSource.PlayOneShot(bowRelease);
        shootArrow();
        stringPullout = new Vector3(-0.44f, -0.06f, 2f);
    }

    public void shootArrow()
    {
        arrow.AddComponent<Rigidbody2D>();
        arrow.transform.SetParent(null);
        arrow.GetComponent<Rigidbody2D>().AddForce(1000f * length * transform.right);
        arrow.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        arrow.GetComponent<ArrowScript>().PlayShooshClip();

        game0ManagementScript.ScoreEffectCreate(transform.TransformPoint(stringPullout), -10, Color.white, Color.red);
    }


}
