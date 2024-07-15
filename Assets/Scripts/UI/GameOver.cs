using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    #region Obj and Comp

    [SerializeField] GameObject LeaderBoard;
    [SerializeField] GameObject BlackBG;

    LeaderBoard boardComponent;

    [SerializeField] GameObject highScore;

    #endregion 

    PlayerControllerInput inputActions;

    public InputAction EnterInput;
    public float waitTime = 3;

    private void Awake()
    {
        inputActions = new PlayerControllerInput();
        EnterInput = inputActions.UI.Enter;

    }

    




    // Start is called before the first frame update
    void Start()
    {
        boardComponent = LeaderBoard.GetComponent<LeaderBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameOver(int score)
    {
        StartCoroutine(GameOverSequence(score));
        inputActions.Enable();

    }

    IEnumerator GameOverSequence(int score)
    {
        BlackBG.SetActive(true);

        yield return null;

        GameManager.Instance.Platform.gameObject.SetActive(false);

        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemyArray)
        {
            Destroy(enemy);
        }

        yield return null;

        yield return new WaitForSeconds(waitTime);

        if (boardComponent.CheckNewScore(score))
        {
            highScore.SetActive(true);
            
        }
        else
        {
            LeaderBoard.transform.localPosition = Vector2.zero;
            EnterInput.performed += RestartGame;
        }

        
    }

    private void RestartGame(InputAction.CallbackContext contex)
    {
        inputActions.Dispose();
        SceneManager.LoadScene("MainScreen");
    }
}
