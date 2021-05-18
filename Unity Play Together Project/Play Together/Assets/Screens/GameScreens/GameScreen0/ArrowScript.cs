using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using MoreMountains.NiceVibrations;


public class ArrowScript : MonoBehaviour
{
    public GameObject game0Management;
    Game0Management game0ManagementScript;

    Vector3 lastAngleArrow;
    bool isHitArrowHead = false;
    public GameObject scorePrefab;
    public AudioSource audioSource;
    public AudioClip arrowHitTarget;
    public AudioClip arrowShoosh;
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        game0ManagementScript = game0Management.GetComponent<Game0Management>();

        Physics2D.IgnoreLayerCollision(8, 8);
        camera = Camera.main;

        audioSource.volume = PlayerPrefs.GetFloat("Setting_SoundFx");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<Rigidbody2D>() != null)
        {
            // do we fly actually?
            if (!GetComponent<Rigidbody2D>().isKinematic && GetComponent<Rigidbody2D>().velocity != Vector2.zero)
            {

                // get the actual velocity
                Vector3 vel = GetComponent<Rigidbody2D>().velocity;
                // calc the rotation from x and y velocity via a simple atan2
                float angleZ = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                float angleY = Mathf.Atan2(vel.z, vel.x) * Mathf.Rad2Deg;
                // rotate the arrow according to the trajectory
                if (vel.x < 0)
                {
                    lastAngleArrow = (transform.eulerAngles = new Vector3(0, -angleY, 180 - angleZ));
                }
                else
                {
                    lastAngleArrow = (transform.eulerAngles = new Vector3(0, -angleY, angleZ));
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        col.GetContacts(contacts);

        if (col.gameObject.tag == "G0target" && isHitArrowHead)
        {
            if (!GetComponent<Rigidbody2D>().isKinematic)
            {
                Debug.Log("OnCollisionEnter2D " + col.gameObject.name);

                if (Convert.ToBoolean(PlayerPrefs.GetInt("Setting_Vibration")))
                    MMVibrationManager.Haptic(HapticTypes.RigidImpact);

                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                GetComponent<Rigidbody2D>().isKinematic = true; ;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                transform.SetParent(col.transform);

                transform.eulerAngles = lastAngleArrow;

                int score = 0;
                Color backgroundColor = Color.green;
                switch (col.gameObject.name)
                {
                    case "targetWhite":
                        score = 3;
                        backgroundColor = Color.white;
                        break;
                    case "targetBlack":
                        score = 5;
                        backgroundColor = Color.black;
                        break;
                    case "targetBlue":
                        score = 10;
                        backgroundColor = new Color32(0, 160, 255, 255);
                        break;
                    case "targetRed":
                        score = 15;
                        backgroundColor = Color.red;
                        break;
                    case "targetYellow":
                        score = 20;
                        backgroundColor = Color.yellow;
                        break;
                }
                game0ManagementScript.ScoreEffectCreate(new Vector3(contacts[0].point.x, contacts[0].point.y, 0), score, backgroundColor, Color.white);

                audioSource.PlayOneShot(arrowHitTarget);

            }

        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnCollisionEnter2D " + col.gameObject.tag);
        if (col.gameObject.tag == "G0target")
        {
            if (!GetComponent<Rigidbody2D>().isKinematic)
            {
                isHitArrowHead = true;
            }

        }
    }
    public void PlayShooshClip()
    {
        audioSource.PlayOneShot(arrowShoosh);

    }

}
