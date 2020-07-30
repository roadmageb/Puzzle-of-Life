using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BackSelectButton  : MonoBehaviour
{
    public Sprite sprUnClicked;
    public Sprite sprClicked;

    private void OnMouseUpAsButton()
    {
        StageManager.Inst.BackSelected();
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
