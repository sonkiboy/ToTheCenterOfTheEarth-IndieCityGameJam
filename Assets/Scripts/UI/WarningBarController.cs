using System.Collections;
using UnityEngine;

public class WarningBarController : MonoBehaviour
{

    [SerializeField] GameObject[] WarningIcons;
    public float EnableRate = .25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < WarningIcons.Length; i++)
        {
            WarningIcons[i].SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWarning(float durration)
    {
        StartCoroutine(WarnRoutine(durration));
    }


    private IEnumerator WarnRoutine(float durration)
    {
        for (int i = 0; i < WarningIcons.Length; i++)
        {
            WarningIcons[i].SetActive(true);

            yield return new WaitForSeconds(EnableRate);
        }

        yield return new WaitForSeconds(durration - (WarningIcons.Length * EnableRate));

        for (int i = 0; i < WarningIcons.Length; i++)
        {
            WarningIcons[i].SetActive(false);

        }
    }
}
