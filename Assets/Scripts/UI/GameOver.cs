using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour, ISceneUpdate
{
    #region Obj and Comp
    GameObject EndUi;
    GameObject LeaderBoard;
    
    public GameObject BlackBG;

    LeaderBoard boardComponent;

    [SerializeField] GameObject highScore;

    #endregion 

    
    public AK.Wwise.Event EndMusicOn;
    public AK.Wwise.Event EndMusicOff;

    
    public float waitTime = 3;

    




    private void OnDisable()
    {
        GameManager.Instance.SoundManager.PlayNonDiageticSound("EndThemeStop");
        SceneManager.activeSceneChanged -= OnSceneChanged;

    }

    // Start is called before the first frame update
    void Start()
    {
        
        
        SceneManager.activeSceneChanged += OnSceneChanged;
        OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameOver(int score)
    {
        StartCoroutine(GameOverSequence(score));
        

    }

    IEnumerator GameOverSequence(int score)
    {
        BlackBG.SetActive(true);

        yield return null;

        GameManager.Instance.ArcadeLEDs.SetCabinetLights(ArcadeLightManager.LightModes.off, 0);


        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemyArray)
        {
            Destroy(enemy);
        }

        yield return null;

        yield return new WaitForSeconds(waitTime);

        GameManager.Instance.ArcadeLEDs.SetCabinetLights(ArcadeLightManager.LightModes.fade, 0);

        GameManager.Instance.SoundManager.PlayNonDiageticSound("EndThemeStart");

        yield return new WaitForSeconds(.5f);

        if (boardComponent.CheckNewScore(score))
        {
            highScore.SetActive(true);
            
        }
        else
        {
            boardComponent.Board.SetActive(true);
            GameManager.Instance.InputManager.MenuInput.started += RestartGame;
        }


        
    }

    private void RestartGame(InputAction.CallbackContext contex)
    {
        //Debug.LogAssertion("HIT IN GAMEOVER");

        GameManager.Instance.InputManager.MenuInput.started -= RestartGame;


        

        
        SceneManager.LoadScene("MainMenu");
    }

    public void OnSceneChanged(Scene Current, Scene Next)
    {
        if(Next.name != "MainMenu")
        {
            EndUi = GameObject.Find("EndCanvas").gameObject;
            LeaderBoard = EndUi.transform.Find("LeaderBoard").gameObject;
            boardComponent = LeaderBoard.GetComponent<LeaderBoard>();
            BlackBG = EndUi.transform.Find("DeathBackground").gameObject;
            highScore = EndUi.transform.Find("HighScore").gameObject;
        }
        else
        {
            EndUi = null;
            LeaderBoard = null;
            boardComponent = null;
            BlackBG= null;
            highScore = null;
        }
        
    }
}
