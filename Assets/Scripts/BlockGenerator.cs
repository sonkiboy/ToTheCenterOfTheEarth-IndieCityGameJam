using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject ground;
    [SerializeField] GameObject platform;

    [SerializeField] int PlatDepth;
    [SerializeField] int PlatSize;


    [SerializeField] GameObject[] Edges;
    //[SerializeField] GameObject[] Blocks;
    //[SerializeField] GameObject[] Features;

    //[SerializeField] int GenerateWidth = 10;
    //[SerializeField] int GenerateHeight = 10;

    [SerializeField] public Vector2Int chunkSize = new Vector2Int(17, 17);
    public Vector2 OffScreenEnemySpawn = Vector2.right * 10;


    public int CurrentLayerIndex = 0;
    int layerStageIndex = 0;

    public LayerSerializedObject[] Layers;

    [SerializeField] float ScrollDownSpeed = .5f;

    private void Awake()
    {
    }

    void Start()
    {
        ResetGeneration();
        //GenerateChunk(chunkSize.y, chunkSize.y);
        //GenerateChunk(0, 0);

        //MultiStageBlockGeneration(20, 20);
        //MultiStageBlockGeneration(chunkSize.y, chunkSize.y);

        //SingleStageBlockGeneration(0, 100 / Layers[CurrentLayerIndex].Stages.Length, 100 / Layers[CurrentLayerIndex].Stages.Length);
        //SingleStageBlockGeneration(0, -(100 / Layers[CurrentLayerIndex].Stages.Length), 0);
        SingleStageBlockGeneration(0,1, 1);
        layerStageIndex++;

        StartCoroutine(ChunkCounter());
        GameManager.Instance.OnDepthChanged += OnLevelUpdated;
        if (GameManager.Instance.GameOptions.IsStaticColorsOn == false)
        {
            GameManager.Instance.PaletteManager.ChangePalette(0f, Layers[CurrentLayerIndex].LayerPallete[0], Layers[CurrentLayerIndex].LayerPallete[1], Layers[CurrentLayerIndex].LayerPallete[2], Layers[CurrentLayerIndex].LayerPallete[3]);
        }

        if (Layers[CurrentLayerIndex].Name == "The Crust")
        {
            GameManager.Instance.ArcadeLEDs.SetCabinetLights(ArcadeLightManager.LightModes.fade, 0);
        }
        else if (Layers[CurrentLayerIndex].Name == "The Jungle")
        {
            GameManager.Instance.ArcadeLEDs.SetCabinetLights(ArcadeLightManager.LightModes.fade, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        GameManager.Instance.OnDepthChanged -= OnLevelUpdated;

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

    private GameObject RandomizeBlock(GameObject[] blockPalette)
    {

        List<TileBehavior> tileBehaviors = new List<TileBehavior>();


        foreach (GameObject tile in blockPalette)
        {
            tileBehaviors.Add(tile.GetComponent<TileBehavior>());
        }

        int sum = 0;

        foreach (TileBehavior tile in tileBehaviors)
        {
            sum += tile.Config.SpawnChance;
        }


        GameObject spawnedBlock = blockPalette[0];

        int randomNum = Random.Range(1, sum);

        int count = 0;

        for (int i = 0; i < blockPalette.Length; i++)
        {
            if (randomNum < tileBehaviors[i].Config.SpawnChance + count)
            {
                spawnedBlock = blockPalette[i];
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
        int nextChunkOrigin = GameManager.Instance.ActuallDepth + chunkSize.y;
        LayerSerializedObject CurrentLayer = Layers[CurrentLayerIndex];
        int numOfStages = CurrentLayer.Stages.Length;
        int stageHeight = 100 / numOfStages;
        int remainderHeight = 100 % numOfStages;

        nextSpawnLevel = stageHeight / 2;
        nextChunkOrigin = stageHeight - 1;
        //Debug.Log($"Starting Next Chunk Level: {nextSpawnLevel}");
        while (this.isActiveAndEnabled)
        {

            //Debug.Log($"next spawn level is {nextSpawnLevel} current: {GameManager.Instance.ActuallDepth}");
            if (GameManager.Instance.ActuallDepth > nextSpawnLevel)
            {
                CurrentLayer = Layers[CurrentLayerIndex];
                numOfStages = CurrentLayer.Stages.Length;
                stageHeight = 100 / numOfStages;
                remainderHeight = 100 % numOfStages;

                

                if (layerStageIndex <= Layers[CurrentLayerIndex].Stages.Length - 1)
                {

                    if (layerStageIndex == 0)
                    {
                        SingleStageBlockGeneration(layerStageIndex, -(nextChunkOrigin), -GameManager.Instance.ActuallDepth - 1);
                        nextSpawnLevel += (stageHeight/2) + (chunkSize.y / 2);
                        nextChunkOrigin += stageHeight;
                        layerStageIndex++;
                    }
                    else if (layerStageIndex == numOfStages - 1)
                    {
                        SingleStageBlockGeneration(layerStageIndex, -(nextChunkOrigin), -GameManager.Instance.ActuallDepth - 1);
                        nextSpawnLevel += ((stageHeight + (stageHeight / 2) - (chunkSize.y / 2)));
                        nextChunkOrigin += ((stageHeight));
                        layerStageIndex++;
                    }
                    else
                    {
                        SingleStageBlockGeneration(layerStageIndex, -(nextChunkOrigin), -GameManager.Instance.ActuallDepth - 1);
                        nextSpawnLevel += (stageHeight);
                        nextChunkOrigin += stageHeight;
                        layerStageIndex++;
                    }
                }
                else
                {
                    GenerateChunk(-(nextChunkOrigin), -GameManager.Instance.ActuallDepth - 1);
                    nextSpawnLevel += chunkSize.y;
                    nextChunkOrigin += chunkSize.y;
                }

                Debug.Log($"INFO:\nCurrent Layer: {CurrentLayer}\nStage Height: {stageHeight}\nRemainder Height: {remainderHeight}\nnext spawn level: {nextSpawnLevel}");


                //Debug.Log("Generating Chunk, next will be at: " +  nextSpawnLevel);


            }
            yield return null;

        }
    }

    public void OnLevelUpdated(object sender, int level)
    {
        while (level > 100)
        {
            level -= 100;
        }

        if (level % (100 / (Layers[CurrentLayerIndex].NumberOfEnemies + 1)) == 0 && level < 100)
        {
            Instantiate(Layers[CurrentLayerIndex].SpawnedEnemies[0], (Vector2)GameManager.Instance.Platform.transform.position + OffScreenEnemySpawn, Quaternion.identity);
            OffScreenEnemySpawn *= Vector2.left;
        }

        if (level % 100 == 0 && level > 10)
        {
            GameManager.Instance.BossManager.StartBossFight(Layers[CurrentLayerIndex].LayerBoss);
        }
    }
    public void GenerateChunk(int depth, int platformlevel)
    {
        //chunkSize = new Vector2Int(17, 17);
        Vector2 chunkStartPos = depth * Vector2.up;


        bool[,] tileArray = new bool[chunkSize.x, chunkSize.y];

        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                if (y < platformlevel)
                {
                    if (x >= 7 && x < chunkSize.x - 7)
                    {
                        tileArray[x, y] = true;
                    }
                }
            }
        }

        int numOfFeatures = Mathf.RoundToInt((chunkSize.y / 100f) * Layers[CurrentLayerIndex].CommonFeatureSpawnRange);

        // COMMON FEATURES 

        // place the dicated number of features into the chunk and fill their spots in the tileArray
        for (int i = 0; i < numOfFeatures; i++)
        {
            LevelFeatureData featureData = Layers[CurrentLayerIndex].CommonFeatures[Random.Range(0, Layers[CurrentLayerIndex].CommonFeatures.Length)].GetComponent<LevelFeatureData>();
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
                    if (validOriginIndex.x > chunkSize.x - featureData.Size.x - 1)
                    {
                        //Debug.Log($"origin index ({validOriginIndex}) overflowed X max {chunkSize.x - featureData.Size.x}, incirmenting Y");

                        validOriginIndex = new Vector2Int(0, validOriginIndex.y + 1);
                        if (validOriginIndex.y > chunkSize.y - 1)
                        {
                            //Debug.Log($"origin index ({validOriginIndex}) overflowed U max {chunkSize.y}, incirmenting to first possible indext {Vector2Int.up * featureData.Size.y}");

                            validOriginIndex = new Vector2Int(0, featureData.Size.y);
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

        int numOfRareFeatures = Mathf.RoundToInt((chunkSize.y / 100f) * Layers[CurrentLayerIndex].RareFeatureSpawnRange);

        for (int i = 0; i < numOfFeatures; i++)
        {
            LevelFeatureData featureData = Layers[CurrentLayerIndex].RareFeatures[Random.Range(0, Layers[CurrentLayerIndex].RareFeatures.Length)].GetComponent<LevelFeatureData>();
            if (featureData != null)
            {
                // find a suitable place in the array

                // choose a random position in the array offset from the, if this position is found to be invalid in the check process, change it to -Vector2Int.one

                Vector2Int evalStartIndex = new Vector2Int(Random.Range(0, chunkSize.x - featureData.Size.x), Random.Range(0 + featureData.Size.y, chunkSize.y));
                Vector2Int validOriginIndex = evalStartIndex;
                Vector2Int finalOrigin = -Vector2Int.one;


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
                    if (validOriginIndex.x > chunkSize.x - featureData.Size.x - 1)
                    {
                        //Debug.Log($"origin index ({validOriginIndex}) overflowed X max {chunkSize.x - featureData.Size.x}, incirmenting Y");

                        validOriginIndex = new Vector2Int(0, validOriginIndex.y + 1);
                        if (validOriginIndex.y > chunkSize.y - 1)
                        {
                            //Debug.Log($"origin index ({validOriginIndex}) overflowed U max {chunkSize.y}, incirmenting to first possible indext {Vector2Int.up * featureData.Size.y}");

                            validOriginIndex = new Vector2Int(0, featureData.Size.y);
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

                    GameObject spawnedTile = Instantiate(RandomizeBlock(Layers[CurrentLayerIndex].BaseBlockPallet), chunkStartPos + new Vector2(x, -y), RandomizeBlock(Layers[CurrentLayerIndex].BaseBlockPallet).transform.rotation, ground.transform);
                    tileArray[x, y] = true;
                }
            }
        }

        // spawn a left edge at -1x and chunksize.x+1 as deep as chunksize.y
        for (int y = 0; y < chunkSize.y; y++)
        {
            Instantiate(Edges[0], chunkStartPos + new Vector2(-1, -y), RandomizeBlock(Layers[CurrentLayerIndex].BaseBlockPallet).transform.rotation, ground.transform);
            Instantiate(Edges[1], chunkStartPos + new Vector2(chunkSize.x, -y), RandomizeBlock(Layers[CurrentLayerIndex].BaseBlockPallet).transform.rotation, ground.transform);
        }
    }


    public void SingleStageBlockGeneration(int stage, int depth, int platformlevel)
    {
        Vector2 chunkStartPos = depth * Vector2.up;

        // create chunk array
        bool[,] tileArray = new bool[chunkSize.x, 100 / Layers[CurrentLayerIndex].Stages.Length];

        // establish where the platform is and what tiles are automatically gone 
        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < 100 / Layers[CurrentLayerIndex].Stages.Length; y++)
            {
                if (y < platformlevel)
                {
                    if (x >= 7 && x < chunkSize.x - 7)
                    {
                        tileArray[x, y] = true;
                    }
                }
            }
        }
        LayerSerializedObject CurrentLayer = Layers[CurrentLayerIndex];
        int numOfStages = CurrentLayer.Stages.Length;
        int stageHeight = 100 / Layers[CurrentLayerIndex].Stages.Length;
        ChunkStageData currentStage = CurrentLayer.Stages[stage];

        // generate the powerup chests
        if (currentStage.PowerupFeatures.Length > 0)
        {
            for (int j = 0; j < currentStage.NumOfPowerUps; j++)
            {
                LevelFeatureData featureData = currentStage.PowerupFeatures[Random.Range(0, currentStage.PowerupFeatures.Length)].GetComponent<LevelFeatureData>();
                if (featureData != null)
                {
                    // find a suitable place in the array
                    // choose a random position in the array offset from the, if this position is found to be invalid in the check process, change it to -Vector2Int.one
                    Vector2Int evalStartIndex = new Vector2Int(Random.Range(0, chunkSize.x - featureData.Size.x), Random.Range(0 + featureData.Size.y, stageHeight));
                    Vector2Int validOriginIndex = evalStartIndex;
                    Vector2Int finalOrigin = -Vector2Int.one;

                    // its possible that due to the random nature of the placement that a feature is not able to be placed in the chunk. maxPlacementAttempts dictates how many times we can try before giving up
                    do
                    {
                        // if the selected tile is not within the same column as the platform, then a hostile tile can spawn there (no hostile tiles directly under drill platform)
                        if (validOriginIndex.x < 7 || validOriginIndex.x >= chunkSize.x - 7)
                        {
                            // if the tile is empty, start checking if the whole shape can fit here
                            if (tileArray[validOriginIndex.x, validOriginIndex.y] == false)
                            {
                                // area that we will be checking to see if the feature can fit here
                                int maxX = validOriginIndex.x + featureData.Size.x;
                                int maxY = validOriginIndex.y - featureData.Size.y;
                                Vector2Int xIndexRange = new Vector2Int(validOriginIndex.x, maxX);
                                Vector2Int yIndexRange = new Vector2Int(maxY, validOriginIndex.y);

                                finalOrigin = validOriginIndex;

                                // loop through all the array indexes within the feature area, if we find that one is filled, this is not a valid origin target and we have to restart
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
                                    if (finalOrigin == -Vector2Int.one) break;
                                }
                                // if index passed the checks, break out
                                if (finalOrigin != -Vector2Int.one) break;
                            }


                        }
                        validOriginIndex += Vector2Int.right;
                        if (validOriginIndex.x > chunkSize.x - featureData.Size.x - 1)
                        {
                            //Debug.Log($"origin index ({validOriginIndex}) overflowed X max {chunkSize.x - featureData.Size.x}, incirmenting Y");

                            validOriginIndex = new Vector2Int(0, validOriginIndex.y + 1);
                            if (validOriginIndex.y > stageHeight - 1)
                            {
                                validOriginIndex = new Vector2Int(0, featureData.Size.y);
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
                    }
                    else
                    {
                        Debug.LogAssertion("Failed to spawn power up tile");

                    }
                }
            }
        }

        // generate features
        if (currentStage.StageFeatures.Length > 0)
        {
            for (int j = 0; j < currentStage.NumOfFeatures; j++)
            {
                LevelFeatureData featureData = currentStage.StageFeatures[Random.Range(0, currentStage.StageFeatures.Length)].GetComponent<LevelFeatureData>();
                if (featureData != null)
                {
                    // find a suitable place in the array
                    // choose a random position in the array offset from the, if this position is found to be invalid in the check process, change it to -Vector2Int.one
                    Vector2Int evalStartIndex = new Vector2Int(Random.Range(0, chunkSize.x - featureData.Size.x), Random.Range(0 + featureData.Size.y, stageHeight));
                    Vector2Int validOriginIndex = evalStartIndex;
                    Vector2Int finalOrigin = -Vector2Int.one;

                    // its possible that due to the random nature of the placement that a feature is not able to be placed in the chunk. maxPlacementAttempts dictates how many times we can try before giving up
                    do
                    {
                        if (validOriginIndex.x < 7 || validOriginIndex.x >= chunkSize.x - 7)
                        {
                            // if the tile is empty, start checking if the whole shape can fit here
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
                                    if (finalOrigin == -Vector2Int.one) break;
                                }
                                // if index passed the checks, break out
                                if (finalOrigin != -Vector2Int.one) break;
                            }
                        }

                        validOriginIndex += Vector2Int.right;
                        if (validOriginIndex.x > chunkSize.x - featureData.Size.x - 1)
                        {
                            //Debug.Log($"origin index ({validOriginIndex}) overflowed X max {chunkSize.x - featureData.Size.x}, incirmenting Y");

                            validOriginIndex = new Vector2Int(0, validOriginIndex.y + 1);
                            if (validOriginIndex.y > stageHeight - 1)
                            {
                                validOriginIndex = new Vector2Int(0, featureData.Size.y);
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
                    }
                }
            }
        }

        // generate holstile tiles
        if (currentStage.HostileFeatures.Length > 0)
        {
            for (int j = 0; j < currentStage.NumOfHostileFeatures; j++)
            {
                LevelFeatureData featureData = currentStage.HostileFeatures[Random.Range(0, currentStage.HostileFeatures.Length)].GetComponent<LevelFeatureData>();
                if (featureData != null)
                {
                    // find a suitable place in the array
                    // choose a random position in the array offset from the, if this position is found to be invalid in the check process, change it to -Vector2Int.one
                    Vector2Int evalStartIndex = new Vector2Int(Random.Range(0, chunkSize.x - featureData.Size.x), Random.Range(0 + featureData.Size.y, stageHeight));
                    Vector2Int validOriginIndex = evalStartIndex;
                    Vector2Int finalOrigin = -Vector2Int.one;

                    // its possible that due to the random nature of the placement that a feature is not able to be placed in the chunk. maxPlacementAttempts dictates how many times we can try before giving up
                    do
                    {
                        if (validOriginIndex.x < 7 || validOriginIndex.x >= chunkSize.x - 7)
                        {
                            // if the tile is empty, start checking if the whole shape can fit here
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
                                    if (finalOrigin == -Vector2Int.one) break;
                                }
                                // if index passed the checks, break out
                                if (finalOrigin != -Vector2Int.one) break;
                            }
                        }
                        validOriginIndex += Vector2Int.right;
                        if (validOriginIndex.x > chunkSize.x - featureData.Size.x - 1)
                        {
                            //Debug.Log($"origin index ({validOriginIndex}) overflowed X max {chunkSize.x - featureData.Size.x}, incirmenting Y");

                            validOriginIndex = new Vector2Int(0, validOriginIndex.y + 1);
                            if (validOriginIndex.y > stageHeight - 1)
                            {
                                validOriginIndex = new Vector2Int(0, featureData.Size.y);
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
                    }
                }
            }
        }
        // fill the rest of the chunk with random blocks


        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < stageHeight; y++)
            {
                if (tileArray[x, y] == false)
                {

                    GameObject spawnedTile = Instantiate(RandomizeBlock(currentStage.BaseBlockPallet), chunkStartPos + new Vector2(x, -y), Quaternion.identity, ground.transform);
                    tileArray[x, y] = true;
                }
            }
        }

        for (int y = 0; y < stageHeight; y++)
        {
            Instantiate(Edges[0], chunkStartPos + new Vector2(-1, -y), Quaternion.identity, ground.transform);
            Instantiate(Edges[1], chunkStartPos + new Vector2(chunkSize.x, -y), Quaternion.identity, ground.transform);
        }

    }
    public void MultiStageBlockGeneration(int depth, int platformlevel)
    {
        //create the container object


        Vector2 chunkStartPos = depth * Vector2.up;

        // create chunk array
        bool[,] tileArray = new bool[chunkSize.x, chunkSize.y];

        // establish where the platform is and what tiles are automatically gone 
        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                if (y < platformlevel)
                {
                    if (x >= 7 && x < chunkSize.x - 7)
                    {
                        tileArray[x, y] = true;
                    }
                }
            }
        }

        LayerSerializedObject CurrentLayer = Layers[CurrentLayerIndex];
        int numOfStages = CurrentLayer.Stages.Length;
        int stageHeight = chunkSize.y / numOfStages;
        int remainderHeight = chunkSize.y % numOfStages;

        Debug.Log($"Chunk Data: Stage Count {numOfStages}| Height {stageHeight}| Remainer {remainderHeight}");

        for (int i = 0; i < numOfStages; i++)
        {
            ChunkStageData currentStage = CurrentLayer.Stages[i];

            // generate the powerup chests
            if (currentStage.PowerupFeatures.Length > 0)
            {
                for (int j = 0; j < currentStage.NumOfPowerUps; j++)
                {
                    LevelFeatureData featureData = currentStage.PowerupFeatures[Random.Range(0, currentStage.PowerupFeatures.Length)].GetComponent<LevelFeatureData>();
                    if (featureData != null)
                    {
                        // find a suitable place in the array
                        // choose a random position in the array offset from the, if this position is found to be invalid in the check process, change it to -Vector2Int.one
                        Vector2Int evalStartIndex = new Vector2Int(Random.Range(0, chunkSize.x - featureData.Size.x), Random.Range(0 + featureData.Size.y + (stageHeight * i), stageHeight + (stageHeight * i)));
                        Vector2Int validOriginIndex = evalStartIndex;
                        Vector2Int finalOrigin = -Vector2Int.one;

                        // its possible that due to the random nature of the placement that a feature is not able to be placed in the chunk. maxPlacementAttempts dictates how many times we can try before giving up
                        do
                        {
                            // if the selected tile is not within the same column as the platform, then a hostile tile can spawn there (no hostile tiles directly under drill platform)
                            if (validOriginIndex.x < 7 || validOriginIndex.x >= chunkSize.x - 7)
                            {
                                // if the tile is empty, start checking if the whole shape can fit here
                                if (tileArray[validOriginIndex.x, validOriginIndex.y] == false)
                                {
                                    // area that we will be checking to see if the feature can fit here
                                    int maxX = validOriginIndex.x + featureData.Size.x;
                                    int maxY = validOriginIndex.y - featureData.Size.y;
                                    Vector2Int xIndexRange = new Vector2Int(validOriginIndex.x, maxX);
                                    Vector2Int yIndexRange = new Vector2Int(maxY, validOriginIndex.y);

                                    finalOrigin = validOriginIndex;

                                    // loop through all the array indexes within the feature area, if we find that one is filled, this is not a valid origin target and we have to restart
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
                                        if (finalOrigin == -Vector2Int.one) break;
                                    }
                                    // if index passed the checks, break out
                                    if (finalOrigin != -Vector2Int.one) break;
                                }


                            }
                            validOriginIndex += Vector2Int.right;
                            if (validOriginIndex.x > chunkSize.x - featureData.Size.x - 1)
                            {
                                //Debug.Log($"origin index ({validOriginIndex}) overflowed X max {chunkSize.x - featureData.Size.x}, incirmenting Y");

                                validOriginIndex = new Vector2Int(0, validOriginIndex.y + 1);
                                if (validOriginIndex.y > stageHeight + (stageHeight * i) - 1)
                                {
                                    validOriginIndex = new Vector2Int(0, featureData.Size.y + (stageHeight * i));
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
                        }
                        else
                        {
                            Debug.LogAssertion("Failed to spawn power up tile");

                        }
                    }
                }
            }

            // generate features
            if (currentStage.StageFeatures.Length > 0)
            {
                for (int j = 0; j < currentStage.NumOfFeatures; j++)
                {
                    LevelFeatureData featureData = currentStage.StageFeatures[Random.Range(0, currentStage.StageFeatures.Length)].GetComponent<LevelFeatureData>();
                    if (featureData != null)
                    {
                        // find a suitable place in the array
                        // choose a random position in the array offset from the, if this position is found to be invalid in the check process, change it to -Vector2Int.one
                        Vector2Int evalStartIndex = new Vector2Int(Random.Range(0, chunkSize.x - featureData.Size.x), Random.Range(0 + featureData.Size.y + (stageHeight * i), stageHeight + (stageHeight * i)));
                        Vector2Int validOriginIndex = evalStartIndex;
                        Vector2Int finalOrigin = -Vector2Int.one;

                        // its possible that due to the random nature of the placement that a feature is not able to be placed in the chunk. maxPlacementAttempts dictates how many times we can try before giving up
                        do
                        {
                            if (validOriginIndex.x < 7 || validOriginIndex.x >= chunkSize.x - 7)
                            {
                                // if the tile is empty, start checking if the whole shape can fit here
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
                                        if (finalOrigin == -Vector2Int.one) break;
                                    }
                                    // if index passed the checks, break out
                                    if (finalOrigin != -Vector2Int.one) break;
                                }
                            }

                            validOriginIndex += Vector2Int.right;
                            if (validOriginIndex.x > chunkSize.x - featureData.Size.x - 1)
                            {
                                //Debug.Log($"origin index ({validOriginIndex}) overflowed X max {chunkSize.x - featureData.Size.x}, incirmenting Y");

                                validOriginIndex = new Vector2Int(0, validOriginIndex.y + 1);
                                if (validOriginIndex.y > stageHeight + (stageHeight * i) - 1)
                                {
                                    validOriginIndex = new Vector2Int(0, featureData.Size.y + (stageHeight * i));
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
                        }
                    }
                }
            }

            // generate holstile tiles
            if (currentStage.HostileFeatures.Length > 0)
            {
                for (int j = 0; j < currentStage.NumOfHostileFeatures; j++)
                {
                    LevelFeatureData featureData = currentStage.HostileFeatures[Random.Range(0, currentStage.HostileFeatures.Length)].GetComponent<LevelFeatureData>();
                    if (featureData != null)
                    {
                        // find a suitable place in the array
                        // choose a random position in the array offset from the, if this position is found to be invalid in the check process, change it to -Vector2Int.one
                        Vector2Int evalStartIndex = new Vector2Int(Random.Range(0, chunkSize.x - featureData.Size.x), Random.Range(0 + featureData.Size.y + (stageHeight * i), stageHeight + (stageHeight * i)));
                        Vector2Int validOriginIndex = evalStartIndex;
                        Vector2Int finalOrigin = -Vector2Int.one;

                        // its possible that due to the random nature of the placement that a feature is not able to be placed in the chunk. maxPlacementAttempts dictates how many times we can try before giving up
                        do
                        {
                            if (validOriginIndex.x < 7 || validOriginIndex.x >= chunkSize.x - 7)
                            {
                                // if the tile is empty, start checking if the whole shape can fit here
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
                                        if (finalOrigin == -Vector2Int.one) break;
                                    }
                                    // if index passed the checks, break out
                                    if (finalOrigin != -Vector2Int.one) break;
                                }
                            }
                            validOriginIndex += Vector2Int.right;
                            if (validOriginIndex.x > chunkSize.x - featureData.Size.x - 1)
                            {
                                //Debug.Log($"origin index ({validOriginIndex}) overflowed X max {chunkSize.x - featureData.Size.x}, incirmenting Y");

                                validOriginIndex = new Vector2Int(0, validOriginIndex.y + 1);
                                if (validOriginIndex.y > stageHeight + (stageHeight * i) - 1)
                                {
                                    validOriginIndex = new Vector2Int(0, featureData.Size.y + (stageHeight * i));
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
                        }
                    }
                }
            }
            // fill the rest of the chunk with random blocks
            if (i == numOfStages - 1)
            {
                for (int x = 0; x < chunkSize.x; x++)
                {
                    for (int y = 0 + (stageHeight * i); y < stageHeight + (stageHeight * i) + remainderHeight; y++)
                    {
                        if (tileArray[x, y] == false)
                        {

                            GameObject spawnedTile = Instantiate(RandomizeBlock(currentStage.BaseBlockPallet), chunkStartPos + new Vector2(x, -y), Quaternion.identity, ground.transform);
                            tileArray[x, y] = true;
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < chunkSize.x; x++)
                {
                    for (int y = 0 + (stageHeight * i); y < stageHeight + (stageHeight * i) + remainderHeight; y++)
                    {
                        if (tileArray[x, y] == false)
                        {

                            GameObject spawnedTile = Instantiate(RandomizeBlock(currentStage.BaseBlockPallet), chunkStartPos + new Vector2(x, -y), Quaternion.identity, ground.transform);
                            tileArray[x, y] = true;
                        }
                    }
                }
            }


        }



        // spawn a left edge at -1x and chunksize.x+1 as deep as chunksize.y
        for (int y = 0; y < chunkSize.y; y++)
        {
            Instantiate(Edges[0], chunkStartPos + new Vector2(-1, -y), Quaternion.identity, ground.transform);
            Instantiate(Edges[1], chunkStartPos + new Vector2(chunkSize.x, -y), Quaternion.identity, ground.transform);
        }






        // spawn edges


    }

    public void NextLayer()
    {
        GameManager.Instance.SoundManager.PlayNonDiageticSound(Layers[CurrentLayerIndex].ThemeMusic + "Stop");

        CurrentLayerIndex++;
        if (CurrentLayerIndex >= Layers.Length)
        {
            CurrentLayerIndex = 0;
        }
        layerStageIndex = 0;
        GameManager.Instance.StatTracker.DisplayTitleCard(Layers[CurrentLayerIndex].Name);

        GameManager.Instance.SoundManager.PlayNonDiageticSound(Layers[CurrentLayerIndex].ThemeMusic + "Start");


        if (GameManager.Instance.GameOptions.IsStaticColorsOn == false)
        {
            GameManager.Instance.PaletteManager.ChangePalette(1.5f, Layers[CurrentLayerIndex].LayerPallete[0], Layers[CurrentLayerIndex].LayerPallete[1], Layers[CurrentLayerIndex].LayerPallete[2], Layers[CurrentLayerIndex].LayerPallete[3]);
        }

        if (Layers[CurrentLayerIndex].Name == "The Crust")
        {
            GameManager.Instance.ArcadeLEDs.SetCabinetLights(ArcadeLightManager.LightModes.fade, 0);
        }
        else if (Layers[CurrentLayerIndex].Name == "The Jungle")
        {
            GameManager.Instance.ArcadeLEDs.SetCabinetLights(ArcadeLightManager.LightModes.fade, 1);

        }

    }

}
