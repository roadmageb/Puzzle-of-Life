using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TopBoardController : MonoBehaviour
{
    [SerializeField] private GameObject alphabetPrefab;
    [SerializeField] private Transform stringParent;
    private List<SpriteRenderer> alphabets;

    [SerializeField] private SpriteRenderer speedObject;
    [SerializeField] private SpriteRenderer stepObject;
    [SerializeField] private SpriteRenderer[] stepNumberObject;

    [SerializeField] private SpriteRenderer background;

    public TopBoard nextButton;
    bool alreadyCleared;

    float resetTime;
    float stopTime;
    float clearTime;
    bool clearDet;

    public Transform rule;

    void Start()
    {
        resetTime = -100.0f;
        stopTime = -100.0f;
        clearTime = 0.0f;
        clearDet = false;
        float alphabetWidth = alphabetPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        alphabets = new List<SpriteRenderer>();

        for (int i = 0; i < 5; ++i)
        {
            GameObject obj = Instantiate(alphabetPrefab, stringParent);
            alphabets.Add(obj.GetComponent<SpriteRenderer>());
            obj.transform.localPosition = new Vector3(alphabetWidth * i, 0, 0);
        }
        ChangeString("EDIT");

        if (GameManager.Inst.IsCleared(GameManager.Inst.stage, GameManager.Inst.level))
        {
            nextButton.ThisLevelIsCleared();
            alreadyCleared = true;
        }
        else
        {
            nextButton.ThisLevelIsNotCleared();
            alreadyCleared = false;
        }
    }

    public void NewLevelStarted()
    {
        resetTime = -100.0f;
        stopTime = -100.0f;
        clearTime = 0.0f;
        clearDet = false;
        LevelManager.Inst.stepCount = 0;

        ChangeString("EDIT");

        if (GameManager.Inst.IsCleared(GameManager.Inst.stage, GameManager.Inst.level))
        {
            nextButton.ThisLevelIsCleared();
            alreadyCleared = true;
        }
        else
        {
            nextButton.ThisLevelIsNotCleared();
            alreadyCleared = false;
        }
    }

    public void ChangeAlphabet(int index, char alphabet)
    {
        if(alphabet>='A' && alphabet <= 'Z')
        {
            alphabets[index].sprite = ImageManager.Inst.topBoardAlphabetSprites[alphabet - 'A'];
            alphabets[index].enabled = true;
        }
        else
        {
            alphabets[index].enabled = false;
        }
    }

    public void ChangeString(string str)
    {
        for(int i=0; i<alphabets.Count; ++i)
        {
            ChangeAlphabet(i, str.Length > i ? str[i] : '\0');
        }
    }

    /// <summary>
    /// 전광판 속도 변경
    /// </summary>
    /// <param name="speed">속도</param>
    public void ChangeSpeedObject(int speed)
    {
        switch (speed)
        {
            case 1:
                speedObject.sprite = ImageManager.Inst.topBoardSpeedSprites[1];
                break;
            case 2:
                speedObject.sprite = ImageManager.Inst.topBoardSpeedSprites[2];
                break;
            case 4:
                speedObject.sprite = ImageManager.Inst.topBoardSpeedSprites[3];
                break;
            case 8:
                speedObject.sprite = ImageManager.Inst.topBoardSpeedSprites[4];
                break;
        }
    }
    /// <summary>
    /// 전광판 스텝 변경
    /// </summary>
    /// <param name="step">스텝</param>
    public void ChangeStepObject(int step)
    {
        if(step == 0)
        {
            ChangeStepObject();
        }
        else
        {
            if (step < 100) // 스텝이 한 자리 수 또는 두 자리 수
            {
                stepNumberObject[0].sprite = ImageManager.Inst.topBoardStepSprites[(step % 10) + 3];
                stepNumberObject[1].sprite = ImageManager.Inst.topBoardStepSprites[(step / 10) + 3];
            }
            else // 스텝이 세 자리 수 이상일 경우: 뒤의 두 자리만 표시
            {
                stepNumberObject[0].sprite = ImageManager.Inst.topBoardStepSprites[((step - (step / 100) * 100) % 10) + 3];
                stepNumberObject[1].sprite = ImageManager.Inst.topBoardStepSprites[((step - (step / 100) * 100) / 10) + 3];
            }
        }
    }
    /// <summary>
    /// 전광판 스텝 초기화
    /// </summary>
    public void ChangeStepObject()
    {
        stepObject.sprite = ImageManager.Inst.topBoardStepSprites[0];
        stepNumberObject[0].sprite = ImageManager.Inst.topBoardStepSprites[2];
        stepNumberObject[1].sprite = ImageManager.Inst.topBoardStepSprites[2];
    }

    public void ChangeResetTime()
    {
        resetTime = 2.0f;
    }

    public void ChangeBoardByState(PlayState prevState, PlayState currState)
    {
        switch (currState)
        {
            case PlayState.EDIT:
                if(prevState == PlayState.PLAY || prevState == PlayState.PLAYFRAME) // 정지 버튼 누름
                {
                    ChangeString("STOP");
                    stopTime = 2.0f;
                }
                else
                {
                    ChangeString(currState.ToString());
                }
                speedObject.sprite = ImageManager.Inst.topBoardSpeedSprites[0];
                break;
            case PlayState.EDITTOINIT: // 다시하기 버튼 한 번 누른 상태임
                break;
            case PlayState.PLAY:
                if (prevState == PlayState.PLAY) // 가속 버튼 누름
                {
                    ChangeString("FAST");
                }
                else // 시작 버튼 누름
                {
                    stopTime = -100.0f;
                    resetTime = -100.0f;
                    ChangeString(currState.ToString());
                    if (prevState == PlayState.EDIT) // 편집 중이었을 경우 step00에 불 킴. 편집 중이 아니고 일시정지 중이었으면 실행 안 함.
                    {
                        stepObject.sprite = ImageManager.Inst.topBoardStepSprites[1];
                        stepNumberObject[0].sprite = ImageManager.Inst.topBoardStepSprites[3];
                        stepNumberObject[1].sprite = ImageManager.Inst.topBoardStepSprites[3];
                    }
                }
                break;
            case PlayState.PLAYFRAME:
                if (prevState == PlayState.PLAY) // 일시정지 버튼 누름
                {
                    ChangeString("PAUSE");
                }
                else // 스텝 버튼 누름
                {
                    stopTime = -100.0f;
                    resetTime = -100.0f;
                    speedObject.sprite = ImageManager.Inst.topBoardSpeedSprites[0];
                    ChangeString("STEP");
                    stepObject.sprite = ImageManager.Inst.topBoardStepSprites[1];

                }
                break;
        }
    }

    private void Update()
    {
        if (LevelManager.Inst.currentLevel.ClearCheck()) // 전광판에 CLEAR 깜빡이게, 배경 초록색으로, 다음 레벨 버튼 활성화
        {
            background.sprite = ImageManager.Inst.backgroundSprites[2];

            if(clearTime <= 0.0f && clearDet == false)
            {
                clearTime = 0.5f;
                clearDet = true;
                ChangeString("CLEAR");
            }
            else if(clearTime > 0.0f && clearDet == true)
            {
                clearTime -= Time.deltaTime;
            }
            else if(clearTime <= 0.0f && clearDet == true)
            {
                clearTime = 0.5f;
                clearDet = false;
                ChangeString("");
            }
            else if(clearTime > 0.0f && clearDet == false)
            {
                clearTime -= Time.deltaTime;
            }

            if (!alreadyCleared) // 이미 클리어되어있던 레벨이 아닐 경우, 다음 레벨 버튼 활성화
            {
                alreadyCleared = true;
                nextButton.ThisLevelIsCleared();
            }
        }

        if (stopTime > 0.0f) // 전광판에 STOP 2초간 띄우기
        {
            stopTime -= Time.deltaTime;
        }
        else if (stopTime <= 0.0f && stopTime > -100.0f)
        {
            ChangeString("EDIT");
            stopTime = -100.0f;
        }

        if(resetTime == 2.0f) // 전광판에 RESET 2초간 띄우기
        {
            ChangeString("RESET");
            resetTime -= Time.deltaTime;
        }
        else if(resetTime > 0.0f)
        {
            resetTime -= Time.deltaTime;
        }
        else if(resetTime <= 0.0f && resetTime > -100.0f)
        {
            ChangeString("EDIT");
            resetTime = -100.0f;
        }
    }
}
