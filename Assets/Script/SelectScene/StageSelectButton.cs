using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public Sprite sprUnClicked;
    public Sprite sprClicked;

    public int stage_to_go;

    private void OnMouseUpAsButton()
    {
        StageManager.Inst.StageSelected(stage_to_go);
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
