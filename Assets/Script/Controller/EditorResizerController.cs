using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorResizerController : MonoBehaviour
{
    private bool mouseDownFlag;
    private int value;
    public void SetResizerType(int value) // 0 = TopLeft, 1 = TopRight, 2 = BottomLeft, 3 = BottomRight
    {
        this.value = value;
        Vector2 offset = new Vector2((LevelManager.Inst.currentLevel.size.x - 1) / 2.0f, (LevelManager.Inst.currentLevel.size.y - 1) / 2.0f);
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.editorResizerSprites[value];
        switch (value)
        {
            case 0:
                GetComponent<BoxCollider2D>().offset = new Vector2(-0.484375f, 0.515625f);
                transform.localPosition = new Vector2(-offset.x, offset.y);
                break;
            case 1:
                GetComponent<BoxCollider2D>().offset = new Vector2(0.484375f, 0.515625f);
                transform.localPosition = new Vector2(offset.x, offset.y);
                break;
            case 2:
                GetComponent<BoxCollider2D>().offset = new Vector2(-0.484375f, -0.453125f);
                transform.localPosition = new Vector2(-offset.x, -offset.y);
                break;
            case 3:
                GetComponent<BoxCollider2D>().offset = new Vector2(0.484375f, -0.453125f);
                transform.localPosition = new Vector2(offset.x, -offset.y);
                break;
        }
    }

    private void OnMouseDown()
    {
        mouseDownFlag = true;
    }

    private void OnMouseUp()
    {
        mouseDownFlag = false;
    }

    private void OnMouseDrag()
    {
        if (mouseDownFlag)
        {
            Vector2 currentMouseCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 coord = transform.parent.InverseTransformPoint(currentMouseCoord);
            switch (value)
            {
                case 0:
                    transform.localPosition = new Vector2(Mathf.Round(coord.x + 0.484375f), Mathf.Round(coord.y - 0.515625f));
                    break;
                case 1:
                    transform.localPosition = new Vector2(Mathf.Round(coord.x - 0.484375f), Mathf.Round(coord.y - 0.515625f));
                    break;
                case 2:
                    transform.localPosition = new Vector2(Mathf.Round(coord.x + 0.484375f), Mathf.Round(coord.y + 0.453125f));
                    break;
                case 3:
                    transform.localPosition = new Vector2(Mathf.Round(coord.x - 0.484375f), Mathf.Round(coord.y + 0.453125f));
                    break;
            }
        }
    }
}
