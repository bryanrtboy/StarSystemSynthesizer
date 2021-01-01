using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Note : ScriptableObject
{
    public GameObject synth;
    public NoteValue note = NoteValue.C;
    public int octave = -1;
    public int pentatonicValue = -1;
    public float velocity = 1;
    public float length = 1;
    public float size = .1f;
}

[System.Serializable]
public enum NoteValue { C, CSharp, D, DSharp, E, F, FSharp, G, GSharp, A, ASharp, B, Random };