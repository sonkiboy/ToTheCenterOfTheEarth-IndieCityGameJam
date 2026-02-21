using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehavior : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] Collider2D mainCollider;
    private enum JumpToPoint
    {
        Score,Platform,Player,None
    }

    [SerializeField] JumpToPoint jumpToPoint;

    [SerializeField] AnimationCurve jumpCurve;


    public int ScoreAmount = 0;
    public int FuelAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        GameManager.Instance.CurrentTreasure += ScoreAmount;
        GameManager.Instance.Platform.CurrentFuel += FuelAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        rb = GetComponent<Rigidbody2D>();

        mainCollider.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector3.zero;

        StartCoroutine(MoveToPoint());
    }

    private IEnumerator MoveToPoint()
    {
        // get the target location this pickup goes too
        Vector3 target = GetTarget();

        // save the position this item was collected at
        Vector2 startPos = transform.position;


        float durration = Mathf.Clamp(10 * Vector3.Distance((Vector2)transform.position, target),30,40);
        float count = 0;

        while ((Vector3.Distance((Vector2)transform.position, target)) > .25)
        {
            target = GetTarget();

            Vector2 newPos = Vector3.Lerp(startPos,target,count/durration);

            newPos = newPos + Vector2.up * jumpCurve.Evaluate(count / durration) * (Mathf.Clamp(Vector3.Distance(startPos, target)/2,0,5)) ;

            //Debug.Log($"Moving Pickup to {newPos} at {count / durration} progress ({jumpCurve.Evaluate(count / durration)})");

            this.transform.position = newPos;

            count++;
            yield return new WaitForFixedUpdate();
        }

        Destroy(this.gameObject);

    }

    Vector2 GetTarget()
    {
        switch (jumpToPoint)
        {
            

            case JumpToPoint.Score:

                return GameManager.Instance.ScoreAnchor.transform.position;

                

            case JumpToPoint.Platform:

                return GameManager.Instance.Platform.transform.position;

            default:

                return transform.position;
        }
    }
}
