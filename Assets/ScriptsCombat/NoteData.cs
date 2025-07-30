using UnityEngine;

[CreateAssetMenu(fileName = "NoteData", menuName = "Scriptable Objects/NoteData")]
public class NoteData : ScriptableObject
{
    public string noteName;
    public int rarity;
    public int level;
    public float noteDuration;
    public Elements element;
    public Attributes attribute;


    public enum Attributes
    {
        Reverb,
        Harmonic,
        Dissonance,
        Syncopate,
        Legato,
        Forte,
        Staccato,
        Tremolo,
        Accelerando,
        Melody,
        Cadence,
    }

    public enum Elements
    {
        Fire,
        Water,
        Lightining
    }
}
