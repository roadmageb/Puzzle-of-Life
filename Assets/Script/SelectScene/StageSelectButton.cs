using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public Sprite sprUnClicked;
    public Sprite sprClicked;

    public int StageToGo;

    private void OnMouseUpAsButton()
    {
        StageManager.Inst.StageSelected(StageToGo);
    }

    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().sprite = sprClicked;
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sprite = sprUnClicked;
    }
}
