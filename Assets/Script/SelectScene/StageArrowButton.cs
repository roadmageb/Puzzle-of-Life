using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageArrowButton : MonoBehaviour
{
    public Sprite sprUnClicked;
    public Sprite sprClicked;


    private void OnMouseUpAsButton()
    {
        SelectSceneManager.Inst.StageChange((int)GetComponent<Transform>().localScale.x);
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
