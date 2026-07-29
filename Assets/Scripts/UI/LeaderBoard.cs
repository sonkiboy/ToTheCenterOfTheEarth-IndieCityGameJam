using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Unity.VisualScripting;
using System.Linq;

public class LeaderBoard : MonoBehaviour
{
    #region Obj and comp
    [SerializeField] Sprite[] FontSprites;
    [SerializeField] GameObject[] PlayerNames;
    [SerializeField] GameObject[] Scores;

    #endregion

    string[] textNames;
    string[] textScores;

    string path;

    private void OnEnable()
    {

    }

    private void Awake()
    {
        path = Application.persistentDataPath + "/LeaderBoard.txt";
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // reads the data from the score board txt file and updates the textNames and textScores
    void ReadDataFromFile()
    {
        // if the file for the score board scores doesn't exist, then make a new one that has no text
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }

        // read all the lines from the score board txt file and save them to a string array
        string[] textArray = File.ReadAllLines(path);

        // the txt file saves the scores and names as seperate lines, so half of the elements in the textArray are scores, and the other half are names
        // create two arrays for the names and scores seperately, sized at half the length of the text array
        textNames = new string[textArray.Length / 2];
        textScores = new string[textArray.Length / 2];

        // seperate the textArray elements into the two seperate textNames and textScores arrays
        int counter = 0;
        for (int i = 0; i < textNames.Length; i++)
        {
            textNames[i] = textArray[counter];
            textScores[i] = textArray[counter + 1];

            counter += 2;
        }
    }

    public void UpdateScoreBoard()
    {
        ReadDataFromFile();

        for (int i = 0; i < Scores.Length; i++)
        {

            if (i < textScores.Length)
            {
                char[] charArray = textScores[i].ToCharArray();

                int digitOffset = Scores[i].transform.childCount - charArray.Length;

                for (int k = 0; k < Scores[i].transform.childCount; k++)
                {
                    if (k < digitOffset)
                    {
                        Scores[i].transform.GetChild(k).GetComponent<Image>().sprite = GetFontSprite("null");
                    }
                    else
                    {
                        Scores[i].transform.GetChild(k).GetComponent<Image>().sprite = GetFontSprite(charArray[k - digitOffset].ToString());
                    }



                }
            }
            else
            {
                for (int k = 0; k < Scores[i].transform.childCount; k++)
                {
                    
                        Scores[i].transform.GetChild(k).GetComponent<Image>().sprite = GetFontSprite("null");
                    
                }
            }


        }

        for (int i = 0; i < PlayerNames.Length; i++)
        {

            if (i < textNames.Length)
            {
                char[] charArray = textNames[i].ToCharArray();

                //Debug.Log($"Setting Player name {i} to {textArray[i]}");

                for (int j = 0; j < charArray.Length; j++)
                {
                    
                    Image image = PlayerNames[i].transform.GetChild(j).GetComponent<Image>();
                    image.sprite = GetFontSprite(charArray[j].ToString());


                }
            }
            else
            {
                for (int j = 0; j < PlayerNames[i].transform.childCount; j++)
                {
                    Image image = PlayerNames[i].transform.GetChild(j).GetComponent<Image>();
                    image.sprite = GetFontSprite("null");


                }
            }
        }
    }

    

    Sprite GetFontSprite(string character)
    {
        string pathName = "LB_" + character;

        foreach(Sprite sprite in FontSprites)
        {
            if (sprite.name == pathName)
            {
                return sprite;
                
            }
        }

        Debug.Log($"No Font Character found for {character}, setting to {FontSprites[0].name}");
        return FontSprites[0];
    }

    public bool CheckNewScore(int newScore)
    {
        UpdateScoreBoard();

        if (textScores.Length < 10)
        {
            return true;
        }
        else if(textScores.Length >= 10)
        {
            int lowestScore = Convert.ToInt32(textScores[9]);

            if (newScore > lowestScore)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            int lowestScore = Convert.ToInt32(textScores[textScores.Length - 1]);

            if (newScore > lowestScore)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

    }

    public IEnumerator InsertNewScore(string name, int score)
    {
        name = name.ToUpper();



        string[] textArray = File.ReadAllLines(path);
        //Debug.Log($"Read {textArray.Length} lines from file : {textArray[0]}");

        int insertIndex = -1;

        for (int i = 0; i < textScores.Length; i++)
        {
            int convertedScore = Convert.ToInt32(textScores[i]);

            if(score > convertedScore)
            {
                insertIndex = i * 2;
                break;
            }
        }

        if(insertIndex == -1 && textArray.Length < 20)
        {
            insertIndex = textArray.Length;
        }


        List<string> oldList = textArray.ToList<string>();

        if(insertIndex >= 0)
        {
            //Debug.Log($"Inserting: {name}, into index {insertIndex} with score {score}");

            oldList.Insert(insertIndex, name);
            oldList.Insert(insertIndex + 1, score.ToString());
        }


        string newSave = string.Empty;

        for (int i = 0; i < oldList.Count; i++)
        {
            if (i != oldList.Count - 1)
            {
                newSave += oldList[i] + "\n";
            }
            else
            {
                newSave += oldList[i];
            }
        }

        

        File.WriteAllText(path, newSave);
        

        yield return new WaitForFixedUpdate();

        UpdateScoreBoard();
    }

    
}
