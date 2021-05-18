using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteFinishEffectScript : MonoBehaviour
{
    public TextMeshProUGUI finishScore;
    void Start()
    {
        finishScore.text = "+" + Game2Management.game2Management.noteFinishScore.ToString();
    }

}
