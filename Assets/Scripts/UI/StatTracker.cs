using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatTracker : MonoBehaviour
{
    #region Obj and Component 

    public GameObject LevelNumbers;
    public GameObject TreasureNumbers;

    #endregion


    [SerializeField] Sprite[] bigNumbers;
    [SerializeField] Sprite[] smallNumbers;

    private GameObject[] levelDigits;
    private GameObject[] treasureDigits;

    // Start is called before the first frame update
    void Start()
    {
        levelDigits = new GameObject[LevelNumbers.transform.childCount];
        for (int i = 0; i < levelDigits.Length; i++)
        {
            levelDigits[i] = LevelNumbers.transform.GetChild(i).gameObject;
        }

        treasureDigits = new GameObject[TreasureNumbers.transform.childCount];
        for (int i = 0; i < treasureDigits.Length; i++)
        {
            treasureDigits[i] = TreasureNumbers.transform.GetChild(i).gameObject;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTreasureScore(int score)
    {
        string strScore = score.ToString();
        char[] charArray = strScore.ToCharArray();


        int digitOffset = treasureDigits.Length - charArray.Length;

        for (int i = 0; i < treasureDigits.Length; i++)
        {
            if(i < digitOffset)
            {
                treasureDigits[i].GetComponent<Image>().sprite = smallNumbers[0];
            }
            else
            {
                int number = Convert.ToInt32(charArray[i - digitOffset].ToString());

                

                treasureDigits[i].GetComponent<Image>().sprite = smallNumbers[number];
            }



        }


    }

    public void SetLevelScore(int score)
    {
        string strScore = score.ToString();
        char[] charArray = strScore.ToCharArray();


        int digitOffset = levelDigits.Length - charArray.Length;

        for (int i = 0; i < levelDigits.Length; i++)
        {
            if (i < digitOffset)
            {
                levelDigits[i].GetComponent<Image>().sprite = bigNumbers[0];
            }
            else
            {
                int number = Convert.ToInt32(charArray[i - digitOffset].ToString());



                levelDigits[i].GetComponent<Image>().sprite = bigNumbers[number];
            }



        }


    }

}
