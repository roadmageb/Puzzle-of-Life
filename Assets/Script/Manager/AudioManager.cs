using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource sndPuzzleClear;
    public AudioSource sndPuzzleReset;
    public AudioSource sndGenerationReplacement;
    public AudioSource sndCellUp;
    public AudioSource sndCellDown;
    public AudioSource sndButtonClicked;
    public AudioSource sndPuzzlePlay;
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
    public void CellUp()
    {
        sndCellUp.Play();
    }
    public void CellDown()
    {
        sndCellDown.Play();
    }
    public void ButtonClicked()
    {
        sndButtonClicked.Play();
    }
    public void PuzzlePlay()
    {
        sndPuzzlePlay.Play();
    }
}