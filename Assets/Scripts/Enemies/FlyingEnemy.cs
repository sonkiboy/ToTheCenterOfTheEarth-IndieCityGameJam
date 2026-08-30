using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{

    Rigidbody2D rb;
    Collider2D collider;
    SpriteRenderer spriteRenderer;

    public Vector2 Speed2D = Vector2.one;

    public Transform Target;
    [SerializeField] Vector2 targetPosition;
    public float OffsetDistance = 0f;
    public float MaxVelocity = 4f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Target  == null)
        {
            Target = GameManager.Instance.Player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(OffsetDistance != 0f)
        {
            Vector2 dir = (Target.position - transform.position).normalized;
            targetPosition = (Vector2)Target.position - (dir * OffsetDistance);
            
        }
        else
        {
            targetPosition = Target.position;
        }

        if (spriteRenderer != null)
        {
            if (targetPosition.x > this.transform.position.x && spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
            else if (targetPosition.x < this.transform.position.x && spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;

            }
        }
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(rb.position, targetPosition) > OffsetDistance)
        {
            Vector2 dir = (targetPosition - (Vector2)rb.position).normalized;
            rb.AddForce(dir * Speed2D) ;
        }

        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, MaxVelocity);
    }
}
