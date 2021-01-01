using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AudioHelm;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class SendNoteOnTrigger : MonoBehaviour
{

    public PentatonicScale m_pentatonic;
    //public HelmController m_sequencer;

    //public Waypoint m_waypointData;
    public float velocity, length;
    public int octave, pentatonicValue;

    public NoteValue m_root = NoteValue.Random;


    public int[] m_midiValues;
    public int m_rootMidiValue = 55;
    public int currentOctave = 1;
    public int m_pickedNote = 55;
    bool m_randomVelocity = false;
    bool m_randomLength = false;
    LineRenderer m_orbitLine;
    Color m_lineColor;

    private void Start()
    {
        if (octave < 0)
            currentOctave = Random.Range(1, 6);
        else
            currentOctave = octave;

        m_rootMidiValue = GetRootNote(m_root, currentOctave);
        m_midiValues = PentatonicArray(m_pentatonic, m_rootMidiValue);
        m_orbitLine = transform.GetComponentInParent<LineRenderer>();

        if (m_orbitLine)
            m_lineColor = m_orbitLine.startColor;

        if (pentatonicValue < 0)
            m_pickedNote = m_midiValues[Random.Range(0, m_midiValues.Length)];
        else
            m_pickedNote = m_midiValues[pentatonicValue];

        if (velocity < .001f)
            m_randomVelocity = true;

        if (length < .001f)
            m_randomLength = true;

    }


    void OnMouseDown()
    {
        Activate();

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Planet")
        {
            Activate();
        }
    }

    public void Activate()
    {

        //if (m_sequencer != null && m_sequencer.gameObject.activeSelf)
        //    NoteOn();

        if (m_orbitLine)
        {
            m_orbitLine.startColor = Color.white;
            m_orbitLine.endColor = Color.white;
            Invoke("ResetLineColor", length);
        }
    }

    void NoteOn()
    {
        if (m_midiValues == null)
            return;

        if (pentatonicValue < 0)
            m_pickedNote = m_midiValues[Random.Range(0, m_midiValues.Length)];

        if (m_randomVelocity)
            velocity = Random.Range(.01f, 1f);

        if (m_randomLength)
            length = Random.Range(.01f, 10f);

        //if (m_sequencer.IsNoteOn(m_pickedNote))
        //    m_sequencer.AllNotesOff();

        //m_sequencer.NoteOn(m_pickedNote, velocity, length);

    }

    void ResetLineColor()
    {
        if (m_orbitLine)
        {
            m_orbitLine.startColor = m_lineColor;
            m_orbitLine.endColor = m_lineColor;
        }
    }

    //If you don't set the length of a note, you must manually turn it off
    IEnumerator NoteOff(int note, float delay)
    {
        yield return new WaitForSeconds(delay);

        //m_sequencer.NoteOff(note);
    }

    int[] PentatonicArray(PentatonicScale pentatonic, int root)
    {
        int[] scale = new int[] { 0 };

        switch (pentatonic)
        {
            case PentatonicScale.Major:
                scale = new int[] { 0, 2, 4, 7, 9 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Minor:
                scale = new int[] { 0, 3, 5, 7, 10 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Dominant_7th:
                scale = new int[] { 0, 4, 5, 7, 10 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Dominant_7th_Sus2:
                scale = new int[] { 0, 2, 4, 7, 10 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Major_7th_Sus2:
                scale = new int[] { 0, 2, 4, 7, 11 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Major_7th_4th:
                scale = new int[] { 0, 4, 5, 7, 11 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Major_Sharp_4th:
                scale = new int[] { 0, 2, 4, 6, 9 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Minor_6th:
                scale = new int[] { 0, 3, 5, 7, 9 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Egyptian:
                scale = new int[] { 0, 2, 5, 7, 10 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Hirajoshi:
                scale = new int[] { 0, 2, 3, 7, 8 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.In:
                scale = new int[] { 0, 1, 5, 7, 8 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Insen:
                scale = new int[] { 0, 1, 5, 7, 10 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            case PentatonicScale.Iwato:
                scale = new int[] { 0, 1, 5, 6, 10 };
                for (int i = 0; i < scale.Length; i++)
                    scale[i] = scale[i] + root;
                break;
            default:
                scale = new int[] { 0, 2, 4, 7, 9 };
                break;
        }


        return scale;
    }

    int GetRootNote(NoteValue note, int octaveValue)
    {
        int root = 55;

        switch (note)
        {
            case NoteValue.C:
                root = 0 + (octaveValue * 12);
                break;
            case NoteValue.CSharp:
                root = 1 + (octaveValue * 12);
                break;
            case NoteValue.D:
                root = 2 + (octaveValue * 12);
                break;
            case NoteValue.DSharp:
                root = 3 + (octaveValue * 12);
                break;
            case NoteValue.E:
                root = 4 + (octaveValue * 12);
                break;
            case NoteValue.F:
                root = 5 + (octaveValue * 12);
                break;
            case NoteValue.FSharp:
                root = 6 + (octaveValue * 12);
                break;
            case NoteValue.G:
                root = 7 + (octaveValue * 12);
                break;
            case NoteValue.GSharp:
                root = 8 + (octaveValue * 12);
                break;
            case NoteValue.A:
                root = 9 + (octaveValue * 12);
                break;
            case NoteValue.ASharp:
                root = 10 + (octaveValue * 12);
                break;
            case NoteValue.B:
                root = 11 + (octaveValue * 12);
                break;
            case NoteValue.Random:
                root = Random.Range(0, 11) + (octaveValue * 12);
                break;
            default:
                root = 55;
                break;
        }

        return root;
    }

}

