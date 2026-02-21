using System.Collections;
using UnityEngine;

public class RandomLetterShake : MonoBehaviour
{
    public Vector2Int YRange;
    public Vector2Int XRange;
    public float ShakeRate = 1f;

    private Vector2 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
        StartCoroutine(ShakeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShakeRoutine()
    {
        int randY;
        int randX;

        while(true)
        {

            randX = Random.Range(XRange.x, XRange.y);
            randY = Random.Range(YRange.x, YRange.y);
            this.transform.position = startingPos + new Vector2(randX,randY);

            yield return new WaitForSeconds(1f/ShakeRate);
        }
    }
}
