using UnityEngine;

public class OldBlockGen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public void GenerateStart()
    //{
    //    ResetGeneration();

    //    float platformSize = PlatSize;
    //    int platDepth = PlatDepth;

    //    //Debug.Log($"Unrounded (Size: {platformSize}) | Left: {-(platformSize - 1) / 2}, right: {(platformSize - 1) / 2}");

    //    int leftPlatSize = -(int)Mathf.Ceil((platformSize - 1) / 2) + Mathf.RoundToInt((float)chunkSize.x / 2f);
    //    int rightPlatSize = (int)Mathf.Floor((platformSize - 1) / 2) + Mathf.RoundToInt((float)chunkSize.x / 2f);

    //    //Debug.Log($"Rounded | Left: {leftPlatSize}, right: {rightPlatSize}");

    //    for (int y = chunkSize.y; y >= -chunkSize.y; y--)
    //    {
    //        Instantiate(Edges[0], new Vector2(-1, y), Edges[0].transform.rotation, ground.transform);
    //        Instantiate(Edges[1], new Vector2(chunkSize.x, y), Edges[0].transform.rotation, ground.transform);


    //        for (int x = 0; x < chunkSize.x; x++)
    //        {
    //            if (y >= platDepth)
    //            {
    //                if (x >= leftPlatSize && x <= rightPlatSize)
    //                {
    //                    continue;
    //                }
    //            }

    //            GameObject block = RandomizeBlock();
    //            Instantiate(block, (Vector2)this.transform.position + new Vector2(x, y), block.transform.rotation, ground.transform);

    //        }
    //    }
    //}

    //public void GenerateFlat()
    //{
    //    ResetGeneration();

    //    float platformSize = PlatSize;
    //    int platDepth = PlatDepth;

    //    //Debug.Log($"Unrounded (Size: {platformSize}) | Left: {-(platformSize - 1) / 2}, right: {(platformSize - 1) / 2}");


    //    int leftPlatSize = -(int)Mathf.Ceil((platformSize - 1) / 2) + Mathf.RoundToInt((float)chunkSize.x / 2f);
    //    int rightPlatSize = (int)Mathf.Floor((platformSize - 1) / 2) + Mathf.RoundToInt((float)chunkSize.x / 2f);

    //    //Debug.Log($"Rounded | Left: {leftPlatSize}, right: {rightPlatSize}");

    //    for (int y = chunkSize.y; y >= -chunkSize.y; y--)
    //    {
    //        Instantiate(Edges[0], new Vector2(-1, y), Edges[0].transform.rotation, ground.transform);
    //        Instantiate(Edges[1], new Vector2(chunkSize.x, y), Edges[0].transform.rotation, ground.transform);


    //        for (int x = 0; x < chunkSize.x; x++)
    //        {
    //            if (y >= platDepth)
    //            {
    //                if (x >= leftPlatSize && x <= rightPlatSize)
    //                {
    //                    continue;
    //                }
    //            }

    //            if (y < -2)
    //            {
    //                GameObject block = RandomizeBlock();
    //                Instantiate(block, (Vector2)this.transform.position + new Vector2(x, y), block.transform.rotation, ground.transform);
    //            }


    //        }
    //    }
    //}
}
