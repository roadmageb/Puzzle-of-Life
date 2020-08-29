using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPaletteButtonController : MonoBehaviour
{
    public bool cellFlag { get; set; } // true = cell edit mode의 palette, false = replacement edit mode의 palette
    public bool isReplaceable { get; set; }
    public Cell cellType { get; set; }
    public int index;
    private EditorPaletteController editorPaletteController;

    private void OnMouseUpAsButton()
    {
        editorPaletteController.ButtonClicked(index);
    }

    private void Start()
    {
        editorPaletteController = GetComponentInParent<EditorPaletteController>();
    }
}
