using UnityEngine;
using System;
using System.Collections;
public class GyserBehavior : MonoBehaviour
{

    ParticleSystem particle;
    BoxCollider2D windCollider;
    Transform rayPoint;
    Animator animator;

    [SerializeField] TileBehavior activeTile = null;

    public float ErruptTime = 2f;
    public float IdleTime = 6f;
    public float ErruptDistance = 3f;
    public int TileDamage = 2;
    public float DamageRate = 1f;

    private bool isDamagingTile = false;
    private bool isErrupting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particle = transform.Find("GasParticle").gameObject.GetComponent<ParticleSystem>();
        rayPoint = transform.Find("RayPoint");
        windCollider = transform.Find("ForceCollider").GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        // set active tile target to null
        activeTile = null;

        // disable particles and wind collider
        particle.Stop();
        windCollider.gameObject.SetActive(false);

        // set animator to inactive
        animator.SetBool("IsActive", false);

        StartCoroutine(RunErruption());
        StartCoroutine(DamageTarget());

    }

    IEnumerator DamageTarget()
    {
        while (isActiveAndEnabled)
        {
            if(activeTile != null)
            { 
                while(activeTile != null && activeTile.Health > 0)
                {

                    activeTile.DamageTile(true,TileDamage);
                    yield return new WaitForSeconds(DamageRate);
                }
            
            }
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    IEnumerator RunErruption()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(IdleTime);

            //set animator to the active for the charge up
            animator.SetBool("IsActive", true);

            // the lead time where the gyser is "charging" is half the time of the actual erruption
            yield return new WaitForSeconds(ErruptTime / 2);

            // turn on particles and wind collider
            particle.Play();
            windCollider.gameObject.SetActive(true);

            RaycastHit2D rayOut;

            int loopPerSec = 60;
            float loopTime = ErruptTime / (loopPerSec * ErruptTime);

            for (int i = 0; i < loopPerSec * ErruptTime; i++)
            {
                // fire a ray upwards
                rayOut = Physics2D.Raycast(rayPoint.position, Vector2.up, ErruptDistance, LayerMask.GetMask("Tiles"));

                // if there is a tile in the line of sight, set as the active tile target if not already
                if (rayOut.collider != null)
                {
                    Debug.Log($"Gyser hit {rayOut.collider} which is {rayOut.distance}");

                    if(activeTile != null)
                    {
                        if (rayOut.collider.gameObject != activeTile.gameObject)
                        {
                            activeTile = rayOut.collider.gameObject.GetComponent<TileBehavior>();
                            Debug.Log($"Setting active tile to : {activeTile.name}");
                        }
                    }
                    else
                    {
                        activeTile = rayOut.collider.gameObject.GetComponent<TileBehavior>();
                        Debug.Log($"Setting active tile to : {activeTile.name}");
                    }
                    

                    // take the length of the raycast and use to adjust the position and size of the wind collider
                    windCollider.transform.position = (Vector2)this.transform.position +(Vector2.up)+ (Vector2.right / 2) + (Vector2.up * (rayOut.distance / 2));
                    windCollider.size = Vector2.right + (Vector2.up * rayOut.distance);
                    //Debug.Log($"Setting force collider to positon: {windCollider.transform.position} and size {windCollider.size} based on distance {rayOut.distance}");

                }
                else
                {
                    Debug.Log("Gyser hit nothing");

                    // take the length of the raycast and use to adjust the position and size of the wind collider
                    windCollider.transform.position = ((Vector2)this.transform.position + Vector2.one / 2) + (Vector2.up * (ErruptDistance / 2));
                    windCollider.size = Vector2.right + (Vector2.up * ErruptDistance);
                    //Debug.Log($"Setting force collider to positon: {windCollider.transform.position} and size {windCollider.size} based on max distance");

                }




                yield return new WaitForSeconds(loopTime);
            }

            // set active tile target to null
            activeTile = null;

            // disable particles and wind collider
            particle.Stop();
            windCollider.gameObject.SetActive(false);

            // set animator to inactive
            animator.SetBool("IsActive", false);

            yield return null;
        }
        
    }
}
