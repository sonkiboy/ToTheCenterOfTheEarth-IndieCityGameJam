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

    public GameObject InstructionsParent;
    public Animator FadeAnimator;

    private void Awake()
    {
        
        GameManager.Instance.InputManager.MenuInput.performed += StartGame;
    }

    


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SlideShow());
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
        GameManager.Instance.SoundManager.PlayNonDiageticSound("GameOverOff");
        GameManager.Instance.InputManager.MenuInput.performed -= StartGame;


        StartCoroutine(RunInstructions());
    }

    IEnumerator RunInstructions()
    {
        FadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        InstructionsParent.SetActive(true);
        GameObject gemImg = InstructionsParent.transform.Find("Gems").gameObject;
        GameObject fuelImg = InstructionsParent.transform.Find("Fuel").gameObject;
        GameObject platImg = InstructionsParent.transform.Find("Platform").gameObject;
        
        gemImg.SetActive(true);
        yield return new WaitForSeconds(1f);
        fuelImg.SetActive(true);

        yield return new WaitForSeconds(1f);
        platImg.SetActive(true);

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("RegularGame");

    }


}
