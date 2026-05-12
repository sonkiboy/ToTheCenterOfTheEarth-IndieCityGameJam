using System.Collections;
using UnityEngine;

public class VibArmBehavior : MonoBehaviour
{

    public Transform TargetPosition;
    

    public float InitialSearchRadius = 2f;
    public float Speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveArm()
    {
        Collider2D foundTileCollider = null;

        for (int i = 1; i < 4; i++)
        {
            foundTileCollider = Physics2D.OverlapCircle(TargetPosition.position, InitialSearchRadius * (float)i, LayerMask.GetMask("Tile"));

            if(foundTileCollider != null )
            {
                break;
            }
        }

        if( foundTileCollider != null )
        {

        }
    }

    IEnumerator MoveArmToPoint(Vector2 targetPosition)
    {


        yield return null;
    }
}
