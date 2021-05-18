using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using MoreMountains.NiceVibrations;

public class Game2Management : MonoBehaviour
{

    GameObject gameScreenManager;
    GameScreenManager gameScreenManagerScript;

    RandomManager randomManager;

    public GameObject scorePanel;
    public GameObject noteObjectPrefab;

    public static Game2Management game2Management;
    public Vector3[] noteLinePoisitons = new Vector3[4];
    public Transform[] objectLinePoisitons = new Transform[4];
    public AudioSource audioSource1;
    NoteEvents noteEvents = new NoteEvents();

    public float lowestDuration;
    public float noteVelocity;

    GameObject previousoffTick;

    public int currentNote = 0;

    List<GameObject> NoteObjects = new List<GameObject>();

    public float noteDurationTolerance = 0.1f;
    public float clipLengthTolerance = 0.15f;

    float previousDuration = 0;
    float previousClickTime = 0;


    float perfectTime = 0.05f;
    float greatTime = 0.01f;
    float niceTime = 0.02f;

    public int perfectScore = 200;
    public int greatScore = 100;
    public int niceScore = 50;
    public int ordinaryScore = 10;
    public int noteFinishScore = 30;


    public List<Mp3AndJson> mp3AndJsons = new List<Mp3AndJson>();
    Mp3AndJson currentMp3AndJsons;

    int nextNoteObject = 0;
    int nextNoteObjectPool = 0;

    int poolObjectSize = 20;

    void Start()
    {
        gameScreenManager = GameObject.FindGameObjectWithTag("GameScreenManager");
        gameScreenManagerScript = gameScreenManager.GetComponent<GameScreenManager>();
        randomManager = gameScreenManagerScript.GetNewRandomManager();

        game2Management = this;
            audioSource1.volume = PlayerPrefs.GetFloat("Setting_SoundFx");

        SetLinePositions();
        audioSource1.Stop();

        previousoffTick = objectLinePoisitons[0].gameObject;

        noteVelocity = 1 / lowestDuration;

        currentMp3AndJsons = mp3AndJsons[0];//UnityEngine.Random.Range(0, mp3AndJsons.Count)];

        audioSource1.clip = currentMp3AndJsons.mp3;

        JsonUtility.FromJsonOverwrite(currentMp3AndJsons.json.text, noteEvents);

        for (int i = 0; i < 10; i++)
        {
            updateNoteObjectPool();
        }
    }

    public void updateNoteObjectPool()
    {
        if (nextNoteObject >= noteEvents.noteEvents.Count)
            nextNoteObject = 0;

        SpawnNoteObject((int)randomManager.RandomGenerate(0, 4), new NoteEventPrivate(NoteObjects.Count, noteEvents.noteEvents[nextNoteObject].onTick, noteEvents.noteEvents[nextNoteObject].offTick));

        nextNoteObject += 1;
    }
    void Update()
    {
        previousClickTime += Time.deltaTime;
        Physics2D.SyncTransforms();
    }

    void SetLinePositions()
    {
        for (int i = 0; i < objectLinePoisitons.Length; i++)
        {
            noteLinePoisitons[i] = objectLinePoisitons[i].localPosition;
        }
    }

