using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public Sprite sprUnClicked;
    public Sprite sprClicked;

    public int level_to_go;

    private void OnMouseUpAsButton()
    {
        Debug.Log("level selected: " + level_to_go);
        GameManager.Inst.stage = level_to_go;
        SceneManager.LoadScene("PuzzleScene");
    }
    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().sprite = sprClicked;
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sprite = sprUnClicked;
    }

    private void Awake()
    {
    }
}
