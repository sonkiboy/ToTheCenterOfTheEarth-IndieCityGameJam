using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    public float LifeTime = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RunLife());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RunLife()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(this.gameObject);
    }
}
