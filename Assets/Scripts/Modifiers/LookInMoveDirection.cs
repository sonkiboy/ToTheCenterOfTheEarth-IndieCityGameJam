using UnityEngine;

public class LookInMoveDirection : MonoBehaviour
{

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity != Vector2.zero)
        {
            Debug.Log($"Linear velocity: {rb.linearVelocity}");
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.linearVelocity.normalized) * Quaternion.Euler(0,0,90);
        }
    }

}
