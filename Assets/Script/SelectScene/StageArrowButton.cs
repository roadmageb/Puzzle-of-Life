using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageArrowButton : SelectButton
{
    public AudioSource ClickSound;
    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.StageChange((int)GetComponent<Transform>().localScale.x);
        ClickSound.Play();
    }
}
