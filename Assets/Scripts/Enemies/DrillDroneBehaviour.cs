using System.Collections;
using UnityEngine;

public class DrillDroneBehaviour : Enemy
{

    BoxCollider2D drillCollider;
    Animator animator;


    [SerializeField] private Vector2 _dir;

    [SerializeField] bool isDrilling = false;
    public Vector2 MoveDirection
    {
        get { return _dir; }
        set
        {
            _dir = value;

            for (int i = 0; i < 4; i++)
            {
                if (value == SonkUtilities.FourDirections[i])
                {
                    animator.SetInteger("Direction", i);
                    drillCollider.transform.rotation = Quaternion.LookRotation(Vector3.forward, _dir) * Quaternion.Euler(0, 0, 90);
                    break;
                }
            }
        }
    }

    public int DrillDamage = 2;
    public float DrillRate = 4;
    public float DecentSpeed = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        drillCollider = transform.Find("DrillCollider").GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        MoveDirection = _dir;

        StartCoroutine(DrillRoutine());
        StartCoroutine(MoveDrill());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DrillRoutine()
    {
        Collider2D foundCollisions =  Physics2D.OverlapBox(drillCollider.bounds.center, drillCollider.size, 0f, ~drillCollider.excludeLayers); ;
        while (isActiveAndEnabled)
        {
            foundCollisions = Physics2D.OverlapBox(drillCollider.bounds.center, drillCollider.size, drillCollider.transform.rotation.eulerAngles.z, ~drillCollider.excludeLayers);

            // if there were any colliders found in the drill bounds...
            if (foundCollisions != null)
            {
                if (foundCollisions.gameObject != this.gameObject && foundCollisions.gameObject != drillCollider.gameObject)
                {

                    isDrilling = true;
                    //Debug.Log($"hitting {foundCollisions.name}");

                    // if the collider was a Tile, deal damage to its Tile Behavior component
                    if (foundCollisions.gameObject.tag == "Tile")
                    {
                        TileBehavior tileBehavior = foundCollisions.GetComponent<TileBehavior>();
                        tileBehavior.DamageTile(true,DrillDamage);
                    }

                    // if the collider was instead an Enemy, deal damage to its Enemy component
                    else if (foundCollisions.gameObject.tag == "Enemy")
                    {
                        Enemy enemy = foundCollisions.gameObject.GetComponent<Enemy>();
                        enemy.Health -= (DrillDamage);
                    }
                    yield return new WaitForSeconds(1 / DrillRate + Random.Range(-.15f, .15f));

                }
                else
                {
                    isDrilling = false;
                }
            }
            else
            {
                isDrilling = false;

            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator MoveDrill()
    {
        yield return new WaitForSeconds(1 / (DecentSpeed));
        while (isActiveAndEnabled)
        {
            if (isDrilling == false)
            {
                for (int i = 0; i < 4; i++)
                {
                    transform.position = (Vector2)transform.position + (MoveDirection.normalized * .25f);

                    yield return new WaitForSeconds(1 / (DecentSpeed));
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(1 / (DecentSpeed));
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
