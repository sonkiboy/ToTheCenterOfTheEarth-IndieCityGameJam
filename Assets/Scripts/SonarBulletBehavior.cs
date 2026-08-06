using System.Collections;
using UnityEngine;

public class SonarBulletBehavior : BulletBehavior
{
    Rigidbody2D rb;
    public float MaxScaleSize = 2f;
    public AnimationCurve MovementCurve;
    public float MaxDistance = 3f;
    public float Speed = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(GrowOverLifetime());
        StartCoroutine(MoveOverCurve());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveOverCurve()
    {
        int incriments = Mathf.RoundToInt(60/Speed);
        float timeIncriment = DestroyTime / (float)incriments;

        Vector2 origin = this.transform.position;
        
        
        Vector2 target = origin + ((Vector2)this.transform.right * MaxDistance);
        Debug.Log($"Origin: {origin}  Target: {target} Dir: {(Vector2)this.transform.forward}");

        for (int i = 0; i < incriments; i++)
        {
            //Debug.Log($"Curve eVAL {MovementCurve.Evaluate((float)i / (float)incriments)} ({(float)i / (float)incriments}) equals {Vector3.Lerp(origin, target, MovementCurve.Evaluate((float)i / (float)incriments))}");
            rb.MovePosition(Vector3.Lerp(origin, target, MovementCurve.Evaluate((float)i / (float)incriments)));

            yield return new WaitForSeconds(timeIncriment);
        }

        yield return null;
    }

    IEnumerator GrowOverLifetime()
    {
        int incriments = Mathf.RoundToInt(60 / Speed);
        float timeIncriment = DestroyTime/(float)incriments;

        for (int i = 0; i < incriments; i++)
        {
            
            this.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * MaxScaleSize, (float)i / (float)incriments);
            yield return new WaitForSeconds(timeIncriment);
        }
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {

        
    }
    

}