    public void playClip(NoteEventPrivate noteEvent)
    {
        audioSource1.time = noteEvent.onTick - (noteEvent.onTick != 0 ? noteDurationTolerance : 0);
        audioSource1.Play();
        audioSource1.SetScheduledEndTime(AudioSettings.dspTime + (noteEvent.offTick - noteEvent.onTick));
    }
    void SpawnNoteObject(int spawnLine, NoteEventPrivate noteEvent)
    {
        noteEvent.line = spawnLine;
        float clipLength = noteEvent.duration / lowestDuration;

        if (clipLength < 1.3f)
        {
            clipLength = 1.3f;

        }

        float spawnY;
        if (previousoffTick.transform.childCount > 0)
        {
            spawnY = previousoffTick.transform.position.y + previousoffTick.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        }
        else
        {
            spawnY = previousoffTick.transform.position.y;
        }

        GameObject noteObject = Instantiate(noteObjectPrefab, new Vector3(noteLinePoisitons[spawnLine].x, spawnY, 0), Quaternion.identity) as GameObject;
        NoteObjects.Add(noteObject);

        previousoffTick = noteObject;

        noteObject.transform.SetPositionAndRotation(new Vector3(noteLinePoisitons[spawnLine].x, spawnY, 0), Quaternion.identity);

        noteObject.GetComponent<NoteObject>().StartNoteObject(noteEvent);

        float sizeClipLength = clipLength - (clipLengthTolerance / lowestDuration);
        if (sizeClipLength < 1.3f)
        {
            sizeClipLength = 1.3f;
        }

        noteObject.GetComponent<SpriteRenderer>().size = new Vector2(noteObject.GetComponent<SpriteRenderer>().size.x, sizeClipLength);

        noteObject.transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector2(noteObject.transform.GetChild(0).GetComponent<BoxCollider2D>().size.x, clipLength);
        noteObject.transform.GetChild(0).GetComponent<BoxCollider2D>().offset = new Vector2(noteObject.transform.GetChild(0).GetComponent<BoxCollider2D>().offset.x, noteObject.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y / 2);

    }

    public void ClickedOnNote(NoteEventPrivate clickedNoteEvent)
    {
        if (currentNote != clickedNoteEvent.number)
        {
            int difference = clickedNoteEvent.number - currentNote;
            Debug.Log("eşit değil fark " + difference);

            for (int i = 0; i < difference; i++)
            {
                if (NoteObjects[i + currentNote].gameObject != null)
                {
                    NoteObjects[i + currentNote].GetComponent<NoteObject>().noteEvent.state = NoteEventPrivate.State.missed;
                    NoteObjects[i + currentNote].GetComponent<NoteObject>().MissedNoteGUI();
                }
            }
        }

        double timeDifference = previousDuration - previousClickTime;
        previousDuration = clickedNoteEvent.duration;
        previousClickTime = 0;

        ScorePanelDisplay(timeDifference);

        currentNote = clickedNoteEvent.number;
        playClip(clickedNoteEvent);
    }

    void ScorePanelDisplay(double timeDifference)
    {
        int score = 0;

        if (Convert.ToBoolean(PlayerPrefs.GetInt("Setting_Vibration")))
            MMVibrationManager.Haptic(HapticTypes.RigidImpact);

        scorePanel.SetActive(true);
        scorePanel.transform.GetComponent<Animator>().Play("ScoreEffectAnimation", -1, 0);
        //scorePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Math.Round(timeDifference, 3).ToString();

        timeDifference = Math.Abs(timeDifference);
        if (timeDifference < perfectTime && timeDifference >= 0)
        {
            scorePanel.transform.GetChild(0).gameObject.SetActive(true);
            scorePanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            scorePanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            scorePanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);

            score = perfectScore;
        }
        if (timeDifference < greatTime && timeDifference >= perfectTime)
        {
            scorePanel.transform.GetChild(0).gameObject.SetActive(true);
            scorePanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            scorePanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            scorePanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);

            score = greatScore;
        }
        if (timeDifference < niceTime && timeDifference >= greatTime)
        {
            scorePanel.transform.GetChild(0).gameObject.SetActive(true);
            scorePanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            scorePanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            scorePanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);

            score = niceScore;
        }

        if (timeDifference >= niceTime)
        {
            scorePanel.transform.GetChild(0).gameObject.SetActive(false);
            scorePanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            scorePanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            scorePanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);

            score = ordinaryScore;
        }
        scorePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + score;
        gameScreenManagerScript.playerScoreAdd(score);
    }
    public void NoteFinishScore()
    {
        gameScreenManagerScript.playerScoreAdd(noteFinishScore);
    }
}
