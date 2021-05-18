using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public SpriteRenderer TubeSR;
    public SpriteRenderer OutdoorOutlineSR;
    public SpriteRenderer BlurMiddleSR;
    public SpriteRenderer CircleCenterSR;

    public GameObject NoteEffectStart;
    public GameObject ClickedEffect;
    public GameObject NoteFinishEffectPrefab;
    public GameObject arrow;
    public GameObject clickedCenter;
    int clickPointerId = 0;
    public NoteEventPrivate noteEvent;
    void Start()
    {
        NoteEffectStart.GetComponent<ParticleSystem>().Stop();
        ClickedEffect.GetComponent<ParticleSystem>().Stop();
    }

    void Update()
    {
        if (noteEvent.state != NoteEventPrivate.State.tapped)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - Game2Management.game2Management.noteVelocity * Time.deltaTime);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().size = new Vector2(gameObject.GetComponent<SpriteRenderer>().size.x, gameObject.GetComponent<SpriteRenderer>().size.y - (Game2Management.game2Management.noteVelocity * Time.deltaTime));

            if (gameObject.GetComponent<SpriteRenderer>().size.y < arrow.GetComponent<SpriteRenderer>().size.y)
                arrow.GetComponent<SpriteRenderer>().size = new Vector2(arrow.GetComponent<SpriteRenderer>().size.x, gameObject.GetComponent<SpriteRenderer>().size.y);
            if (gameObject.GetComponent<SpriteRenderer>().size.y <= 1.3f)
            {
                GameObject noteFinishEffect = Instantiate(NoteFinishEffectPrefab, new Vector3(transform.position.x, transform.position.y + 0.65f, 0), Quaternion.identity);
                Game2Management.game2Management.currentNote = noteEvent.number + 1;
                Destroy(gameObject);
                Game2Management.game2Management.NoteFinishScore();
                Game2Management.game2Management.updateNoteObjectPool();
            }
        }

        if (Camera.main.orthographicSize < (-transform.localPosition.y - transform.GetComponent<SpriteRenderer>().bounds.size.y))
        {
            Destroy(gameObject);
            Game2Management.game2Management.updateNoteObjectPool();
        }

        if (Camera.main.orthographicSize < -transform.localPosition.y && noteEvent.state == NoteEventPrivate.State.ready)
        {
            noteEvent.state = NoteEventPrivate.State.missed;
            MissedNoteGUI();
            Game2Management.game2Management.currentNote = noteEvent.number + 1;
        }
    }
    public void StartNoteObject(NoteEventPrivate noteEvent)
    {
        this.noteEvent = noteEvent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (noteEvent.state == NoteEventPrivate.State.ready)
        {
            Game2Management.game2Management.ClickedOnNote(noteEvent);
            noteEvent.state = NoteEventPrivate.State.tapped;
            clickPointerId = eventData.pointerId;
            arrow.SetActive(true);
            clickedCenter.SetActive(true);


            if (noteEvent.line == 0)
            {
                ParticleSystem.ShapeModule sm = NoteEffectStart.GetComponent<ParticleSystem>().shape;
                sm.radiusMode = ParticleSystemShapeMultiModeValue.Loop;
                sm.radiusSpeed = 1.999f;
            }
            else if (noteEvent.line == 3)
            {
                ParticleSystem.ShapeModule sm = NoteEffectStart.GetComponent<ParticleSystem>().shape;
                sm.radiusMode = ParticleSystemShapeMultiModeValue.Loop;
                sm.radiusSpeed = 0;
            }
            else
            {
                NoteEffectStart.GetComponent<ParticleSystem>().Simulate(Random.Range(0, 2) == 0 ? 1f : 1.5f, true, true);
            }
            NoteEffectStart.GetComponent<ParticleSystem>().Play();
            ClickedEffect.GetComponent<ParticleSystem>().Play();

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (clickPointerId == eventData.pointerId)
        {
            noteEvent.state = NoteEventPrivate.State.oldtapped;
            arrow.SetActive(false);
            clickedCenter.SetActive(false);
            NoteEffectStart.GetComponent<ParticleSystem>().Stop();
            ClickedEffect.GetComponent<ParticleSystem>().Stop();

            MissedNoteGUI();
            Game2Management.game2Management.currentNote = noteEvent.number + 1;
        }
    }

    public void MissedNoteGUI()
    {
        float alpha = 0.4f;
        TubeSR.color = new Color(1, 1, 1, alpha);
        BlurMiddleSR.color = new Color(1, 1, 1, alpha);
        CircleCenterSR.color = new Color(1, 1, 1, alpha);
        OutdoorOutlineSR.color = new Color(1, 1, 1, alpha);
    }

}
