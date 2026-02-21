using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PaletteManager : MonoBehaviour
{
    // the renderer component of the low screen resolution
    public RawImage ScreenRenderer;

    public Color[] DefaultColors;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetColors();
    }

    private void OnDisable()
    {
        ResetColors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePalette(float time, Color color1, Color color2, Color color3, Color color4)
    {
        StartCoroutine(SwapPalette(time, color1,color2 ,color3,color4)); 
    }

    private IEnumerator SwapPalette(float time, Color color1, Color color2, Color color3, Color color4)
    {
        int stepCount = 4;

        Color og1 = ScreenRenderer.material.GetColor("_NEW1");
        Color og2 = ScreenRenderer.material.GetColor("_NEW2");
        Color og3 = ScreenRenderer.material.GetColor("_NEW3");
        Color og4 = ScreenRenderer.material.GetColor("_NEW4");

        for (int i = 0; i < stepCount; i++)
        {
            ScreenRenderer.material.SetColor("_NEW1", Color.Lerp(og1, color1, (float)i / (float)stepCount));
            ScreenRenderer.material.SetColor("_NEW2", Color.Lerp(og2, color2, (float)i / (float)stepCount));
            ScreenRenderer.material.SetColor("_NEW3", Color.Lerp(og3, color3, (float)i / (float)stepCount));
            ScreenRenderer.material.SetColor("_NEW4", Color.Lerp(og4, color4, (float)i / (float)stepCount));

            yield return new WaitForSeconds(time/stepCount);
        }

        ScreenRenderer.material.SetColor("_NEW1", color1);
        ScreenRenderer.material.SetColor("_NEW2", color2);
        ScreenRenderer.material.SetColor("_NEW3", color3);
        ScreenRenderer.material.SetColor("_NEW4", color4);
    }

    public void ResetColors()
    {
        if(ScreenRenderer != null)
        {
            ScreenRenderer.material.SetColor("_NEW1", DefaultColors[0]);
            ScreenRenderer.material.SetColor("_NEW2", DefaultColors[1]);
            ScreenRenderer.material.SetColor("_NEW3", DefaultColors[2]);
            ScreenRenderer.material.SetColor("_NEW4", DefaultColors[3]);
        }
        
    }
}
