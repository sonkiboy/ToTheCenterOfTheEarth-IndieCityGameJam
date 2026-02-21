using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class DropItemsInVolume : MonoBehaviour
{

    BoxCollider2D boundCollider;
    public float SpawnRate = 5;

    public GameObject[] SpawnedObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boundCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(SpawnItems());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(boundCollider.bounds.min.x, boundCollider.bounds.max.x), this.transform.position.y);

            int rand = Random.Range(0, SpawnedObjects.Length);

            Instantiate(SpawnedObjects[rand], spawnPosition, Quaternion.Euler(0,Random.Range(0,180),0));

            yield return new WaitForSeconds(1 / SpawnRate);
        }
        
    }
}
