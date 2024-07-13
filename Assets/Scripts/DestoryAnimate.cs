using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimate : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(DestroyTimer());

    }

    IEnumerator DestroyTimer()
    {
        float time = animator.GetCurrentAnimatorStateInfo(0).length;


        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }


}
