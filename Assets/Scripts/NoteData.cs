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
    public Sprite icon;
    

    /// <summary>
    /// Attributes for note projectiles. Each has a unique effect.
    /// </summary>
    public enum AttributeType
    {
        /// <summary>Creates an explosion on impact</summary>
        Reverb,
        /// <summary>Enhances the next note to deal double damage, stacks if next is harmonic</summary>
        Harmonic,
        /// <summary>Bonus damage per note of matching type</summary>
        Symponic,
        /// <summary>Traps enemies hit in place</summary>
        Dissonance,
        /// <summary>Temporarily disrupts the enemy’s rhythm, slowing their actions in bursts</summary>
        Syncopate,
        /// <summary>Increases damage by the number of legato notes in your music</summary>
        Legato,
        /// <summary>This note deals double damage</summary>
        Forte,
        /// <summary>Shoots 2 notes instead of 1</summary>
        Staccato,
        /// <summary>Shoots 3 notes instead of 1</summary>
        Trio,
        /// <summary>Shoots 4 notes instead of 1</summary>
        Quartet,
        /// <summary>Shoots 5 notes instead of 1</summary>
        Quintet,
        /// <summary>Speeds up the movement of the note, making it travel faster</summary>
        Accelerando,
        /// <summary>Heals you for a portion of the damage dealt with this note</summary>
        Melody,
        /// <summary>Increases your movement speed temporarily after hitting an enemy</summary>
        Cadence
    }

    public enum Elements
    {
        Fire,
        Water,
        Lightining
    }

    public override bool Equals(object obj)
    {
        if (obj is NoteData other)
        {
            return noteName == other.noteName && level == other.level;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (noteName + level).GetHashCode();
    }
}