using UnityEngine;

public class BasicCurvedLineController : MonoBehaviour
{
    public LineRenderer Line;

    public Vector2 Origin;
    public Vector2 Target;
    public Vector2 RelativeBezPoint;

    public int JointCount = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Line.positionCount != JointCount)
        {
            Line.positionCount = JointCount;
        }

        Line.SetPosition(0, Origin);
        Line.SetPosition(Line.positionCount-1, Target);

        for (int i = 1; i < JointCount-1; i++)
        {


            float calculatedI = (float)i / (float)JointCount;
            float x = Mathf.Pow(1-calculatedI,2)*Origin.x + 2*(1-calculatedI)*calculatedI*RelativeBezPoint.x+Mathf.Pow(calculatedI,2)*Target.x;
            float y = Mathf.Pow(1 - calculatedI, 2) * Origin.y + 2 * (1 - calculatedI) * calculatedI * RelativeBezPoint.y + Mathf.Pow(calculatedI, 2) * Target.y;

            Line.SetPosition(i, new Vector2(x, y));

        }
    }
}
