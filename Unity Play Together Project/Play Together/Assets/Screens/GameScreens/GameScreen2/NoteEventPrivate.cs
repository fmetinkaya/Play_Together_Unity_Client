using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEventPrivate
{
    public enum State
    {
        ready,
        tapped,
        oldtapped,
        missed
    }
    public int number { get; set; }
    public float onTick { get; set; }
    public float offTick { get; set; }
    public float duration { get; set; }
    public State state { get; set; }
    public int line { get; set; }

    public NoteEventPrivate(int number, float onTick, float offTick)
    {
        this.number = number;
        this.onTick = onTick;
        this.offTick = offTick;
        this.duration = offTick - onTick;
        this.state = State.ready;
    }
}
