using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{

    Rigidbody2D rb;
    Collider2D collider;

    public Vector2 Speed2D = Vector2.one;

    public Transform Target;
    public float OffsetDistance = 0f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
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
        
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(rb.position, Target.position) > OffsetDistance)
        {
            Vector2 dir = ((Vector2)Target.position - (Vector2)rb.position).normalized;
            rb.AddForce(dir * Speed2D) ;
        }
    }
}
