using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MoreMountains.NiceVibrations;


public class BirdScript : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip die;
    public AudioClip hit;
    public AudioClip point;
    public AudioClip swooshing;
    public AudioClip wing;


    Rigidbody2D birdRigidbody;
    void Start()
    {
        birdRigidbody = transform.GetComponent<Rigidbody2D>();

            audioSource.volume = PlayerPrefs.GetFloat("Setting_SoundFx");
    }

    void Update()
    {

        if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && Time.timeScale != 0)
        {
            birdRigidbody.velocity = Vector2.up * Game1Management.game1Management.birdUpForce;
            audioSource.PlayOneShot(wing);

            if (Convert.ToBoolean(PlayerPrefs.GetInt("Setting_Vibration")))
                MMVibrationManager.Haptic(HapticTypes.RigidImpact);

            Debug.Log("Clicked on screen");
        }
        updateAngle();
    }

    void updateAngle()
    {
        float verticalVelocity = birdRigidbody.velocity.y;
        transform.eulerAngles = new Vector3(0, 0, verticalVelocity * 45f / Game1Management.game1Management.birdUpForce);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Game1Management.game1Management.RestartGame();
        birdRigidbody.velocity = Vector3.zero;
        transform.position = Vector3.zero;

        audioSource.PlayOneShot(hit);
        audioSource.PlayOneShot(die);
    }
}
