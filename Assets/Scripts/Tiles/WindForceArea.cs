using System.Collections;
using UnityEngine;

public class WindForceArea : MonoBehaviour
{

    BoxCollider2D boxCollider;

    public Vector2 Direciton;
    public float ForceStrength = 5.0f;
    public float MaxVelocity = 20;
    public LayerMask ForceMask;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Collider2D[] foundEntities = Physics2D.OverlapBoxAll((Vector2)boxCollider.transform.position + boxCollider.offset, boxCollider.size, 0f, ForceMask);
        if(foundEntities.Length > 0 )
        {
            foreach(Collider2D entity in foundEntities)
            {
                if(Mathf.Abs(entity.attachedRigidbody.linearVelocity.magnitude) < MaxVelocity)
                {
                    entity.attachedRigidbody.AddForce(Direciton * ForceStrength);

                }


            }
        }
    }



    
    IEnumerator PushEntity()
    {
        Collider2D foundPlayer = Physics2D.OverlapBox((Vector2)boxCollider.transform.position + boxCollider.offset, boxCollider.size, 0f, LayerMask.GetMask("Player"));

        while (foundPlayer != null)
        {
            
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

}
