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

    void Start()
    {
        float alphabetWidth = alphabetPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        alphabets = new List<SpriteRenderer>();
        for(int i=0; i<5; ++i)
        {
            GameObject obj = Instantiate(alphabetPrefab, stringParent);
            alphabets.Add(obj.GetComponent<SpriteRenderer>());
            obj.transform.localPosition = new Vector3(alphabetWidth * i, 0, 0);
        }
        ChangeString("EDIT");
            
    }

    void ChangeAlphabet(int index, char alphabet)
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

    void ChangeString(string str)
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
        
    }
    /// <summary>
    /// 전광판 스텝 변경
    /// </summary>
    /// <param name="step">스텝</param>
    public void ChangeStepObject(int step)
    {

    }
    /// <summary>
    /// 전광판 스텝 초기화
    /// </summary>
    public void ChangeStepObject()
    {

    }

    public void ChangeBoardByState(PlayState prevState, PlayState currState)
    {
        Debug.Log(prevState + " " + currState);
    }
}
