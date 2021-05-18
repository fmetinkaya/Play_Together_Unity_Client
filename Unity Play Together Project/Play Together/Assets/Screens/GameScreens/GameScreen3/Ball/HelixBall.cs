using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using MoreMountains.NiceVibrations;

public class HelixBall : MonoBehaviour
{
    int pitchKeeper = 0;
    public CanvasGroup redImageCG;

    public ParticleSystem speedTrailRender;
    public ParticleSystem speedParticle;

    public GameObject splashParticleEffectPrefab;
    public GameObject splashEffectPrefab;
    Rigidbody ballRB;
    public float speed = 1f;
    public static float GlobalGravity = 9.81f;
    public float gravityScale = 1.0f;
    bool isForce = true;

    float ballVelocity = 0;

    float highSpeedLimit = 10;

    float cameraBallDifference = 1.55f;

    public AudioClip destroyHelix;
    public AudioClip splash;
    public AudioSource audioSourceSplash;
    public AudioSource audioSourcedestroyHelix;


    void Start()
    {

        audioSourceSplash.volume = PlayerPrefs.GetFloat("Setting_SoundFx");
        audioSourcedestroyHelix.volume = PlayerPrefs.GetFloat("Setting_SoundFx");

        ballRB = GetComponent<Rigidbody>();
        ballRB.useGravity = false;

    }

    void FixedUpdate()
    {
        Vector3 gravity = GlobalGravity * gravityScale * Vector3.down;
        ballRB.AddForce(gravity, ForceMode.Acceleration);
    }

    void Update()
    {
        ballVelocity = ballRB.velocity.y;

        speedEffectStartStop(ballVelocity < -highSpeedLimit + 3);

        if (transform.position.y < Camera.main.transform.position.y - cameraBallDifference)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y + cameraBallDifference, Camera.main.transform.position.z);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (isForce)
        {
            audioEffectPlay(splash);

            if (collision.gameObject.GetComponent<HelixPieceScript>().helixState == HelixPieceScript.HelixState.obstacle && ballVelocity >= -highSpeedLimit)
                isHitObstacle();

            isForce = false;

            ballRB.velocity = Vector3.zero;
            ballRB.AddForce(Vector3.up * speed, ForceMode.Impulse);

            ContactPoint contact = collision.GetContact(0);

            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            GameObject splashEffect = Instantiate(splashEffectPrefab, pos, Quaternion.identity);
            splashEffect.transform.SetParent(collision.gameObject.transform, true);
            splashEffect.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
            splashEffect.transform.localScale = splashEffectPrefab.transform.localScale;
            splashEffect.transform.localPosition = new Vector3(splashEffect.transform.localPosition.x, splashEffect.transform.localPosition.y, -0.001f);

            GameObject splashParticleEffect = Instantiate(splashParticleEffectPrefab, new Vector3(pos.x, pos.y, pos.z), splashParticleEffectPrefab.transform.rotation);
            Destroy(splashParticleEffect, 1);

            transform.GetComponent<Animator>().SetTrigger("AnimationTrigger");

            if (ballVelocity < -highSpeedLimit)
            {
                ParticleSystem.Burst burts = splashParticleEffect.GetComponent<ParticleSystem>().emission.GetBurst(0);
                burts.count = 300;
                splashParticleEffect.GetComponent<ParticleSystem>().emission.SetBurst(0, burts);

                collision.transform.parent.gameObject.GetComponent<HelixScript>().desTroyAnimation();
                Game3Management.game3Management.scoreDisplay(Game3Management.game3Management.extremeAddScore);

                audioEffectPlay(splash);

                if (Convert.ToBoolean(PlayerPrefs.GetInt("Setting_Vibration")))
                    MMVibrationManager.Haptic(HapticTypes.HeavyImpact);

            }
            else
            {
                if (Convert.ToBoolean(PlayerPrefs.GetInt("Setting_Vibration")))
                    MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            }


        }
        Invoke("isForceUpdate", 0.2f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Border" && isForce)
        {
            other.transform.parent.gameObject.GetComponent<HelixScript>().desTroyAnimation();
            Game3Management.game3Management.scoreDisplay(Game3Management.game3Management.ordinaryAddScore);

            audioEffectPlay(destroyHelix);
        }
    }


    void isForceUpdate()
    {
        isForce = true;
    }

    void isHitObstacle()
    {
        Debug.Log("isHitObstacle");
        redImageCG.alpha = 1;
        redImageCG.DOFade(0, 0.5f);

        Game3Management.game3Management.scoreDisplay(Game3Management.game3Management.ordinaryRemoveScore);
    }

    void speedEffectStartStop(bool isOn)
    {
        if (isOn && !speedTrailRender.isPlaying)
        {
            speedTrailRender.Simulate(0, true, true);
            speedTrailRender.Play();
            speedParticle.Simulate(0, true, true);
            speedParticle.Play();
        }
        if (!isOn && speedTrailRender.isPlaying)
        {
            speedTrailRender.Stop();
            speedParticle.Stop();
        }

    }

    void audioEffectPlay(AudioClip clip)
    {
        if (clip == destroyHelix)
        {
            audioSourcedestroyHelix.pitch += pitchKeeper * 0.2f;
            pitchKeeper += 1;
            audioSourcedestroyHelix.clip = clip;
            audioSourcedestroyHelix.Play();
        }
        else
        {
            audioSourcedestroyHelix.pitch = 1;
            pitchKeeper = 0;

            audioSourceSplash.clip = clip;
            audioSourceSplash.Play();
        }

    }

}
