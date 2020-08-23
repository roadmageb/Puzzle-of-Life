using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapResizerController : MonoBehaviour
{
    private bool mouseDownFlag;
    private int value;
    private Vector2 oldCoord, newCoord;
    private Vector2 offset;
    public void SetResizerType(int value) // 0 = TopLeft, 1 = TopRight, 2 = BottomLeft, 3 = BottomRight
    {
        this.value = value;
        Vector2 offset = new Vector2((LevelManager.Inst.currentLevel.size.x - 1) / 2.0f, (LevelManager.Inst.currentLevel.size.y - 1) / 2.0f);
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapResizerSprites[value];
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

    private void ChangeMapSize()
    {
        Cell[,] tempMap = new Cell[LevelManager.Inst.currentLevel.size.x, LevelManager.Inst.currentLevel.size.y];
        bool[,] tempIsReplaceable = new bool[LevelManager.Inst.currentLevel.size.x, LevelManager.Inst.currentLevel.size.y];
        for (int i = 0; i < LevelManager.Inst.currentLevel.size.x; ++i)
        {
            for (int j = 0; j < LevelManager.Inst.currentLevel.size.y; ++j)
            {
                tempMap[i, j] = LevelManager.Inst.currentLevel.map[i, j];
                tempIsReplaceable[i, j] = LevelManager.Inst.currentLevel.isReplaceable[i, j];
            }
        }

        Vector2Int size = LevelManager.Inst.currentLevel.size;
        Vector2Int diff = new Vector2Int((int)(newCoord - oldCoord).x, (int)(newCoord - oldCoord).y);
        switch (value)
        {
            case 0:
                size.x -= diff.x;
                size.y += diff.y;
                break; 
            case 1:
                size.x += diff.x;
                size.y += diff.y;
                break;
            case 2:
                size.x -= diff.x;
                size.y -= diff.y;
                break; 
            case 3:
                size.x += diff.x;
                size.y -= diff.y;
                break;

        }
        
        if (size.x <= 0 || size.y <= 0)
        {
            return;
        }

        Cell[,] map = new Cell[size.x, size.y];
        bool[,] isReplaceable = new bool[size.x, size.y];

        for (int i = 0; i < size.x; ++i)
        {
            for (int j = 0; j < size.y; ++j)
            {
                map[i, j] = Cell.NULL;
                isReplaceable[i, j] = false;
            }
        }

        switch (value)
        {
            case 0:
                for (int i = Mathf.Min(size.x, LevelManager.Inst.currentLevel.size.x) - 1; i >= 0; --i)
                {
                    for (int j = Mathf.Min(size.y, LevelManager.Inst.currentLevel.size.y) - 1; j >= 0; --j)
                    {
                        map[i - Mathf.Min(diff.x, 0), j + Mathf.Max(diff.y, 0)] = tempMap[i + Mathf.Max(diff.x, 0), j - Mathf.Min(diff.y, 0)];
                        isReplaceable[i - Mathf.Min(diff.x, 0), j + Mathf.Max(diff.y, 0)] = tempIsReplaceable[i + Mathf.Max(diff.x, 0), j - Mathf.Min(diff.y, 0)];
                    }
                }
                break;
            case 1:
                for (int i = 0; i < Mathf.Min(size.x, LevelManager.Inst.currentLevel.size.x); ++i)
                {
                    for (int j = Mathf.Min(size.y, LevelManager.Inst.currentLevel.size.y) - 1; j >= 0; --j)
                    {
                        map[i, j + Mathf.Max(diff.y, 0)] = tempMap[i, j - Mathf.Min(diff.y, 0)];
                        isReplaceable[i, j + Mathf.Max(diff.y, 0)] = tempIsReplaceable[i, j - Mathf.Min(diff.y, 0)];
                    }
                }
                break;
            case 2:
                for (int i = Mathf.Min(size.x, LevelManager.Inst.currentLevel.size.x) - 1; i >= 0; --i)
                {
                    for (int j = 0; j < Mathf.Min(size.y, LevelManager.Inst.currentLevel.size.y); ++j)
                    {
                        map[i - Mathf.Min(diff.x, 0), j] = tempMap[i + Mathf.Max(diff.x, 0), j];
                        isReplaceable[i - Mathf.Min(diff.x, 0), j] = tempIsReplaceable[i + Mathf.Max(diff.x, 0), j];
                    }
                }
                break;
            case 3:
                for (int i = 0; i < Mathf.Min(size.x, LevelManager.Inst.currentLevel.size.x); ++i)
                {
                    for (int j = 0; j < Mathf.Min(size.y, LevelManager.Inst.currentLevel.size.y); ++j)
                    {
                        map[i, j] = tempMap[i, j];
                        isReplaceable[i, j] = tempIsReplaceable[i, j];
                    }
                }
                break;
        }

        LevelManager.Inst.currentLevel.size = size;
        LevelManager.Inst.currentLevel.map = map;
        LevelManager.Inst.currentLevel.isReplaceable = isReplaceable;
    }

    private float Round(float f, bool flag) // true면 일반적인 round를, half면 0.5를 기준으로 round를 반환
    {
        if (flag)
        {
            return Mathf.Round(f);
        }
        else
        {
            return Mathf.Round(f - 0.5f) + 0.5f;
        }
    }

    private void OnMouseDown()
    {
        mouseDownFlag = true;
        oldCoord = new Vector2(transform.localPosition.x, transform.localPosition.y);
        offset = new Vector2(0, 0);
        if (LevelManager.Inst.currentLevel.size.x % 2 == 0)
        {
            offset.x = 0.5f;
        }
        if (LevelManager.Inst.currentLevel.size.y % 2 == 0)
        {
            offset.y = 0.5f;
        }
    }

    private void OnMouseUp()
    {
        mouseDownFlag = false;
        newCoord = new Vector2(transform.localPosition.x, transform.localPosition.y);
        ChangeMapSize();
        LevelManager.Inst.MapInstantiate();
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
                    transform.localPosition = new Vector2(Mathf.Round(coord.x + 0.484375f - offset.x) + offset.x, Mathf.Round(coord.y - 0.515625f - offset.y) + offset.y);
                    break;
                case 1:
                    transform.localPosition = new Vector2(Mathf.Round(coord.x - 0.484375f - offset.x) + offset.x, Mathf.Round(coord.y - 0.515625f - offset.y) + offset.y);
                    break;
                case 2:
                    transform.localPosition = new Vector2(Mathf.Round(coord.x + 0.484375f - offset.x) + offset.x, Mathf.Round(coord.y + 0.453125f - offset.y) + offset.y);
                    break;
                case 3:
                    transform.localPosition = new Vector2(Mathf.Round(coord.x - 0.484375f - offset.x) + offset.x, Mathf.Round(coord.y + 0.453125f - offset.y) + offset.y);
                    break;
            }
        }
    }
}
