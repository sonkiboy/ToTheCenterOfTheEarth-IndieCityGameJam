using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartSlideShow : MonoBehaviour
{
    public GameObject Slides;
    public float SlideTime = 5;

    PlayerControllerInput inputActions;
    InputAction StartInput;

    public AK.Wwise.Event StopAllSound;

    private void Awake()
    {
        inputActions = new PlayerControllerInput();
        StartInput = inputActions.Player.Start;
        StartInput.performed += StartGame;
    }

    private void OnEnable()
    {
        inputActions.Enable();

    }
    private void OnDisable()
    {
        inputActions.Disable();
        
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SlideShow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SlideShow()
    {
        GameObject[] slideArray = new GameObject[Slides.transform.childCount];

        for (int i = 0; i < slideArray.Length; i++)
        {
            slideArray[i] = Slides.transform.GetChild(i).gameObject;
        }

        int index = 0;

        yield return new WaitForSeconds(SlideTime);

        while (true)
        {
            slideArray[index].SetActive(false);

            index++;

            if (index >= slideArray.Length)
            {
                index = 0;
            }

            slideArray[index].SetActive(true);

            yield return new WaitForSeconds(SlideTime);
        }


    }

    void StartGame(InputAction.CallbackContext context)
    {
        

        SceneManager.LoadScene("MainScreen");
    }
}
