using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatTracker : MonoBehaviour
{
    #region Obj and Component 

    // The Root Object for the Level Number and the Treasure Number, as well as arrays of all of their individual digits
    public GameObject LevelNumbers;
    private GameObject[] levelDigits;
    public GameObject TreasureNumbers;
    private GameObject[] treasureDigits;

    public GameObject LevelBossText;
    public GameObject BossWinText;

    #endregion

    // Sprites for the large and small variants of the score digits
    [SerializeField] Sprite[] bigNumbers;
    [SerializeField] Sprite[] smallNumbers;

    // Start is called before the first frame update
    void Start()
    {
        // Create a new array based on the number of digits the Level Number can display
        levelDigits = new GameObject[LevelNumbers.transform.childCount];

        // loop through the newly made array and save all the digits in Level Number to it
        for (int i = 0; i < levelDigits.Length; i++)
        {
            levelDigits[i] = LevelNumbers.transform.GetChild(i).gameObject;
        }

        // Create a new array based on the number of digits the Treasure Number can display
        treasureDigits = new GameObject[TreasureNumbers.transform.childCount];

        // loop through the newly made array and save all the digits in Treasure Number to it
        for (int i = 0; i < treasureDigits.Length; i++)
        {
            treasureDigits[i] = TreasureNumbers.transform.GetChild(i).gameObject;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Sets the Treasure UI Number to the passed in score value
    public void SetTreasureScore(int score)
    {
        // convert the int to a string and split all the characters in that string to an array as their own index
        string strScore = score.ToString();
        char[] charArray = strScore.ToCharArray();

        // if the number of digits is less than the number of digits that could be displayed, save difference in digits
        // this will be used to add 0's infront of the number to fill the empty space
        int digitOffset = treasureDigits.Length - charArray.Length;

        // Loop through the digits in the UI and set them to their correct number
        for (int i = 0; i < treasureDigits.Length; i++)
        {
            // if the current digit is less than the digit offset, then the number is to small to use this digit, so set it to 0
            if(i < digitOffset)
            {

                if(treasureDigits[i] != null)
                {
                    treasureDigits[i].GetComponent<Image>().sprite = smallNumbers[0];

                }
            }

            // if the score has a value for this digit spot, then set the UI digit to the correct number
            else
            {
                int number = Convert.ToInt32(charArray[i - digitOffset].ToString());

                if (treasureDigits[i] != null)
                {
                    treasureDigits[i].GetComponent<Image>().sprite = smallNumbers[number];
                }
            }
        }
    }

    // Sets the Score UI Number to the passed in score value
    public void SetLevelScore(int score)
    {
        // convert the int to a string and split all the characters in that string to an array as their own index
        string strScore = score.ToString();
        char[] charArray = strScore.ToCharArray();

        // if the number of digits is less than the number of digits that could be displayed, save difference in digits
        // this will be used to add 0's infront of the number to fill the empty space
        int digitOffset = levelDigits.Length - charArray.Length;

        // Loop through the digits in the UI and set them to their correct number
        for (int i = 0; i < levelDigits.Length; i++)
        {
            // if the current digit is less than the digit offset, then the number is to small to use this digit, so set it to 0
            if (i < digitOffset)
            {
                levelDigits[i].GetComponent<Image>().sprite = bigNumbers[0];
            }

            // if the score has a value for this digit spot, then set the UI digit to the correct number
            else
            {
                int number = Convert.ToInt32(charArray[i - digitOffset].ToString());



                levelDigits[i].GetComponent<Image>().sprite = bigNumbers[number];
            }
        }
    }

    public void TurnOnBoss()
    {
        LevelBossText.SetActive(true);
        BossWinText.SetActive(false);
    }

    public void TurnOffBoss(bool win)
    {
        LevelBossText.SetActive(false);

        if (!win)
        {
            BossWinText.SetActive(true);
        }
    }
}
