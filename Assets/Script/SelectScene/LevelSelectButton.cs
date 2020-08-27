using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class LevelSelectButton : SelectButton
{
    public SpriteRenderer Light;
    public Sprite LightOff;
    public GameObject Cell;

    private int LevelToGo;
    private bool possible;

    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.LevelSelected(LevelToGo);
        if (possible)
        {
            AudioManager.Inst.ButtonClicked();
        }
        else
        {
            AudioManager.Inst.ButtonCantBeClicked();
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
