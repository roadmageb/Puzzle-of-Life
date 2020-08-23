using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPaletteController : MonoBehaviour
{
    public EditorPaletteButtonController editorPaletteButtonPrefab;
    private EditorPaletteButtonController[] editorPaletteButtons;
    private SpriteRenderer editorPaletteButtonSelected;
    public LevelEditor levelEditor;

    private void Start()
    {
        editorPaletteButtons = new EditorPaletteButtonController[11];
        for (int i = 0; i < 11; ++i)
        {
            editorPaletteButtons[i] = Instantiate(editorPaletteButtonPrefab, transform).GetComponent<EditorPaletteButtonController>();
            editorPaletteButtons[i].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.editorButtonSprites[i];
            editorPaletteButtons[i].index = i;
            editorPaletteButtons[i].transform.localPosition = new Vector2(i, 0);
            if (i < 9)
            {
                editorPaletteButtons[i].cellFlag = true;
                switch (i)
                {
                    case 0:
                        editorPaletteButtons[i].cellType = (Cell)i;
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        editorPaletteButtons[i].cellType = (Cell)i + 2;
                        break;
                    case 7:
                    case 8:
                        editorPaletteButtons[i].cellType = (Cell)i - 6;
                        break;
                }
            }
            else if (i == 9)
            {
                editorPaletteButtons[i].cellFlag = false;
                editorPaletteButtons[i].isReplaceable = true;
            }
            else if (i == 10)
            {
                editorPaletteButtons[i].cellFlag = false;
                editorPaletteButtons[i].isReplaceable = false;
            }
        }
        editorPaletteButtonSelected = Instantiate(new GameObject().AddComponent<SpriteRenderer>(), transform).GetComponent<SpriteRenderer>();
        editorPaletteButtonSelected.sprite = ImageManager.Inst.editorButtonSelectedSprite;
        editorPaletteButtonSelected.sortingOrder = 1;
        editorPaletteButtonSelected.transform.localPosition = new Vector2(0, 0);
    }

    public void ButtonClicked(int index)
    {
        editorPaletteButtonSelected.transform.localPosition = new Vector2(index, 0);
        levelEditor.editMode = editorPaletteButtons[index].cellFlag;
        if (editorPaletteButtons[index].cellFlag)
        {
            levelEditor.selectedCell = editorPaletteButtons[index].cellType;
        }
        else
        {
            levelEditor.selectedReplaceability = editorPaletteButtons[index].isReplaceable;
            Debug.Log(editorPaletteButtons[index].isReplaceable);
        }
    }
}
