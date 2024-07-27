using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreCreator : MonoBehaviour
{
    #region Obj and Comp

    [SerializeField] GameObject Selector;
    [SerializeField] Image[] LetterRenderers;

    [SerializeField] Sprite[] LetterSprites;

    [SerializeField] GameObject LeaderBoardObj;
    LeaderBoard leaderBoard;

    #endregion

    PlayerControllerInput inputActions;
    InputAction moveInput;
    InputAction EnterInput;

    public AK.Wwise.Event StopAllSound;

    public int score = 100;

    int selectedIcon = 0;
    int selectedCharacter = 0;

    private void Awake()
    {
        inputActions = new PlayerControllerInput();
        moveInput = inputActions.UI.Move;
        EnterInput = inputActions.UI.Enter;

        moveInput.performed += InputNavigate;
        EnterInput.performed += EnterName;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        leaderBoard = LeaderBoardObj.GetComponent<LeaderBoard>();
        score = GameManager.Instance.CurrentTreasure;
    }

    void InputNavigate(InputAction.CallbackContext context) 
    {
        Vector2 direction = context.ReadValue<Vector2>();

        float threshHold = .5f;

        // if left or right, change which letter is selected
        if (direction.x > threshHold || direction.x < -threshHold)
        {
            if(direction.x > 0)
            {
                if(selectedIcon + 1 >= LetterRenderers.Length)
                {
                    selectedIcon = 0;
                }
                else
                {
                    selectedIcon++;
                }
                

            }
            else if (direction.x < 0)
            {
                if (selectedIcon - 1 <= -1)
                {
                    selectedIcon = LetterRenderers.Length - 1;
                }
                else
                {
                    selectedIcon--;
                }
            }

            selectedCharacter = GetIndexOfLetterSprite(LetterRenderers[selectedIcon].sprite);

            Selector.transform.localPosition = new Vector2(-65 + (selectedIcon * 36), -26);
        }

        // else if the input is up or down, 
        else if (direction.y > threshHold || direction.y < -threshHold)
        {
            if(direction.y > 0)
            {
                if(selectedCharacter + 1 >= LetterSprites.Length)
                {
                    selectedCharacter = 0;

                }
                else
                {
                    selectedCharacter++;
                }

            }
            else if (direction.y < 0)
            {
                if (selectedCharacter - 1 == -1)
                {
                    selectedCharacter = LetterSprites.Length - 1;

                }
                else
                {
                    selectedCharacter--;
                }

            }

            if (selectedIcon != LetterRenderers.Length - 1)
            {
                LetterRenderers[selectedIcon].sprite = LetterSprites[selectedCharacter];
            }
        }


    }

    int GetIndexOfLetterSprite(Sprite sprite)
    {
        for (int i = 0; i < LetterSprites.Length; i++)
        {
            if (LetterSprites[i].name == sprite.name)
            {
                return i;
            }


        }
        return 0;
    }

    string GetStrOfLetterSprite(Sprite sprite)
    {
        for (int i = 0; i < LetterSprites.Length; i++)
        {
            if (LetterSprites[i].name == sprite.name)
            {
                char[] nameArray = sprite.name.ToCharArray();

                return nameArray[nameArray.Length-1].ToString();
            }


        }
        return "A";
    }

    void EnterName(InputAction.CallbackContext contex)
    {
        if(selectedIcon == LetterRenderers.Length - 1)
        {
            string playerName = string.Empty;

            for (int i = 0; i < 4; i++)
            {
                playerName += GetStrOfLetterSprite(LetterRenderers[i].sprite);
            }

            //Debug.Log($"Writing {playerName} to the scoreboard with score {score}");
            score = GameManager.Instance.CurrentTreasure;
            StartCoroutine(leaderBoard.InsertNewScore(playerName, score));
            

            LeaderBoardObj.transform.localPosition = Vector2.zero;
            

            gameObject.transform.localPosition = Vector2.up * 400;

            moveInput.performed -= InputNavigate;
            EnterInput.performed -= EnterName;
            EnterInput.performed += RestartGame;
            
        }

    }

    private void RestartGame(InputAction.CallbackContext contex)
    {
        

        inputActions.Dispose();
        SceneManager.LoadScene("IntroScene");
    }

    


}
