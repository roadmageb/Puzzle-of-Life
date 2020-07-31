using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButtonController : MonoBehaviour
{
    public InGameButton[] button;
    private float time;

    public void SetButtonForPlayState(PlayState playState)
    {
        switch (playState)
        {
            case PlayState.EDIT:
                button[0].ChangeButtonState(ButtonState.PLAY);
                button[1].ChangeButtonState(ButtonState.PLAYFRAME);
                button[2].ChangeButtonState(ButtonState.RESETGRAY);
                break;
            case PlayState.EDITTOINIT:
                button[0].ChangeButtonState(ButtonState.PLAY);
                button[1].ChangeButtonState(ButtonState.PLAYFRAME);
                button[2].ChangeButtonState(ButtonState.RESETRED);
                time = 1.0f;
                break;
            case PlayState.PLAY:
                button[0].ChangeButtonState(ButtonState.FASTFORWARD);
                button[1].ChangeButtonState(ButtonState.PAUSE);
                button[2].ChangeButtonState(ButtonState.STOP);
                break;
            case PlayState.PLAYFRAME:
                button[0].ChangeButtonState(ButtonState.PLAY);
                button[1].ChangeButtonState(ButtonState.PLAYFRAME);
                button[2].ChangeButtonState(ButtonState.STOP);
                break;
        }
    }

    private void Start()
    {
        LevelManager.Inst.SetPlayState(PlayState.EDIT);
        SetButtonForPlayState(PlayState.EDIT);
        time = 0.0f;
    }

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else if (LevelManager.Inst.GetPlayState() == PlayState.EDITTOINIT)
        {
            time = 0.0f;
            LevelManager.Inst.SetPlayState(PlayState.EDIT);
            SetButtonForPlayState(PlayState.EDIT);
        }
    }
}
