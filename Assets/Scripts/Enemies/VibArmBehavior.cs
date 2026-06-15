using System.Collections;
using UnityEngine;

public class VibArmBehavior : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform TargetPosition;
    public GameObject ArmEnd;
    
    private Vector2 handPosition = Vector2.zero;
    public float InitialSearchRadius = 2f;
    public float Speed = 5f;

    public Vector2 MoveTimeRange = Vector2.one *2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoveArmUpdate());
        handPosition = TargetPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, (Vector2)transform.InverseTransformPoint(handPosition));
        ArmEnd.transform.position = handPosition;
    }

    IEnumerator MoveArmUpdate()
    {
        while (isActiveAndEnabled)
        {
            MoveArm();

            yield return new WaitForSeconds(Random.Range(MoveTimeRange.x,MoveTimeRange.y));
        }
    }

    void MoveArm()
    {
        Collider2D foundTileCollider = null;

        // loop number of times we will try and search for a tile
        for (int i = 1; i < 4; i++)
        {
            foundTileCollider = Physics2D.OverlapCircle(TargetPosition.position, InitialSearchRadius * (float)i, LayerMask.GetMask("Tile"));

            // if we found a tile break out of the loop
            if(foundTileCollider != null )
            {
                break;
            }
        }

        // if there was a tile found, move the arm to that tiles center
        if( foundTileCollider != null )
        {
            //Debug.Log($"Moving Arm to Tile at {foundTileCollider.transform.position + foundTileCollider.bounds.size / 2}");
            StartCoroutine(MoveArmToPoint(foundTileCollider.transform.position + foundTileCollider.bounds.size/2));
        }

        // if no tile was found, move the arm to the exact target position regardless if there is a tile there
        else
        {
            //Debug.Log($"Moving to Open Space at {TargetPosition.position}");
            StartCoroutine(MoveArmToPoint(TargetPosition.position));
        }
    }

    IEnumerator MoveArmToPoint(Vector2 targetPosition)
    {
        float distanceThresh = .25f;
        Vector2 dir = Vector2.zero;
        Vector2 newPos = Vector2.zero;

        //lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.InverseTransformPoint(targetPosition));
        //ArmEnd.transform.position = targetPosition;

        while (Vector2.Distance(transform.TransformPoint(lineRenderer.GetPosition(lineRenderer.positionCount - 1)), targetPosition) > distanceThresh)
        {
            dir = (targetPosition - (Vector2)transform.TransformPoint(lineRenderer.GetPosition(lineRenderer.positionCount - 1))).normalized;
            newPos = (Vector2)transform.TransformPoint((Vector2)lineRenderer.GetPosition(lineRenderer.positionCount - 1)) + dir * Speed * Time.deltaTime;

            handPosition = newPos;

            //Debug.Log($"Traveling to {newPos}");

            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }
}
