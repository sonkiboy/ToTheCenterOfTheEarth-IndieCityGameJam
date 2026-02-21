using System.Collections;
using UnityEngine;

public class PalletteCycler : MonoBehaviour
{
    public PaletteManager paletteManager;
    public Color[] palletteOne;
    public Color[] palletteTwo;
    public Color[] palletteThree;
    public Color[] palletteFour;
    public float waitTime = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RunCycle()); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RunCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            paletteManager.ChangePalette(.5f, palletteOne[0], palletteOne[1], palletteOne[2], palletteOne[3]);
            yield return new WaitForSeconds(waitTime);
            paletteManager.ChangePalette(.5f, palletteTwo[0], palletteTwo[1], palletteTwo[2], palletteTwo[3]);

            yield return new WaitForSeconds(waitTime);
            paletteManager.ChangePalette(.5f, palletteThree[0], palletteThree[1], palletteThree[2], palletteThree[3]);

            yield return new WaitForSeconds(waitTime);
            paletteManager.ChangePalette(.5f, palletteFour[0], palletteFour[1], palletteFour[2], palletteFour[3]);

            yield return new WaitForSeconds(waitTime);
            paletteManager.ChangePalette(.5f, paletteManager.DefaultColors[0], paletteManager.DefaultColors[1], paletteManager.DefaultColors[2], paletteManager.DefaultColors[3]);
        }
    }
}
