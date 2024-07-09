using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] int GenerateWidth = 10;
    [SerializeField] int GenerateHeight = 10;


    [SerializeField] int FuelChance = 30;

    [Range(0, 100)]
    [SerializeField] int TreasureChance = 20;

    [SerializeField] float ScrollDownSpeed = .5f;

    void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateStart()
    {
        ResetGeneration();

        float platformSize = PlatSize;
        int platDepth = PlatDepth;

        Debug.Log($"Unrounded (Size: {platformSize}) | Left: {-(platformSize - 1) / 2}, right: {(platformSize - 1) / 2}");

        int leftPlatSize = -(int)Mathf.Ceil((platformSize - 1) / 2);
        int rightPlatSize = (int)Mathf.Floor((platformSize - 1) / 2);

        Debug.Log($"Rounded | Left: {leftPlatSize}, right: {rightPlatSize}");

        for (int y = -GenerateHeight; y <= GenerateHeight; y++)
        {
            Instantiate(Edges[0], new Vector2(-GenerateWidth - 1, y), Edges[0].transform.rotation, ground.transform);
            Instantiate(Edges[1], new Vector2(GenerateWidth + 1, y), Edges[0].transform.rotation, ground.transform);


            for (int x = -GenerateWidth; x <= GenerateWidth; x++)
            {
                if (y >= platDepth)
                {
                    if(x >= leftPlatSize && x <= rightPlatSize)
                    {
                        continue;
                    }
                }

                GameObject block = RandomizeBlock();
                Instantiate(block, new Vector2(x, y), block.transform.rotation, ground.transform);
                
            }
        }
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

        foreach(TileBehavior tile in tileBehaviors)
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

    public void GenerateNextLevel()
    {

        
        float platformSize = PlatSize;
        int platDepth = PlatDepth;

        int y = -GameManager.Instance.CurrentDepth - GenerateHeight;
        //Debug.Log($"Generating Tiles at y : {y}");

        Instantiate(Edges[0], new Vector2(-GenerateWidth - 1, y), Edges[0].transform.rotation, ground.transform);
        Instantiate(Edges[1], new Vector2(GenerateWidth + 1, y), Edges[0].transform.rotation, ground.transform);

        for (int x = -GenerateWidth; x <= GenerateWidth; x++)
        {
            GameObject newBlock = Instantiate(RandomizeBlock(), new Vector2(x, y), RandomizeBlock().transform.rotation, ground.transform);
            
        }
    }

    



}
