using System.Collections;
using UnityEngine;

public class TankArmBehavior : MonoBehaviour
{
    float ThighLength = 2f;
    float ShinLength = 2f;

    [SerializeField] Transform ThighObj;
    [SerializeField] Transform ShinObj;
    [SerializeField] Transform FootObj;

    // refrence point that moves with boss platform. When updated this is where the foot lands
    public Transform TargetTrans;

    // the position of the foot
    [SerializeField] Vector2 targetPos;


    public AnimationCurve StepCurve;
    public Vector2 StepTimeRange = Vector2.up;
    public Vector2 InvalidLocalBounds = Vector2.zero;
    public bool InvalidBoundsIfGreater = false;
    public Vector2 RandomStepPositionOffset = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = TargetTrans.position;
        StartCoroutine(StepUpdate());


    }

    // Update is called once per frame
    void Update()
    {
        CalculateArmerature();
    }

    IEnumerator StepUpdate()
    {
        while (true)
        {

            float ranTime = Random.Range(StepTimeRange.x, StepTimeRange.y);
            int incriments = 60;
            float stepTime = ranTime / (float)incriments;
            Vector2 stepTargetPos = Vector2.zero;

            for (int i = 0; i < incriments; i++)
            {
                if (Vector3.Distance(targetPos, TargetTrans.position) > (ThighLength))
                {
                    if (!InvalidBoundsIfGreater)
                    {
                        stepTargetPos = (Vector2)TargetTrans.position - InvalidLocalBounds / 2;
                    }
                    else
                    {
                        stepTargetPos = (Vector2)TargetTrans.position + InvalidLocalBounds / 2;
                    }
                        Debug.Log("Leg distance strained, breaking from cool down");
                    break;
                }

                if (FootObj.transform.position.y > TargetTrans.position.y + InvalidLocalBounds.y )
                {
                    Debug.Log($"Leg is too far Up from the target. Moving to {(Vector2)TargetTrans.position - InvalidLocalBounds / 2} (Target Pos: {(Vector2)TargetTrans.position})");
                    stepTargetPos = (Vector2)TargetTrans.position - InvalidLocalBounds/2;
                    break;
                }
                else if (FootObj.transform.position.y < TargetTrans.position.y - InvalidLocalBounds.y)
                {
                    Debug.Log($"Leg is too far Down from the target. Moving to {(Vector2)TargetTrans.position + InvalidLocalBounds / 2} (Target Pos: {(Vector2)TargetTrans.position})");

                    stepTargetPos = (Vector2)TargetTrans.position + InvalidLocalBounds/2;

                    break;
                }

                    yield return new WaitForSeconds(stepTime);
            }

            if(stepTargetPos == Vector2.zero)
            {
                stepTargetPos = (Vector2)TargetTrans.position + new Vector2(Random.Range(RandomStepPositionOffset.x, RandomStepPositionOffset.y), Random.Range(RandomStepPositionOffset.x, RandomStepPositionOffset.y));
            }
            
            Vector2 startPos = targetPos;
            
            float durration = Mathf.Clamp( Vector3.Distance(targetPos, TargetTrans.position), 10, 20);
            float count = 0;
            Vector2 newPos;
            //Debug.Log("hit");
            do
            {


                newPos = Vector3.Lerp(startPos, stepTargetPos, count / durration);

                newPos = newPos + Vector2.left * StepCurve.Evaluate(count / durration) * (Mathf.Clamp(Vector3.Distance(startPos, stepTargetPos) / 2, 0, 5));

                //Debug.Log($"Moving Pickup to {newPos} at {count / durration} progress ({jumpCurve.Evaluate(count / durration)})");

                targetPos = newPos;

                count++;
                yield return new WaitForFixedUpdate();
            } while (Vector2.Distance(FootObj.position, stepTargetPos) > .15f && count < durration);


            targetPos = stepTargetPos;

        }
    }

    void CalculateArmerature()
    {
        Vector2 originPos = ThighObj.transform.position;

        this.transform.rotation = Quaternion.LookRotation(Vector3.forward, (targetPos - originPos).normalized);


        float hypotnuseLength = Mathf.Clamp(Vector2.Distance(targetPos, originPos), 0, ThighLength + ShinLength);

        float bsMathStuffForThigh = (Mathf.Pow(ThighLength, 2) + Mathf.Pow(hypotnuseLength, 2) - Mathf.Pow(ShinLength, 2)) / (2 * ThighLength * hypotnuseLength);
        float bsMathStuffForShin = Mathf.Pow(ShinLength, 2) + Mathf.Pow(ThighLength, 2) - Mathf.Pow(hypotnuseLength, 2) / 2 * ShinLength * ThighLength;


        float thighRadian = 180 + Mathf.Acos(Mathf.Clamp(bsMathStuffForThigh, -1, 1)) * Mathf.Rad2Deg;
        float shinRadian = Mathf.Acos(Mathf.Clamp(bsMathStuffForShin, -1, 1)) * Mathf.Rad2Deg;

        ThighObj.localRotation = Quaternion.Euler(0, 0, thighRadian);
        ShinObj.localRotation = Quaternion.Euler(0, 0, 180 - (thighRadian * 2));

    }
}
