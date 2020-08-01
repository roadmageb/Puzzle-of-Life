using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public Sprite sprUnClicked;
    public Sprite sprClicked;

    public int LevelToGo;

    private void OnMouseUpAsButton()
    {
        SelectSceneManager.Inst.LevelSelected(LevelToGo);
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
