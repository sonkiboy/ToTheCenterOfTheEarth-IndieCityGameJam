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

    
    public AK.Wwise.Event EndMusicOn;
    public AK.Wwise.Event EndMusicOff;

    PlayerControllerInput inputActions;

    public InputAction EnterInput;
    public float waitTime = 3;

    private void Awake()
    {
        inputActions = new PlayerControllerInput();
        EnterInput = inputActions.UI.Enter;

    }




    private void OnDisable()
    {
        EndMusicOff.Post(GameManager.Instance.CenterScreen);
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

        AkSoundEngine.StopAll(GameObject.FindGameObjectWithTag("Player"));

        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemyArray)
        {
            Destroy(enemy);
        }

        yield return null;

        yield return new WaitForSeconds(waitTime);
        

        EndMusicOn.Post(GameManager.Instance.CenterScreen);

        yield return new WaitForSeconds(.5f);

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

        EndMusicOff.Post(GameManager.Instance.CenterScreen);

        inputActions.Dispose();
        SceneManager.LoadScene("IntroScene");
    }
}
