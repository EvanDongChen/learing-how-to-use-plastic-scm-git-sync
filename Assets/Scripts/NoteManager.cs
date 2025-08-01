using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
{
    public List<NoteData> noteData;

    //temp prefab
    public GameObject notePrefab;

    [SerializeField] private Canvas mainCanvas;

    private void Start()
    {

    }
}