using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class InsectQueenBehavior : Enemy
{
    #region Object and COmponents

    private Animator queenAnimator;
    private ParticleSystem explosionParticle;

    private BoxCollider2D bossCollider;
    private WarningBarController warningBarController;

    private SpriteRenderer lazerSprite;

    #endregion

    

    public float Downtime = 11f;
    public float ChargeTime = 4f;
    public float FireDurration = 5f;

    public GameObject RewardChest;


    // Start is called before the first frame update
    void Start()
    {
        damageFlash = GetComponentsInChildren<Renderer>();
        queenAnimator = GetComponent<Animator>();
        bossCollider = GetComponent<BoxCollider2D>();
        warningBarController = GetComponentInChildren<WarningBarController>();
        explosionParticle = GetComponentInChildren<ParticleSystem>();
        lazerSprite = transform.Find("Lazer").gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine(EntranceSequence());
        StartCoroutine(BreakTiles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EntranceSequence()
    {
        // move off screen
        queenAnimator.SetTrigger("MoveOnScreen");
 
        yield return new WaitForSeconds(2f);

        StartCoroutine(BattleSequence()); 

        yield return null;
    }

    IEnumerator BattleSequence()
    {

        while (true)
        {
            queenAnimator.SetTrigger("TriggerIdle");
            yield return new WaitForSeconds(Downtime);

            queenAnimator.SetTrigger("TriggerCharge");
            warningBarController.ShowWarning(ChargeTime);
            yield return new WaitForSeconds(ChargeTime);

            StartCoroutine(FireLazer());
            queenAnimator.SetTrigger("TriggerFire");
            yield return new WaitForSeconds(FireDurration);
        }
    }

    IEnumerator FireLazer()
    {
        lazerSprite.gameObject.SetActive(true);

        float acrewedTime = 0f;

        for (float i = 1; i < 6; i += .5f)
        {
            lazerSprite.size = new Vector2(lazerSprite.size.x, i);

            acrewedTime += .05f;
            yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(FireDurration - acrewedTime);

        lazerSprite.size = new Vector2(lazerSprite.size.x, 1);
        lazerSprite.gameObject.SetActive(false);
    }

    public override void Die()
    {
        // Turn off any relevant coroutines
        StopCoroutine(BreakTiles());
        StopCoroutine(FireLazer());
        StopCoroutine(BattleSequence());

        lazerSprite.gameObject.SetActive(false);
        warningBarController.gameObject.SetActive(false);


        // start the off screen death 
        StartCoroutine(QueenDeathSequence());
    }

    IEnumerator QueenDeathSequence()
    {
        // start the death animator
        queenAnimator.SetTrigger("TriggerDeath");

        

        // play the explosion particle 
        explosionParticle.Play();

        yield return new WaitForSeconds(2f);

        // move off screen
        queenAnimator.SetTrigger("MoveOffScreen");

        yield return new WaitForSeconds(3f);

        if(RewardChest != null) {Instantiate(RewardChest,this.transform.position,Quaternion.identity);}
        GameManager.Instance.PaletteManager.IncrimentPallete(1.5f);


        // destroy the boss object
        base.Die();

        yield return null;
    }

    IEnumerator BreakTiles()
    {
        yield return new WaitForSeconds(.5f);

        while (isDead == false)
        {
            Collider2D[] foundColliders = Physics2D.OverlapBoxAll(bossCollider.bounds.center, bossCollider.size, 0f,LayerMask.GetMask("Tiles"));

            //Debug.Log($"Found: {foundColliders.Length} tiles at position {bossCollider.bounds.center} size {bossCollider.size}");

            if(foundColliders.Length > 0 )
            {
                foreach(Collider2D col in foundColliders)
                {
                    TileBehavior tile = col.GetComponent<TileBehavior>();

                    if(tile != null )
                    {
                        //Debug.Log("Boss breaking tile");

                        tile.BreakTile(false);
                    }
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
