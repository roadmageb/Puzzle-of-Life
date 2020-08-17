using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class LevelSelectButton : SelectButton
{
    public SpriteRenderer Light;
    public Sprite LightOff;
    public GameObject Cell;

    public AudioSource ClickSound;
    public AudioSource UnClickable;

    private int LevelToGo;
    private bool possible;

    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.LevelSelected(LevelToGo);
        if (possible)
        {
            ClickSound.Play();
        }
        else
        {
            UnClickable.Play();
        }
    }

    public void SetLevelSelectButton(int Level, bool IsCleared, bool IsPlayable)
    {
        possible = IsPlayable;
        LevelToGo = Level;
        if (IsCleared == false)
        {
            Cell.SetActive(false);
            if (IsPlayable == false)
            {
                Light.sprite = LightOff;
                sprClicked = LightOff;
                sprUnClicked = LightOff;
            }
        }
    }
}
