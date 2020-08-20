using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TopBoardController : MonoBehaviour
{
    public GameObject alphabetPrefab;
    public Transform stringParent;
    private List<SpriteRenderer> alphabets;

    public SpriteRenderer speedObject;
    public SpriteRenderer stepObject;
    public SpriteRenderer[] stepNumberObject;

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

    void ChangeSpeedObject(int speed)
    {

    }

    void ChangeStepObject(int step)
    {

    }

    void ChangeStepObject()
    {

    }
}
