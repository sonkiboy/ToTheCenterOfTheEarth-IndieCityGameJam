using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject ground;
    [SerializeField] GameObject platform;
    PlatformBehavior platformBehavior;

    [SerializeField] GameObject Block;

    [SerializeField] int GenerateWidth = 10;
    [SerializeField] int GenerateHeight = 10;

    [Range(0, 100)]
    [SerializeField] int FuelChance = 30;

    [Range(0, 100)]
    [SerializeField] int TreasureChance = 20;

    [SerializeField] float ScrollDownSpeed = .5f;

    void Start()
    {
        ground = GameObject.Find("Ground");

        platformBehavior = platform.GetComponent<PlatformBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateStart()
    {
        ResetGeneration();

        float platformSize = platform.GetComponent<PlatformBehavior>().Size;
        int platDepth = platform.GetComponent<PlatformBehavior>().Depth;

        Debug.Log($"Unrounded (Size: {platformSize}) | Left: {-(platformSize - 1) / 2}, right: {(platformSize - 1) / 2}");

        int leftPlatSize = -(int)Mathf.Ceil((platformSize - 1) / 2);
        int rightPlatSize = (int)Mathf.Floor((platformSize - 1) / 2);

        Debug.Log($"Rounded | Left: {leftPlatSize}, right: {rightPlatSize}");

        for (int y = -GenerateHeight; y <= GenerateHeight; y++)
        {
            for (int x = -GenerateWidth; x <= GenerateWidth; x++)
            {
                if (y >= platDepth)
                {
                    if(x >= leftPlatSize && x <= rightPlatSize)
                    {
                        continue;
                    }
                }
                GameObject newBlock = Instantiate(Block, new Vector2(x,y),Block.transform.rotation,ground.transform);
                RandomizeBlock(newBlock.GetComponent<BlockBehavior>());
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

    private void RandomizeBlock(BlockBehavior Block)
    {
        int randomNum = Random.Range(1, 101);

        if(randomNum <= TreasureChance)
        {
            Block.Type = BlockBehavior.RockType.Treasure;
        }
        else if(randomNum <= FuelChance)
        {
            Block.Type = BlockBehavior.RockType.Fuel;
        }
        else
        {
            Block.Type = BlockBehavior.RockType.Normal;
        }

    }

    public void GenerateNextLevel()
    {

        
        float platformSize = platform.GetComponent<PlatformBehavior>().Size;
        int platDepth = platform.GetComponent<PlatformBehavior>().Depth;

        int y = -GenerateHeight;

        for (int x = -GenerateWidth; x <= GenerateWidth; x++)
        {
            GameObject newBlock = Instantiate(Block, new Vector2(x, y), Block.transform.rotation, ground.transform);
            RandomizeBlock(newBlock.GetComponent<BlockBehavior>());
        }
    }

    public IEnumerator IncrimentLevel()
    {
        platform.GetComponent<PlatformBehavior>().isDecending = true;

        //Debug.Log($"In BlockGen: IncrimentLevel hit");

        for (int i = 0; i < 15;)
        {
            Vector2 scrollAmount = new Vector2(0, 0.06f);

            //ground.GetComponent<Rigidbody2D>().MovePosition((Vector2)ground.transform.position + scrollAmount);

            if(platformBehavior.CurrentFuel > 0)
            {
                platformBehavior.CurrentFuel -= platformBehavior.FuelConsumption;
                ground.transform.position += (Vector3)scrollAmount;

                i++;
            }
            
            

            //Debug.Log($"In BlockGen: Loop {i}, moving ground position to {ground.transform.position} ({scrollAmount})");

            yield return new WaitForSeconds(ScrollDownSpeed);
        }

        ground.transform.position = new Vector2(0, Mathf.RoundToInt(ground.transform.position.y));

        GenerateNextLevel();

        platform.GetComponent<PlatformBehavior>().isDecending = false;
    }



}
