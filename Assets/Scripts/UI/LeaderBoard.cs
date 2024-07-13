using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Unity.VisualScripting;

public class LeaderBoard : MonoBehaviour
{
    #region Obj and comp
    [SerializeField] Sprite[] FontSprites;
    [SerializeField] GameObject[] PlayerNames;
    [SerializeField] GameObject[] Scores;
    
    #endregion

    string path;

    private void OnEnable()
    {

        

        
        

    }
    // Start is called before the first frame update
    void Start()
    {
        path = Application.persistentDataPath + "/LeaderBoard.txt";




        InsertNewScore("kill",9999999);
        UpdateScoreBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScoreBoard()
    {
        StreamReader reader = new StreamReader(path);

        string[] textArray = reader.ReadToEnd().Split("\n");
        reader.Close();

        string[] textNames = new string[textArray.Length/2];
        string[] textScores = new string[textArray.Length / 2];

        int counter = 0;

        for (int i = 0; i < textNames.Length; i++)
        {
            textNames[i] = textArray[counter];
            textScores[i] = textArray[counter + 1];

            counter += 2;
        }

       
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

                for (int j = 0; j < PlayerNames[i].transform.childCount; j++)
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

    public bool CheckNewScore(int newScore, string[] textArray)
    {
        

        int lowestScore = Convert.ToInt32(textArray[textArray.Length - 1]);

        if(newScore > lowestScore)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void InsertNewScore(string name, int score)
    {
        name = name.ToUpper();

        StreamReader reader = new StreamReader(path);

        string[] textArray = reader.ReadToEnd().Split("\n");
        reader.Close();

        int indexToInsert = -1;

        // first find where the score should be in the list
        for (int i = 0; i < textArray.Length; i += 2)
        {
            // i is the name
            int checkingScore = Convert.ToInt32(textArray[textArray.Length + 1]);

            if(score > checkingScore)
            {
                indexToInsert = i;
                break;
            }
        }

        // then create the new string to store
        string[] newDataArray = new string[20];

        for (int i = 0; i < newDataArray.Length; i++)
        {
            if(i < indexToInsert * 2)
            {
                newDataArray[i] = textArray[i];
            }
            else if (i == indexToInsert)
            {
                newDataArray[i] = name;
            }
            else if (i == indexToInsert + 1)
            {
                newDataArray[i] = score.ToString();
            }
            else
            {
                if(i < textArray.Length)
                {
                    newDataArray[i] = textArray[i];
                }
                
            }
            

        }

        string newSave = string.Empty;

        for (int i = 0; i < newDataArray.Length; i++)
        {
            newSave += newDataArray[i] + "\n";

        }

        StreamWriter writer = new StreamWriter(path);
        writer.Write(newSave);


    }
}
