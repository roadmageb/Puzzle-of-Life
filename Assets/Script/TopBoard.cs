using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBoard : MonoBehaviour
{
    public BoardState boardState;
    private bool isButtonDown;
    public Transform rule;
    private float timeMenu, timeNextLevel;

    public void ThisLevelIsCleared()
    {
        boardState = BoardState.NEXTLEVELBLACK;
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
    }
    private void OnMouseDown()
    {
        isButtonDown = true;
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2 + 1];
    }

    private void OnMouseExit()
    {
        isButtonDown = false;
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
    }

    private void OnMouseUp()
    {
        if (isButtonDown)
        {
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
            switch (boardState)
            {
                case BoardState.MENUBLACK:
                    timeMenu = 1.0f;
                    boardState = BoardState.MENUYELLOW;
                    GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
                    break;
                case BoardState.MENUYELLOW:
                    SceneManager.LoadScene("SelectScene");
                    break;

                case BoardState.NEXTLEVELNOTABLE:
                    break;
                case BoardState.NEXTLEVELBLACK:
                    timeNextLevel = 1.0f;
                    boardState = BoardState.NEXTLEVELGREEN;
                    GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
                    break;
                case BoardState.NEXTLEVELGREEN:
                    timeNextLevel = 0.0f;
                    if (GameManager.Inst.level == 10)
                    {
                        GameManager.Inst.stage += 1;
                        GameManager.Inst.level = 1;
                        rule.transform.position = new Vector3(rule.position.x, 4.5f, rule.position.z);
                    }
                    else
                    {
                        GameManager.Inst.level += 1;
                        rule.transform.position = new Vector3(rule.position.x, 4.5f, rule.position.z);
                    }
                    LevelManager.Inst.MapReset(GameManager.Inst.stage, GameManager.Inst.level);
                    LevelManager.Inst.SetPlayState(PlayState.EDIT);
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeMenu = 0.0f;
        timeNextLevel = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeMenu > 0)
        {
            timeMenu -= Time.deltaTime;
        }
        else if (boardState == BoardState.MENUYELLOW)
        {
            timeMenu = 0.0f;
            boardState = BoardState.MENUBLACK;
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
        }

        if (timeNextLevel > 0)
        {
            timeNextLevel -= Time.deltaTime;
        }
        else if(boardState == BoardState.NEXTLEVELGREEN)
        {
            timeNextLevel = 0.0f;
            boardState = BoardState.NEXTLEVELBLACK;
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
        }
    }
}