using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NoteEvent
{
    public enum State
    {
        ready,
        tapped,
        oldtapped,
        missed
    }
    public int number;
    public float onTick;
    public float offTick;
    public float duration;
    public State state;
    public int line;
    public NoteEvent(int number, float onTick, float offTick)
    {
        this.number = number;
        this.onTick = onTick;
        this.offTick = offTick;
        this.duration = (offTick - onTick);
        this.state = State.ready;
    }
}
[System.Serializable]
public class NoteEvents
{
    public List<NoteEvent> noteEvents = new List<NoteEvent>();

}