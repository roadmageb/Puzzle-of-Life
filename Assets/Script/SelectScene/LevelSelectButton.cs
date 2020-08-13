using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class LevelSelectButton : SelectButton
{
    public SpriteRenderer Light;
    public Sprite LightOff;
    public GameObject Cell;

    private int StageToGo;
    private int LevelToGo;

    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.LevelSelected(StageToGo, LevelToGo);
    }

    public void SetLight(int STG, int LTG)
    {
        StageToGo = STG;
        LevelToGo = LTG;
        if (GameManager.Inst.LevelClearData.IsClear[StageToGo - 1, LevelToGo - 1] == false)
        {
            Cell.SetActive(false);
            int last_stage = StageToGo;
            int last_level = LevelToGo - 1;
            if (last_level == 0)
            {
                last_level = 10;
                last_stage -= 1;
                if (last_stage == 0)
                {
                    return;
                }
            }
            if (GameManager.Inst.LevelClearData.IsClear[last_stage - 1, last_level - 1] == false)
            {
                Light.sprite = LightOff;
                sprClicked = LightOff;
                sprUnClicked = LightOff;
            }
        }
    }
}
