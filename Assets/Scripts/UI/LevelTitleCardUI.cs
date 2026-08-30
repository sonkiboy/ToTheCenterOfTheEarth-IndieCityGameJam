using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTitleCardUI : MonoBehaviour
{
    TextMeshProUGUI nextLevelTxt; 
    TextMeshProUGUI titleTxt;

    [SerializeField]private float textSpeed = .2f;

    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextLevelTxt = transform.Find("NextLevel").GetComponent<TextMeshProUGUI>();
        titleTxt = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();

        nextLevelTxt.text = string.Empty;
        titleTxt.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayLayerTitle(string title)
    {
        //nextLevelTxt.text = string.Empty;
        //titleTxt.text = string.Empty;
        //StartCoroutine(TextScroll(title));
    }

    //IEnumerator TextScroll(string title)
    //{
    //    print "next level"


    //    char[] displayTxt = "NEXT LEVEL".ToCharArray();

    //    foreach (char c in displayTxt)
    //    {
    //        nextLevelTxt.text += c;

    //        yield return new WaitForSeconds(textSpeed);
    //    }

    //    yield return new WaitForSeconds(.5f);

    //    print title


    //   title.Replace(" ", "\n");

    //    displayTxt = title.ToCharArray();

    //    foreach (char c in displayTxt)
    //    {
    //        titleTxt.text += c;
    //        yield return new WaitForSeconds(textSpeed * 1.5f);

    //    }

    //    yield return new WaitForSeconds(3f);
    //    nextLevelTxt.text = string.Empty;
    //    titleTxt.text = string.Empty;
    //}
}
