using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource sndPuzzleClear;
    public AudioSource sndPuzzleReset;
    public AudioSource sndGenerationReplacement;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    public void PuzzleClear()
    {
        sndPuzzleClear.Play();
    }
    public void PuzzleReset()
    {
        sndPuzzleReset.Play();
    }
    public void GenerationReplacement()
    {
        sndGenerationReplacement.Play();
    }
}