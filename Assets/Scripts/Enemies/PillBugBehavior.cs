using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PillBugBehavior : Enemy
{

    #region Obj and Components

    Rigidbody2D rb;
    [SerializeField] BoxCollider2D wallCheck;

    

    #endregion

    

    [Range(0f, 5f)]
    public float Speed = .15f;

    

    Vector2 currentDirection = Vector2.left;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageFlash[0] = transform.Find("Sprite").GetComponent<Renderer>();
        StartCoroutine(ContinuousWallCheck());  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 newVel = new Vector2(currentDirection.x * Speed, rb.linearVelocity.y);
        rb.linearVelocity = newVel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        WallCheck();
    }

    IEnumerator ContinuousWallCheck()
    {
        while (Health > 0)
        {
            WallCheck();
            yield return new WaitForSeconds(1f);
        }
    }

    void WallCheck()
    {
        Collider2D foundWall = Physics2D.OverlapBox(wallCheck.bounds.center, wallCheck.size, 0f, wallCheck.includeLayers);

        if (foundWall != null)
        {

            currentDirection = -currentDirection;

            if (currentDirection.x > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(Vector3.up * 180);
            }
            else if (currentDirection.x < 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            }



        }
    }
    

    

    

    
}
