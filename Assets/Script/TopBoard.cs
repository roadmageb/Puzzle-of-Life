using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBoard : MonoBehaviour
{
    public BoardState boardState;
    private bool isButtonDown;
    //public Transform rule;
    private Transform ruleBar;
    private float timeMenu, timeNextLevel;

    public void ThisLevelIsCleared()
    {
        boardState = BoardState.NEXTLEVELBLACK;
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
    }

    public void ThisLevelIsNotCleared()
    {
        boardState = BoardState.NEXTLEVELNOTABLE;
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
                    AudioManager.Inst.ButtonClicked();
                    timeMenu = 1.0f;
                    boardState = BoardState.MENUYELLOW;
                    GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
                    break;
                case BoardState.MENUYELLOW:
                    AudioManager.Inst.ButtonClicked();
                    SceneManager.LoadScene("SelectScene");
                    break;

                case BoardState.NEXTLEVELNOTABLE:
                    AudioManager.Inst.ButtonCantBeClicked();
                    break;
                case BoardState.NEXTLEVELBLACK:
                    AudioManager.Inst.ButtonClicked();
                    timeNextLevel = 1.0f;
                    boardState = BoardState.NEXTLEVELGREEN;
                    GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.topBoardSprites[(int)boardState * 2];
                    break;
                case BoardState.NEXTLEVELGREEN:
                    AudioManager.Inst.ButtonClicked();
                    timeNextLevel = 0.0f;
                    ruleBar = transform.parent.GetComponent<TopBoardController>().rule;
                    ruleBar.transform.position = new Vector3(ruleBar.position.x, 4.5f, ruleBar.position.z);

                    if (GameManager.Inst.level == 10)
                    {
                        GameManager.Inst.stage += 1;
                        GameManager.Inst.level = 1;
                    }
                    else
                    {
                        GameManager.Inst.level += 1;
                    }
                    LevelManager.Inst.MapReset(GameManager.Inst.stage, GameManager.Inst.level);
                    LevelManager.Inst.PauseLevel();
                    LevelManager.Inst.SetPlayState(PlayState.EDIT);
                    transform.parent.GetComponent<TopBoardController>().ChangeStepObject();
                    transform.parent.GetComponent<TopBoardController>().NewLevelStarted();
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