using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButton : MonoBehaviour
{
    private ButtonState buttonState { get; set; }
    private bool isButtonDown;

    public void ChangeButtonState(ButtonState buttonState)
    {
        this.buttonState = buttonState;
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.buttonSprites[(int)buttonState * 2];
    }

    private void OnMouseDown()
    {
        isButtonDown = true;
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.buttonSprites[(int)buttonState * 2 + 1];
    }

    private void OnMouseExit()
    {
        isButtonDown = false;
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.buttonSprites[(int)buttonState * 2];
    }

    private void OnMouseUp()
    {
        if (isButtonDown)
        {
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.buttonSprites[(int)buttonState * 2];
            switch (buttonState)
            {
                case ButtonState.PLAY:
                    LevelManager.Inst.PlayLevel();
                    LevelManager.Inst.SetPlayState(PlayState.PLAY);
                    break;
                case ButtonState.FASTFORWARD:
                    LevelManager.Inst.FastForwardLevel();
                    LevelManager.Inst.SetPlayState(PlayState.PLAY);
                    break;
                case ButtonState.PAUSE:
                    LevelManager.Inst.PauseLevel();
                    LevelManager.Inst.SetPlayState(PlayState.PLAYFRAME);
                    break;
                case ButtonState.PLAYFRAME:
                    LevelManager.Inst.PlayFrame();
                    LevelManager.Inst.SetPlayState(PlayState.PLAYFRAME);
                    break;
                case ButtonState.RESETGRAY:
                    LevelManager.Inst.SetPlayState(PlayState.EDITTOINIT);
                    break;
                case ButtonState.RESETRED:
                    LevelManager.Inst.MapReset();
                    LevelManager.Inst.SetPlayState(PlayState.EDIT);
                    break;
                case ButtonState.STOP:
                    LevelManager.Inst.StopLevel();
                    LevelManager.Inst.SetPlayState(PlayState.EDIT);
                    break;
            }
        }
    }
}
