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
        
    }

    


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SlideShow());
        GameManager.Instance.InputManager.MenuInput.performed += StartGame;

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
        GameManager.Instance.SoundManager.PlayNonDiageticSound("EndThemeStop");
        GameManager.Instance.InputManager.MenuInput.performed -= StartGame;
        GameManager.Instance.Player.enabled = false;
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

        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");
        gemImg.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");
        fuelImg.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");
        platImg.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("RegularGame");

    }


}
