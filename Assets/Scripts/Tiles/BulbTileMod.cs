using System.Collections;
using UnityEngine;

public class BulbTileMod : MonoBehaviour
{

    Animator animator;
    ConductiveTileMod selfConduitMod;

    public float IdleTime = 5f;
    public float ChargeTime = 3f;
    public float FlashTime = 2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        selfConduitMod = GetComponent<ConductiveTileMod>();

        StartCoroutine(FlashRoutine()); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FlashRoutine()
    {
        while (isActiveAndEnabled)
        {
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(IdleTime);
            animator.SetTrigger("Charge");
            yield return new WaitForSeconds(ChargeTime);
            selfConduitMod.enabled = true;
            animator.SetTrigger("Flash");
            yield return new WaitForSeconds(FlashTime);
            selfConduitMod.enabled = false;

        }
    }
}
