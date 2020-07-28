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
                    GetComponentInParent<InGameButtonController>().ChangePlayState(PlayState.PLAY);
                    break;
                case ButtonState.FASTFORWARD:
                    LevelManager.Inst.FastForwardLevel();
                    GetComponentInParent<InGameButtonController>().ChangePlayState(PlayState.PLAY);
                    break;
                case ButtonState.PAUSE:
                    LevelManager.Inst.PauseLevel();
                    GetComponentInParent<InGameButtonController>().ChangePlayState(PlayState.PLAYFRAME);
                    break;
                case ButtonState.PLAYFRAME:
                    LevelManager.Inst.PlayFrame();
                    GetComponentInParent<InGameButtonController>().ChangePlayState(PlayState.PLAYFRAME);
                    break;
                case ButtonState.RESETGRAY:
                    GetComponentInParent<InGameButtonController>().ChangePlayState(PlayState.EDITTOINIT);
                    break;
                case ButtonState.RESETRED:
                    LevelManager.Inst.MapReset();
                    GetComponentInParent<InGameButtonController>().ChangePlayState(PlayState.EDIT);
                    break;
                case ButtonState.STOP:
                    LevelManager.Inst.StopLevel();
                    GetComponentInParent<InGameButtonController>().ChangePlayState(PlayState.EDIT);
                    break;
            }
        }
    }
}
