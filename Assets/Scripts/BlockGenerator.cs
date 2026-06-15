using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.Collections.AllocatorManager;

public class BlockGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject ground;
    [SerializeField] GameObject platform;

    [SerializeField] int PlatDepth;
    [SerializeField] int PlatSize;


    [SerializeField] GameObject[] Edges;
    [SerializeField] GameObject[] Blocks;
    [SerializeField] GameObject[] Features;

    //[SerializeField] int GenerateWidth = 10;
    //[SerializeField] int GenerateHeight = 10;

    [SerializeField]public Vector2Int chunkSize = new Vector2Int(17, 17);



    [SerializeField] int FuelChance = 30;

    [Range(0, 100)]
    [SerializeField] int TreasureChance = 20;

    [SerializeField] float ScrollDownSpeed = .5f;

    [SerializeField] Vector2Int FeatureRange = Vector2Int.up * 5;

    void Start()
    {
        ResetGeneration();
        GenerateChunk(chunkSize.y, chunkSize.y);
        GenerateChunk(0, 0);
        StartCoroutine(ChunkCounter());

    }

    // Update is called once per frame
    void Update()
    {

    }

    
    private List<GameObject> GetAllBlocks()
    {
        List<GameObject> blocks = new List<GameObject>();

        for (int i = 0; i < ground.transform.childCount; i++)
        {
            blocks.Add(ground.transform.GetChild(i).gameObject);
        }

        return blocks;
    }

    public void ResetGeneration()
    {
        List<GameObject> blocks = GetAllBlocks();

        foreach (GameObject block in blocks)
        {
            DestroyImmediate(block.gameObject);
        }
    }

    private GameObject RandomizeBlock()
    {

        List<TileBehavior> tileBehaviors = new List<TileBehavior>();


        foreach (GameObject tile in Blocks)
        {
            tileBehaviors.Add(tile.GetComponent<TileBehavior>());
        }

        int sum = 0;

        foreach (TileBehavior tile in tileBehaviors)
        {
            sum += tile.Config.SpawnChance;
        }


        GameObject spawnedBlock = Blocks[0];

        int randomNum = Random.Range(1, sum);

        int count = 0;

        for (int i = 0; i < Blocks.Length; i++)
        {
            if (randomNum < tileBehaviors[i].Config.SpawnChance + count)
            {
                spawnedBlock = Blocks[i];
                break;

            }
            else
            {
                count += tileBehaviors[i].Config.SpawnChance;
            }


        }




        return spawnedBlock;
    }

    //public void GenerateNextLevel()
    //{


    //    float platformSize = PlatSize;
    //    int platDepth = PlatDepth;

    //    int y = -GameManager.Instance.ActuallDepth - GenerateHeight;
    //    //Debug.Log($"Generating Tiles at y : {y}");

    //    Instantiate(Edges[0], new Vector2(0, y), Edges[0].transform.rotation, ground.transform);
    //    Instantiate(Edges[1], new Vector2(chunkSize.x, y), Edges[0].transform.rotation, ground.transform);

    //    for (int x = -GenerateWidth; x <= GenerateWidth; x++)
    //    {
    //        GameObject newBlock = Instantiate(RandomizeBlock(), new Vector2(x, y), RandomizeBlock().transform.rotation, ground.transform);

    //    }
    //}

    public IEnumerator ChunkCounter()
    {
        int nextSpawnLevel = GameManager.Instance.ActuallDepth + chunkSize.y / 2;
        Debug.Log($"Starting Next Chunk Level: {nextSpawnLevel}");
        while (this.isActiveAndEnabled)
        {
            
            //Debug.Log($"next spawn level is {nextSpawnLevel} current: {GameManager.Instance.ActuallDepth}");
            if (GameManager.Instance.ActuallDepth > nextSpawnLevel)
            {
                GenerateChunk(-(nextSpawnLevel + (chunkSize.y / 2)), -GameManager.Instance.ActuallDepth - 1);
                nextSpawnLevel += chunkSize.y;
                
                //Debug.Log("Generating Chunk, next will be at: " +  nextSpawnLevel);


            }
            yield return null;

        }
    }
    public void GenerateChunk(int depth,int platformlevel)
    {
        //chunkSize = new Vector2Int(17, 17);
        Vector2 chunkStartPos = depth * Vector2.up;
        

        bool[,] tileArray = new bool[chunkSize.x, chunkSize.y];

        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                if(y < platformlevel)
                {
                    if(x >= 7 && x < chunkSize.x - 7)
                    {
                        tileArray[x,y] = true;
                    }
                }
            }
        }

        int numOfFeatures = Random.Range(FeatureRange.x, FeatureRange.y);

        // place the dicated number of features into the chunk and fill their spots in the tileArray
        for (int i = 0; i < numOfFeatures; i++)
        {
            LevelFeatureData featureData = Features[Random.Range(0, Features.Length)].GetComponent<LevelFeatureData>();
            if (featureData != null)
            {
                // find a suitable place in the array

                // choose a random position in the array offset from the, if this position is found to be invalid in the check process, change it to -Vector2Int.one
                
                Vector2Int evalStartIndex = new Vector2Int(Random.Range(0, chunkSize.x - featureData.Size.x), Random.Range(0 + featureData.Size.y, chunkSize.y));
                Vector2Int validOriginIndex = evalStartIndex;
                Vector2Int finalOrigin = -Vector2Int.one;

                // its possible that due to the random nature of the placement that a feature is not able to be placed in the chunk. maxPlacementAttempts dictates how many times we can try before giving up
                do
                {


                    // if the tile is empty, star checking if the whole shape can fit here
                    if (tileArray[validOriginIndex.x, validOriginIndex.y] == false)
                    {
                        // area that we will be checking to see if the feature can fit here
                        int maxX = validOriginIndex.x + featureData.Size.x;
                        int maxY = validOriginIndex.y - featureData.Size.y;


                        Vector2Int xIndexRange = new Vector2Int(validOriginIndex.x, maxX);
                        Vector2Int yIndexRange = new Vector2Int(maxY, validOriginIndex.y);

                        finalOrigin = validOriginIndex;

                        // loop through all the array indexes within the range, if we find that one is filled, this is not a valid origin target and we have to restart
                        for (int x = xIndexRange.x; x <= xIndexRange.y; x++)
                        {
                            for (int y = yIndexRange.x; y <= yIndexRange.y; y++)
                            {
                                // this index is already taken, feature cant fit, break out of x y loop and repeat the placement attempts loop
                                if (tileArray[x, y] == true)
                                {
                                    finalOrigin = -Vector2Int.one;
                                    break;
                                }
                            }
                            // this index is already taken, feature cant fit, break out of x y loop and repeat the placement attempts loop
                            if (finalOrigin == -Vector2Int.one)
                            {
                                break;
                            }
                        }
                        // if index passed the checks, break out
                        if (finalOrigin != -Vector2Int.one)
                        {
                            break;
                        }
                        

                    }
                    //Debug.Log($"incrimenting origin index from {validOriginIndex} to {validOriginIndex + Vector2Int.right}");

                    validOriginIndex += Vector2Int.right;
                    if (validOriginIndex.x >= chunkSize.x - featureData.Size.x - 1)
                    {
                        //Debug.Log($"origin index ({validOriginIndex}) overflowed X max {chunkSize.x - featureData.Size.x}, incirmenting Y");

                        validOriginIndex = new Vector2Int(0, validOriginIndex.y + 1);
                        if (validOriginIndex.y >= chunkSize.y - 1)
                        {
                            //Debug.Log($"origin index ({validOriginIndex}) overflowed U max {chunkSize.y}, incirmenting to first possible indext {Vector2Int.up * featureData.Size.y}");

                            validOriginIndex = Vector2Int.up * featureData.Size.y;
                        }
                    }

                } while (validOriginIndex != evalStartIndex);

                // if there was a spot for this feature found, put it in the 
                if (finalOrigin != -Vector2Int.one)
                {
                    // area that we will be checking to see if the feature can fit here
                    int maxX = finalOrigin.x + featureData.Size.x;
                    int maxY = finalOrigin.y - featureData.Size.y;

                    

                    Vector2Int xIndexRange = new Vector2Int(finalOrigin.x, maxX);
                    Vector2Int yIndexRange = new Vector2Int(maxY, finalOrigin.y);

                    // loop through all the array indexes within the range, then fill it with the feature
                    for (int x = xIndexRange.x; x < xIndexRange.y; x++)
                    {
                        for (int y = yIndexRange.x; y < yIndexRange.y; y++)
                        {
                            tileArray[x, y] = true;
                        }
                    }

                    // now instatiate the feature in the scene based on the origin tile index position
                    Instantiate(featureData.gameObject, chunkStartPos + new Vector2(finalOrigin.x, -finalOrigin.y + 1), Quaternion.identity, ground.transform);
                    //Debug.Log($"Spawning {featureData.gameObject} at {chunkStartPos + new Vector2(validOriginIndex.x, -validOriginIndex.y)} ({new Vector2(validOriginIndex.x, -validOriginIndex.y)})");
                }
            }
        }

        //debug
        //string print = "Array post features:\n";
        //for (int y = 0; y < chunkSize.y; y++)
        //{
        //    for (int x = 0; x < chunkSize.x; x++)
        //    {
        //        if (tileArray[x, y])
        //        {
        //            print += "■, ";

        //        }
        //        else
        //        {
        //            print += "□, ";

        //        }
        //    }
        //    print += "\n";
        //}
        //Debug.Log(print);

        // fill the rest of the chunk with random blocks
        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                if (tileArray[x, y] == false)
                {
                    
                    GameObject spawnedTile = Instantiate(RandomizeBlock(), chunkStartPos + new Vector2(x, -y), RandomizeBlock().transform.rotation, ground.transform);
                    tileArray[x, y] = true;
                }
            }
        }

        // spawn a left edge at -1x and chunksize.x+1 as deep as chunksize.y
        for(int y = 0; y < chunkSize.y; y++)
        {
            Instantiate(Edges[0], chunkStartPos +new Vector2(-1,-y), RandomizeBlock().transform.rotation, ground.transform);
            Instantiate(Edges[1], chunkStartPos + new Vector2(chunkSize.x,-y), RandomizeBlock().transform.rotation, ground.transform);
        }
    }



}
