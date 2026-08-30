using TMPro;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    TextMeshProUGUI foreTxt;
    TextMeshProUGUI backTxt;

    public string[] Hints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreTxt = transform.Find("ForeText").GetComponent<TextMeshProUGUI>();
        backTxt = transform.Find("BackText").GetComponent<TextMeshProUGUI>();

        int index = Random.Range(0, Hints.Length);
        foreTxt.text = "Hint: " + Hints[index];
        backTxt.text = "Hint: " + Hints[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
