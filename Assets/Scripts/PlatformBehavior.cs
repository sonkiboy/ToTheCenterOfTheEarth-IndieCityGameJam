using System.Collections;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    #region Object and Components

    SpriteRenderer spriteRenderer;
    BoxCollider2D drillCollider;

    BlockGenerator blockGenerator;

    #endregion


    
    public float DecentSpeed = 2;
    public float DrillRate = 2;
    public int DrillDamage = 3;

    bool isDecending = false;

    // Start is called before the first frame update
    void Start()
    {
        drillCollider = transform.Find("DrillCollider").GetComponent<BoxCollider2D>();
        blockGenerator = GameObject.Find("Cavern").GetComponent<BlockGenerator>();


        StartCoroutine(Drill());    
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Drill()
    {
        // just padding to help with errors
        yield return new WaitForSeconds(1f);

        while (true)
        {
            Collider2D[] foundCollisions = Physics2D.OverlapBoxAll(drillCollider.bounds.center,drillCollider.size,0f,~drillCollider.excludeLayers);

            if (foundCollisions.Length > 0)
            {
                Debug.Log("Digging");

                foreach (Collider2D collision in foundCollisions)
                {
                    if (collision.gameObject.tag == "Tile")
                    {
                        TileBehavior tileBehavior = collision.GetComponent<TileBehavior>();

                        tileBehavior.Health -= DrillDamage;
                    }
                }
            }
            else if(foundCollisions.Length == 0)
            {
                if (!isDecending)
                {
                    Debug.Log("Decending");
                    StartCoroutine(Decend());
                }
            }

            yield return new WaitForSeconds(1/DrillRate);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Decend()
    {
        isDecending = true;

        for (int i = 0; i < 4; i++)
        {
            transform.position = (Vector2)transform.position + Vector2.down * .25f;

            yield return new WaitForSeconds(1/DecentSpeed);
            yield return new WaitForFixedUpdate();
        }

        GameManager.Instance.CurrentDepth++;
        blockGenerator.GenerateNextLevel();

        isDecending = false;
    }
}
