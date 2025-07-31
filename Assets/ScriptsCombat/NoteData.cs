using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NoteData", menuName = "Scriptable Objects/NoteData")]
public class NoteData : ScriptableObject
{
    public string noteName;
    public int rarity;
    public int level;
    public float noteDuration;
    public Elements element;
    public List<AttributeType> attributes;

    public enum AttributeType
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
        Cadence
    }

    public enum Elements
    {
        Fire,
        Water,
        Lightining
    }
}
