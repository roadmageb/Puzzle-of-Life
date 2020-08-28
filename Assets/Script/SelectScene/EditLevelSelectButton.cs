using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditLevelSelectButton : SelectButton
{
    public SpriteRenderer Light;
    public GameObject Spanner;
    public Sprite sprUnClickable;

    private int LevelToGo;
    private bool IsPlayable;
    private bool Edit = true;

    Sprite sprUnClicked_s;
    Sprite sprClicked_s;

    public void Start()
    {
        sprUnClicked_s = sprUnClicked;
        sprClicked_s = sprClicked;
    }

    protected override void ButtonAction()
    {
        GameManager.Inst.editNum = LevelToGo;
        SceneManager.LoadScene("LevelEditor");
        if (Edit)
        {
            AudioManager.Inst.ButtonClicked();
        }
        else
        {
            if (IsPlayable)
            {
                AudioManager.Inst.ButtonClicked();
            }
            else
            {
                AudioManager.Inst.ButtonCantBeClicked();
            }
        }
    }

    public void SetEditLevelSelectButton(int Level, bool _IsPlayable)
    {
        LevelToGo = Level;
        IsPlayable = _IsPlayable;
    }

    public void EditModeChange(bool _Edit)
    {
        Edit = _Edit;
        if (Edit)
        {
            Spanner.SetActive(true);
            if (!IsPlayable)
            {
                Light.sprite = sprUnClicked_s;
                sprUnClicked = sprUnClicked_s;
                sprClicked = sprClicked_s;
            }
        }
        else
        {
            Spanner.SetActive(false);
            if (!IsPlayable)
            {
                Light.sprite = sprUnClickable;
                sprUnClicked = sprUnClickable;
                sprClicked = sprUnClickable;
            }
        }
    }
}
