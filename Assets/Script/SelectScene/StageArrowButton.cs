using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageArrowButton : MonoBehaviour
{
    public Sprite sprUnClicked;
    public Sprite sprClicked;


    private void OnMouseUpAsButton()
    {
        //StageManager.Inst.StageSelected(stage_to_go);
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
