using UnityEngine;

[CreateAssetMenu]
public class ChunkStageData : ScriptableObject
{
    public GameObject[] BaseBlockPallet;
    public int NumOfPowerUps = 4;
    public GameObject[] PowerupFeatures;
    public int NumOfFeatures = 4;

    public GameObject[] StageFeatures;
    public int NumOfHostileFeatures = 4;

    public GameObject[] HostileFeatures;
}
