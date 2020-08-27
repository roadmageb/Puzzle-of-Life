using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditButton : SelectButton
{
    public Sprite sprUnClicked2;
    public Sprite sprClicked2;

    bool edit = true;

    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.EditSelected(edit);
        AudioManager.Inst.ButtonClicked();
        if (edit) { edit = false; }
        else { edit = true; }
        SwapSprite();
    }

    void SwapSprite()
    {
        Sprite temp;
        temp = sprClicked;
        sprClicked = sprClicked2;
        sprClicked2 = temp;
        temp = sprUnClicked;
        sprUnClicked = sprUnClicked2;
        sprUnClicked2 = temp;
    }
}
