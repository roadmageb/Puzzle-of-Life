using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButton : MonoBehaviour
{
    private ButtonState buttonState { get; set; }

    public void ChangeButtonState(ButtonState buttonState)
    {
        this.buttonState = buttonState;
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.buttonSprites[(int)buttonState * 2];
    }

    private void OnMouseDown()
    {
        if (LevelManager.Inst.GetPlayState() == PlayState.ERROR && buttonState != ButtonState.STOP)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.buttonSprites[(int)buttonState * 2 + 1];
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.buttonSprites[(int)buttonState * 2];
    }

    private void OnMouseUpAsButton()
    {
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.buttonSprites[(int)buttonState * 2];
        switch (buttonState)
        {
            case ButtonState.PLAY:
                if (LevelManager.Inst.GetPlayState() == PlayState.ERROR)
                {
                    return;
                }
                if (LevelManager.Inst.PlayLevel())
                {
                    LevelManager.Inst.SetPlayState(PlayState.PLAY);
                }
                else
                {
                    LevelManager.Inst.SetPlayState(PlayState.ERROR);
                }
                break;
            case ButtonState.FASTFORWARD:
                LevelManager.Inst.FastForwardLevel();
                LevelManager.Inst.SetPlayState(PlayState.PLAY);
                AudioManager.Inst.ButtonClicked();
                break;
            case ButtonState.PAUSE:
                LevelManager.Inst.PauseLevel();
                LevelManager.Inst.SetPlayState(PlayState.PLAYFRAME);
                AudioManager.Inst.ButtonClicked();
                break;
            case ButtonState.PLAYFRAME:
                if (LevelManager.Inst.GetPlayState() == PlayState.ERROR)
                {
                    return;
                }
                if (LevelManager.Inst.PlayFrame())
                {
                    LevelManager.Inst.SetPlayState(PlayState.PLAYFRAME);
                }
                else
                {
                    LevelManager.Inst.SetPlayState(PlayState.ERROR);
                }
                break;
            case ButtonState.RESETGRAY:
                LevelManager.Inst.SetPlayState(PlayState.EDITTOINIT);
                AudioManager.Inst.ButtonClicked();
                break;
            case ButtonState.RESETRED:
                LevelManager.Inst.MapReset();
                LevelManager.Inst.SetPlayState(PlayState.EDIT);
                LevelManager.Inst.topBoardController.ChangeResetTime();
                AudioManager.Inst.PuzzleReset();
                break;
            case ButtonState.STOP:
                LevelManager.Inst.StopLevel();
                LevelManager.Inst.SetPlayState(PlayState.EDIT);
                AudioManager.Inst.ButtonClicked();
                break;
        }
    }
}
