using System.Collections;
using UnityEngine;

public class OverlayUI : MonoBehaviour
{

    #region Stage Titles

    [SerializeField] GameObject CrustStageTitle;
    [SerializeField] GameObject JungleStageTitle;
    [SerializeField] GameObject MachinaStageTitle;

    #endregion

    [SerializeField] GameObject CountdownParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCountdown()
    {
        StartCoroutine(RunCountdown());
    }

    IEnumerator RunCountdown()
    {
        GameObject three = CountdownParent.transform.GetChild(0).gameObject; 
        GameObject two = CountdownParent.transform.GetChild(1).gameObject; 
        GameObject one = CountdownParent.transform.GetChild(2).gameObject; 
        GameObject go = CountdownParent.transform.GetChild(3).gameObject;

        three.SetActive(true);
        yield return new WaitForSeconds(1); 
        three.SetActive(false);
        two.SetActive(true);
        yield return new WaitForSeconds(1);
        two.SetActive(false);
        one.SetActive(true);
        yield return new WaitForSeconds(1);
        one.SetActive(false);
        go.SetActive(true);
        yield return new WaitForSeconds(3); 
        three.SetActive(false);
        two.SetActive(false);
        one.SetActive(false);
        go.SetActive(false);
    }

    public void ShowLevelTitle(int stage)
    {

    }
}
