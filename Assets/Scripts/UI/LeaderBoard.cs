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
    // Start is called before the first frame update
    void Start()
    {

        path = Application.persistentDataPath + "/LeaderBoard.txt";

        

        UpdateScoreBoard();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScoreBoard()
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path,"");
        }

        string[] textArray = File.ReadAllLines(path);



        //Debug.Log($"Read from file : {textArray[0]}");
        

        textNames = new string[textArray.Length/2];
        textScores = new string[textArray.Length / 2];

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
