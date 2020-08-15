using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBoard : MonoBehaviour
{
    public BoardState boardState;
    private bool isButtonDown;
    public Transform rule;

    private void OnMouseDown()
    {
        isButtonDown = true;
    }

    private void OnMouseExit()
    {
        isButtonDown = false;
    }

    private void OnMouseUp()
    {
        if (isButtonDown)
        {
            switch (boardState)
            {
                case BoardState.MENU:
                    SceneManager.LoadScene("SelectScene"); // 메인 메뉴 불러오기
                    break;

                case BoardState.NEXTLEVEL:
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
                    LevelManager.Inst.SetPlayState(PlayState.EDIT); // 다음 레벨 불러오기
                    break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
